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
    public float shieldHP = 20;
    public float shieldScale=1;
    public float shieldSurvivalTime = 3;

    public void Start()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent += SkillStart;
            isLink = true;
            controller.SkillMinusEvent += SkillLinkOff;
        }
    }
    public override void SkillStart()
    {
        base.SkillStart();
        shieldOBJ = PhotonNetwork.Instantiate("Prefabs/Player/Shield",transform.position,Quaternion.identity);
        Shield shield = shieldOBJ.GetComponent<Shield>();
        shieldOBJ.transform.SetParent(gameObject.transform);
        shield.Initialized(shieldHP, shieldScale, shieldSurvivalTime);
        Invoke("SkillEnd", shieldSurvivalTime);        
    }

    public override void SkillEnd()//애를 별거 아니라고 봤는데 엄청중요함 현재 스킬이 사용되고 나서 쿨이도는방식
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }
}
