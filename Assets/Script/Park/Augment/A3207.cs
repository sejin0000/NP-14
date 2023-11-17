using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3207 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
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
