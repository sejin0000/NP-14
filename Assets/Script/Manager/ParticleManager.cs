using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    NONE,
    NORMAL,
    SPECIAL
}


public class ParticleManager : LocalSingleton<ParticleManager>
{
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Dictionary<string, GameObject> prefabDict;

    private void Awake()
    {
        base.Awake();
        Initialize();
    }

    private void Initialize()
    {
        prefabDict = new Dictionary<string, GameObject>();

        // 리스트에 올려둔 이펙트 Dict에 추가하여 캐싱
        foreach(var prefab in prefabs)
            prefabDict.Add(prefab.name, prefab);
    }

    /// <summary>
    /// <para>파티클 오브젝트를 불러옵니다.</para>
    /// <para>캐싱 경로는 Resources/Particle/... 입니다.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parents"></param>
    static public void PlayEffect(string name, Vector3 pos, Transform parents=null)
    {
        var tempDict = Instance.prefabDict;

        if (!CheckContainKey(name))
            return;

        GameObject prefab = Instantiate(tempDict[name], parents);
        prefab.transform.position = pos;

        prefab.GetComponent<ParticleSystem>().Play();
    }

    static private bool CheckContainKey(string name)
    {
        var tempDict = Instance.prefabDict;
        if (!tempDict.ContainsKey(name))
        {
            Debug.Log(name + " is not Contained audioClips.");
            bool result = TryCachingClip(name);

            if (!result)
                return false;
        }
        return true;
    }

    static private bool TryCachingClip(string name)
    {
        var clipDict = Instance.prefabDict;

        if (!clipDict.ContainsKey(name))
        {
            var clip = Resources.Load<GameObject>("Prefabs/Particle/" + name);

            if (clip == null)
            {
                Debug.LogError("Can't find " + name);
                return false;
            }

            clipDict.Add(clip.name, clip);
        }
        return true;
    }
}
