using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0222 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnSkillEvent += RollingHeal;
        }
    }
    void RollingHeal()
    {
        playerStat.CurHP += playerStat.HP.total*0.1f;
    }
}
