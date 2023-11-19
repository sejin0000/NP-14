using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0106 : MonoBehaviourPun
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

            playerStat.KillCatchEvent += DrainPower; // �߿��Ѻκ�
        }
    }
    // Update is called once per frame
    void DrainPower()
    {
        playerStat.ATK.added += 0.01f; // �߿��� �κ�2
    }
}
