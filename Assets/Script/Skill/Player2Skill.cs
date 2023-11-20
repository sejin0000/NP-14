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

    public override void SkillEnd()//�ָ� ���� �ƴ϶�� �ôµ� ��û�߿��� ���� ��ų�� ���ǰ� ���� ���̵��¹��
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }
}
