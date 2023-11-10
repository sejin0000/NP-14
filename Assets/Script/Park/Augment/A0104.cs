using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
public class A0104 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private MainGameManager gameManager;
    private float bigPower;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();

            MainGameManager.Instance.OnGameStartedEvent += PowerUp;
            MainGameManager.Instance.OnGameEndedEvent += PowerDown;
        }
                
     }
        void PowerUp()
        {
            Powerset();
            playerStat.ATK.added += bigPower; // 중요한 부분2
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
                bigPower = 30;
                break;

            case 2:
                bigPower = 20;
                break;

            case 3:
                bigPower = 10;
                break;

            case 4:
                bigPower = 5;
                break;

            case 5:
                bigPower = 5;
                break;

            default:
                bigPower = 5;
                break;
            }
        }
    
}
