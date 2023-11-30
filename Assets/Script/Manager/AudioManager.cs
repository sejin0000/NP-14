using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;

public enum ClipType
{
    NONE,
    BGM,
    SE
}

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource BGMPlayer;
    private AudioSource[] SEPlayer;

    [Header("Data")]
    [SerializeField] private AudioMixer mixer;

    [Header("Setup")]
    [SerializeField] private int SEPlayerSize = 8;
    [SerializeField] private List<AudioClip> SEClips;
    [SerializeField] private List<AudioClip> BGMClips;

    [SerializeField] private Dictionary<string, AudioClip> clipDict;

    public AudioMixer Mixer { get { return mixer; } }

    private void Awake()
    {
        base.Awake();
        InitializeData();
        InitializeObject();
    }

    // Dictinonary에 오디오 클립 추가
    public void InitializeData()
    {
        clipDict = new Dictionary<string, AudioClip>();

        // BGM
        foreach (var clip in BGMClips)
            clipDict.Add(clip.name, clip);

        // SE
        foreach(var clip in SEClips)
            clipDict.Add(clip.name, clip);
    }

    // AudioManager의 자식으로 AudioSource 컴포넌트 오브젝트 추가, Mixer 설정
    public void InitializeObject()
    {
        GameObject bgmPlayer = new GameObject("BGMPlayer");
        GameObject sePlayer = new GameObject("SEPlayer");

        bgmPlayer.transform.parent = transform;
        sePlayer.transform.parent = transform;

        //Add Component to Objects
        BGMPlayer = bgmPlayer.AddComponent<AudioSource>();
        SEPlayer = new AudioSource[SEPlayerSize];

        BGMPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/BGM")[0];

        for(int i=0; i<SEPlayerSize; ++i)
        {
            SEPlayer[i] = new AudioSource();
            SEPlayer[i] = sePlayer.AddComponent<AudioSource>();
            SEPlayer[i].outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SE")[0];
        }
    }

    /// <summary>
    /// <para>BGM 오디오 클립을 실행합니다. 클립이 존재하지 않으면 캐싱합니다.</para>
    /// <para>캐싱 경로는 Resources/Audio/BGM/... 입니다.</para>
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="loop"></param>
    static public void PlayBGM(string clipName, float volume=1f, bool loop=true)
    {
        var clipDict = Instance.clipDict;
        var player = Instance.BGMPlayer;

        if (!CheckContainKey(clipName, ClipType.BGM))
            return;

        player.clip = clipDict[clipName];
        player.volume = volume;
        player.loop = loop;
        player.Play();
    }

    /// <summary>
    /// <para>SE 오디오 클립을 실행합니다. 클립이 존재하지 않으면 캐싱합니다.</para>
    /// <para>캐싱 경로는 Resources/Audio/SE/... 입니다.</para>
    /// </summary>
    /// <param name="clipName"></param>
    static public void PlaySE(string clipName, float volume=1f)
    {
        var clipDict = Instance.clipDict;

        if (!CheckContainKey(clipName, ClipType.SE))
            return;

        foreach (var player in Instance.SEPlayer)
        {
            if (!player.isPlaying)
            {
                player.clip = clipDict[clipName];
                player.volume = volume;
                player.loop = false;
                player.Play();
                return;
            }
        }
    }

    static public void PlayClip(AudioClip clip, float volume = 1f)
    {
        foreach (var player in Instance.SEPlayer)
        {
            if (!player.isPlaying)
            {
                player.clip = clip;
                player.volume = volume;
                player.loop = false;
                player.Play();
                return;
            }
        }
    }

    static private bool CheckContainKey(string clipName, ClipType clipType)
    {
        var clipDict = Instance.clipDict;
        if(!clipDict.ContainsKey(clipName))
        {
            Debug.Log(clipName + " is not Contained audioClips.");
            bool result = TryCachingClip(clipName, clipType);

            if (!result)
                return false;
        }
        return true;
    }

    static private bool TryCachingClip(string clipName, ClipType clipType)
    {
        var clipDict = Instance.clipDict;

        if (!clipDict.ContainsKey(clipName))
        {
            var clip = Resources.Load<AudioClip>("Audio/"+clipType.ToString()+"/"+clipName);

            if (clip == null)
            {
                Debug.LogError("Can't find " + clipName);
                return false;
            }

            clipDict.Add(clip.name, clip);
        }
        return true;
    }
}
