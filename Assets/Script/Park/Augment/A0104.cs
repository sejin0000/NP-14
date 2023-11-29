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
            bigPower = 0;
            GameManager.Instance.OnStageStartEvent += PowerSet;
            GameManager.Instance.OnBossStageStartEvent += PowerSet;
        }
                
     }
        void PowerSet()
        {
            playerStat.ATK.added -= bigPower;
            Powerset();
            playerStat.ATK.added += bigPower; // 중요한 부분2
        }

        void Powerset() 
        {
        int stage = GameManager.Instance.curStage;
            switch (stage) 
            {
            case 1:
                bigPower = 20;
                break;

            case 2:
                bigPower = 15;
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
