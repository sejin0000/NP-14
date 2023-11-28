using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Photon.Pun;
using UnityEditor.Rendering;

public enum ParticleType
{
    NONE,
    NORMAL,
    SPECIAL
}

public class ParticleManager : LocalSingletonPun<ParticleManager>
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

        // ����Ʈ�� �÷��� ����Ʈ Dict�� �߰��Ͽ� ĳ��
        foreach(var prefab in prefabs)
            prefabDict.Add(prefab.name, prefab);
    }

    /// <summary>
    /// <para>��ƼŬ ������Ʈ�� �����մϴ�.</para>
    /// <para>ĳ�� ��δ� Resources/Particle/... �Դϴ�.</para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parents"></param>
    static public void PlayEffectLocal(string name, Vector3 pos, Transform parents)
    {
        var tempDict = Instance.prefabDict;

        if (!CheckContainKey(name))
            return;

        GameObject prefab = Instantiate(tempDict[name], parents);
        prefab.transform.position = pos;

        prefab.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// <para>��ƼŬ ������Ʈ�� �����մϴ�.</para>
    /// <para>ĳ�� ��δ� Resources/Particle/... �Դϴ�.</para>
    /// <para>��� �÷��̾�� ���̴� ��ƼŬ�Դϴ�. </para>
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="pViewID">�θ� �� ������Ʈ�� ViewID</param>
    public void PlayEffect(string name, Vector3 pos, GameObject parent = null)
    {
        PhotonView pv = Instance.GetComponent<PhotonView>();

        int viewID = pv.ViewID;
        int pViewID = int.MinValue;

        if (parent != null)
            pViewID = parent.GetComponent<PhotonView>().ViewID;

        if (!CheckContainKey(name))
            return;

        pv.RPC("SendEffect", RpcTarget.All, viewID, name, pos, pViewID);
    }

    public void PlayEffect(string name, Vector3 pos)
    {
        PhotonView pv = Instance.GetComponent<PhotonView>();

        int viewID = pv.ViewID;

        if (!CheckContainKey(name))
            return;

        pv.RPC("SendEffect", RpcTarget.All, viewID, name, pos);
    }

    [PunRPC]
    public void SendEffect(int viewID, string name, Vector3 pos, int pViewID)
    {
        Debug.Log("[ParticleManager] SendEffect!");

        var tempDict = Instance.prefabDict;
        PhotonView pv = PhotonView.Find(viewID);

        PhotonView pv_p = PhotonView.Find(pViewID);

        GameObject prefab = Instantiate(tempDict[name], pos, Quaternion.identity, pv_p.transform);

        prefab.GetComponent<ParticleSystem>().Play();
    }

    [PunRPC]
    public void SendEffect(int viewID, string name, Vector3 pos)
    {
        Debug.Log("[ParticleManager] SendEffect!");

        var tempDict = Instance.prefabDict;
        PhotonView pv = PhotonView.Find(viewID);

        GameObject prefab = Instantiate(tempDict[name], pos, Quaternion.identity);

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
