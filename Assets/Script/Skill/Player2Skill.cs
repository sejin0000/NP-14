using Photon.Pun;
using System;
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

    public event Action<float> OnGiveReflectCoeffEvent;
    public float ReflectCoeff;

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
        int shieldID = shieldOBJ.GetPhotonView().ViewID;
        photonView.RPC("SetShieldParent", RpcTarget.All, shieldID, photonView.ViewID);
        Shield shield = shieldOBJ.GetComponent<Shield>();
        OnGiveReflectCoeffEvent?.Invoke(ReflectCoeff);
        shield.Initialized(shieldHP, shieldScale, shieldSurvivalTime);
        Invoke("SkillEnd", shieldSurvivalTime);        
    }

    [PunRPC]
    private void SetShieldParent(int shieldID, int viewID)
    {
        PhotonView playerPV = PhotonView.Find(viewID);
        PhotonView shieldPV = PhotonView.Find(shieldID);
        shieldPV.transform.SetParent(playerPV.transform);
    }

    public override void SkillEnd()//�ָ� ���� �ƴ϶�� �ôµ� ��û�߿��� ���� ��ų�� ���ǰ� ���� ���̵��¹��
    {
        Destroy(shieldOBJ);
        base.SkillEnd();
    }
    public void SetReflectCoeff(float coeff)
    {
        ReflectCoeff = coeff;
    }
}
