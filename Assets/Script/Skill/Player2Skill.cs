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
        pv.RPC("CreateAshield",RpcTarget.AllBuffered);
        Invoke("SkillEnd", 3);
        base.SkillStart();
    }

    public override void SkillEnd()
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }

    [PunRPC]
    private void CreateAshield()
    {
        Debug.Log("��ų ��� �Ϸ�");

        shieldOBJ = Instantiate(shieldPrefab);
        shieldOBJ.transform.SetParent(MainGameManager.Instance.InstantiatedPlayer.transform);
        shieldOBJ.transform.position = MainGameManager.Instance.InstantiatedPlayer.transform.position;
    }
}