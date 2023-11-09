using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2202 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();
            controller.OnRollEvent += Cooltime;
        }

    }


    // Update is called once per frame
    void Cooltime()
    {
        coolTimeController.curSkillCool -= 1f;
        Debug.Log("±¼·¶±â¿¡ ÄðÅ¸ÀÓ 1ÃÊ °¨¼Ò");
    }
}
