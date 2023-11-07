using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static System.Net.WebRequestMethods;

public class Player2Skill : Skill
{
    private PhotonView pv;
    private GameObject shieldOBJ;
    private GameObject shieldPrefab;




    public override void Awake()
    {
        shieldPrefab = Resources.Load<GameObject>("Prefabs/Shield");
        pv = GetComponent<PhotonView>();
        base.Awake();
    }
    public override void SkillStart()
    {
        int viewID = MainGameManager.Instance.InstantiatedPlayer.GetPhotonView().ViewID;
        PhotonView photonView = PhotonView.Find(viewID);
        photonView.RPC("CreateAshield",RpcTarget.AllBuffered, viewID);
        Invoke("SkillEnd", 3);
        base.SkillStart();
    }

    public override void SkillEnd()
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }

    [PunRPC]
    private void CreateAshield(int viewID)
    {
        Debug.Log("스킬 사용 완료");

        shieldOBJ = Instantiate(shieldPrefab);
        shieldOBJ.transform.SetParent(MainGameManager.Instance.InstantiatedPlayer.transform);
        shieldOBJ.transform.position = MainGameManager.Instance.InstantiatedPlayer.transform.position;
    }
}
