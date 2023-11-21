using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2201 : MonoBehaviourPun
{
    //기본공격시 구르기 쿨감
    private TopDownCharacterController controller;
    private CoolTimeController playerCool;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerCool = GetComponent<CoolTimeController>();
            controller.OnAttackEvent += RollingCoolTime;
        }

    }
    void RollingCoolTime()
    {
        playerCool.curRollCool -= 0.5f;
        Debug.Log($"기본 공격을 통해 구르기 쿨 2초 감소 이거 로그에 엄청나올듯");
    }
}
