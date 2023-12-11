using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;

public enum ClipType
{
    NONE,
    BGM,
    SE
}

// ADDED
public enum BGMList
{
    Ace_Of_Bananas,
    Dragao_Inkomodo,
    Duty_Cycle_GB,
    Strike_Witches_Get_Bitches,
}

public class AudioManager : SingletonPun<AudioManager>
{
    private AudioSource BGMPlayer;
    private GameObject[] SEPlayer;

    [Header("Data")]
    [SerializeField] private AudioMixer mixer;

    [Header("Setup")]
    [SerializeField] private int SEPlayerSize = 8;
    [SerializeField] private List<AudioClip> SEClips;
    [SerializeField] private List<AudioClip> BGMClips;

    [SerializeField] private Dictionary<string, AudioClip> clipDict;

    // ADDED
    [Header("AudioLibrary")]
    public AudioLibrary AudioLibrary;

    public AudioMixer Mixer { get { return mixer; } }

    public override void Initialize()
    {
        InitializeData();
        InitializeObject();
        
        // ADDED
        AudioLibrary = this.gameObject.GetComponent<AudioLibrary>();

        gameObject.AddComponent<PhotonView>();
        photonView.ViewID = 10;
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

        bgmPlayer.transform.parent = Instance.transform;
        sePlayer.transform.parent = Instance.transform;

        //Add Component to Objects
        BGMPlayer = bgmPlayer.AddComponent<AudioSource>();
        SEPlayer = new GameObject[SEPlayerSize];

        BGMPlayer.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/BGM")[0];

        for(int i=0; i<SEPlayerSize; ++i)
        {
            GameObject player = Instantiate(new GameObject("se_obj"), sePlayer.transform);
            var source = player.AddComponent<AudioSource>();
            source.spatialBlend = 1;
            source.minDistance = 1;
            source.maxDistance = 10;
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SE")[0];
        }
    }

    /// <summary>
    /// <para>BGM 오디오 클립을 실행합니다. 클립이 존재하지 않으면 캐싱합니다.</para>
    /// <para>캐싱 경로는 Resources/Audio/BGM/... 입니다.</para>
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="loop"></param>
    static public void PlayBGM(BGMList clipName, float volume = 1f, bool loop = true)
    {
        var clipDict = Instance.clipDict;
        var player = Instance.BGMPlayer;
        var encodedName = EncodeBGMEnum(clipName);
        if (!CheckContainKey(encodedName, ClipType.BGM))
            return;

        player.clip = clipDict[encodedName];
        player.volume = volume;
        player.loop = loop;
        player.Play();
    }

    //static public void PlayBGM(string clipName, float volume=1f, bool loop=true)
    //{
    //    var clipDict = Instance.clipDict;
    //    var player = Instance.BGMPlayer;

    //    if (!CheckContainKey(clipName, ClipType.BGM))
    //        return;

    //    player.clip = clipDict[clipName];
    //    player.volume = volume;
    //    player.loop = loop;
    //    player.Play();
    //}

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
            var source = player.GetComponent<AudioSource>();
            if (!source.isPlaying)
            {
                source.clip = clipDict[clipName];
                source.volume = volume;
                source.loop = false;
                source.gameObject.transform.position = Vector3.zero;
                source.Play();
                return;
            }
        }
    }
    static public void PlaySE(string clipName, Vector3 pos, float volume = 1f)
    {
        var clipDict = Instance.clipDict;

        if (!CheckContainKey(clipName, ClipType.SE))
            return;

        foreach (var player in Instance.SEPlayer)
        {
            var source = player.GetComponent<AudioSource>();
            if (!source.isPlaying)
            {
                source.clip = clipDict[clipName];
                source.volume = volume;
                source.loop = false;
                source.gameObject.transform.position = pos;
                source.Play();
                return;
            }
        }
    }


    static public void PlayClip(AudioClip clip, float volume = 1f)
    {
        foreach (var player in Instance.SEPlayer)
        {
            var temp = player.GetComponent<AudioSource>();
            if (!temp.isPlaying)
            {
                temp.clip = clip;
                temp.volume = volume;
                temp.loop = false;
                temp.Play();
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

    // ADDED
    static private string EncodeBGMEnum(BGMList bgmEnum)
    {
        string bgmName = Enum.GetName(typeof(BGMList), bgmEnum);
        string encodedName = bgmName.Replace('_', ' ');
        return encodedName;
    }
}
