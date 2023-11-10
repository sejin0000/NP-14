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
            bigPower = 0f;
            MainGameManager.Instance.OnGameStartedEvent += PowerUp;
            MainGameManager.Instance.OnGameEndedEvent += PowerDown;
        }

    }
    void PowerUp()
    {
        Powerset();
        playerStat.ATK.added += bigPower;
    }
    void PowerDown()
    {
        playerStat.ATK.added -= bigPower;
    }
    void Powerset()
    {
        int stage = MainGameManager.Instance.stageData.currentStage;
        switch (stage)
        {
            case 1:
                bigPower = 5;
                break;

            case 2:
                bigPower = 5;
                break;

            case 3:
                bigPower = 10;
                break;

            case 4:
                bigPower = 20;
                break;

            case 5:
                bigPower = 30;
                break;

            default:
                bigPower = 30;
                break;
        }
    }
}
