using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0212 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private float bigPower;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            bigPower = 0;
            GameManager.Instance.OnStageStartEvent += PowerUp;
            GameManager.Instance.OnBossStageStartEvent += PowerUp;
        }

    }
    void PowerUp()
    {
        playerStat.ATK.added -= bigPower;
        Powerset();
        playerStat.ATK.added += bigPower; // �߿��� �κ�2
    }
    void PowerDown()
    {
        playerStat.ATK.added -= bigPower;
    }
    void Powerset()
    {
        int stage = GameManager.Instance.curStage;
        switch (stage)
        {
            case 1:
                bigPower = 3;
                break;

            case 2:
                bigPower = 3;
                break;

            case 3:
                bigPower = 6;
                break;

            case 4:
                bigPower = 6;
                break;

            case 5:
                bigPower = 9;
                break;

            case 6:
                bigPower = 9;
                break;



            default:
                bigPower = 15;
                break;
        }
    }
}
