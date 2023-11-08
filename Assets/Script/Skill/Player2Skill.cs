using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static System.Net.WebRequestMethods;

public class Player2Skill : Skill
{
    private PhotonView pv;
    private GameObject shieldOBJ;

    public override void Awake()
    {
        pv = GetComponent<PhotonView>();
        base.Awake();
    }
    public override void SkillStart()
    {
        shieldOBJ = PhotonNetwork.Instantiate("Prefabs/Player/Shield",transform.position,Quaternion.identity);
        shieldOBJ.transform.SetParent(gameObject.transform);
        Invoke("SkillEnd", 3);
        base.SkillStart();
    }

    public override void SkillEnd()
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }
}
