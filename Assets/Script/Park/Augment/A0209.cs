using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0209 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private CoolTimeController coolTime;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            coolTime = GetComponent<CoolTimeController>();
            controller.OnRollEvent += Reloading;
        }

    }
    // Update is called once per frame
    void Reloading()
    {
        coolTime.curReloadCool = 0f;
        controller.CallReloadEvent();
    }
}
