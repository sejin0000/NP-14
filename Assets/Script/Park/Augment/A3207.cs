using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3207 : MonoBehaviourPun
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

            controller.OnAttackEvent += atkCoolTime;
        }
    }
    // Update is called once per frame
    void atkCoolTime()
    {
        coolTimeController.curSkillCool -= 0.3f;
    }
}
