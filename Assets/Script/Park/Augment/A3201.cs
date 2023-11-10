using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3201 : MonoBehaviourPun
{

    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTime;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTime = GetComponent<CoolTimeController>();
            controller.OnRollEvent += Reloading;
        }

    }
    // Update is called once per frame
    void Reloading()
    {
        playerStat.AmmoMax.added += 3;
        coolTime.curReloadCool = 0f;
        controller.CallReloadEvent();
        Invoke("reloadcontrol", 3);
    }
    void reloadcontrol() 
    {
        playerStat.AmmoMax.added -= 3;
    }
}
