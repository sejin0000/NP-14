using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Photon.Pun;
using UnityEngine.UIElements;

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

    [SerializeField] private float minLength;
    [SerializeField] private float maxLength;

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

    // Dictinonary�� ����� Ŭ�� �߰�
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

    // AudioManager�� �ڽ����� AudioSource ������Ʈ ������Ʈ �߰�, Mixer ����
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
            SEPlayer[i] = new GameObject("se_obj");
            SEPlayer[i].transform.SetParent(sePlayer.transform);

            var source = SEPlayer[i].AddComponent<AudioSource>();
            source.outputAudioMixerGroup = mixer.FindMatchingGroups("Master/SE")[0];
        }
    }

    /// <summary>
    /// <para>BGM ����� Ŭ���� �����մϴ�. Ŭ���� �������� ������ ĳ���մϴ�.</para>
    /// <para>ĳ�� ��δ� Resources/Audio/BGM/... �Դϴ�.</para>
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
    /// <para>SE ����� Ŭ���� �����մϴ�. Ŭ���� �������� ������ ĳ���մϴ�.</para>
    /// <para>ĳ�� ��δ� Resources/Audio/SE/... �Դϴ�.</para>
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
    static public void PlaySE(string clipName, Vector3 pos)
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
                source.loop = false;

                // �Ÿ��� ���� ���� ����
                Vector3 vec;
                if (SceneManager.GetActiveScene().name != "LobbyScene")
                   vec  = GameManager.Instance.clientPlayer.transform.position - pos;
                else
                   vec = LobbyManager.Instance.instantiatedPlayer.transform.position - pos;
                float volume = Mathf.InverseLerp(Instance.maxLength, Instance.minLength, vec.magnitude);
                source.volume = volume;

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
