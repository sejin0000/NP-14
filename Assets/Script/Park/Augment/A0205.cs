using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0205 : MonoBehaviourPun//���ù��� ù���� 
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowPower;
    float oldPower;
    bool Isfirst;
    bool ready;
    private void Awake()//���� ź���� ++ = �����ð�����
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowPower = 0;
            oldPower = 0;
            Isfirst = false;
            ready = true;
            controller.OnReloadEvent += SetPower;//�̰� ��ȹ�� �̺�Ʈ���� 
            controller.OnEndAttackEvent += LostPower;
        }
    }
    // Update is called once per frame
    void SetPower()
    {
        if(ready)
        nowPower = playerStat.ATK.total;
        playerStat.ATK.added += nowPower;
        oldPower = nowPower;
        ready = false;
        Isfirst = true;
    }
    void LostPower() 
    {
        if (Isfirst) 
        {
            playerStat.ATK.added -= oldPower;
            ready = true;
        }
        Isfirst = false;

    }
}
