using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0113 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;

    private int nowgold;
    private float nowpower;
    private float oldpower;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            //nowgold = MainGameManager.Instance.Gold;
            nowpower = nowgold * 0.1f;
            oldpower = 0;
            //MainGameManager.Instance.OnGameStartedEvent += setgold;//이걸 돈획득 이벤트에검 

        }
    }
    // Update is called once per frame
    void setgold()
    {
        nowpower = nowgold * 0.1f;
        playerStat.ATK.added += nowpower; // 중요한 부분2
        playerStat.ATK.added -= oldpower; // 중요한 부분2
        oldpower = nowpower;
    }
}
