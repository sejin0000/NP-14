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
            //MainGameManager.Instance.OnGameStartedEvent += setgold;//�̰� ��ȹ�� �̺�Ʈ���� 

        }
    }
    // Update is called once per frame
    void setgold()
    {
        nowpower = nowgold * 0.1f;
        playerStat.ATK.added += nowpower; // �߿��� �κ�2
        playerStat.ATK.added -= oldpower; // �߿��� �κ�2
        oldpower = nowpower;
    }
}
