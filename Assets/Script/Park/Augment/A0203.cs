using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0203 : MonoBehaviourPun//����ü�º�� ���ݷ�
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowPower;
    float oldPower;
    float nowSpeed;
    float oldSpeed;
    private void Awake()//���� ź���� ++ = �����ð�����
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowPower = 0;
            oldPower = 0;
            nowSpeed = 0;
            oldSpeed = 0;
            playerStat.HitEvent += SetPower;//�̰� ��ȹ�� �̺�Ʈ���� 
        }
    }
    // Update is called once per frame
    void SetPower()
    {
        nowPower = GetPower();
        nowSpeed = GetSpeed();
        playerStat.ATK.added += nowPower;
        playerStat.ATK.added -= oldPower;
        playerStat.AtkSpeed.added += nowSpeed;
        playerStat.AtkSpeed.added -= oldSpeed;
        oldPower = nowPower;
        oldSpeed = nowSpeed;
        Debug.Log(playerStat.AtkSpeed.added);
    }
    float GetPower() 
    {
        float a = (playerStat.CurHP/ playerStat.HP.total) * 100f;
        int power=0;
        if (a >= 40)
        {
            power = 5;
        }
        else if (a >= 30) 
        {
            power = 15;
        }
        else if (a >= 20)
        {
            power = 15;
        }
        else if (a >= 10)
        {
            power = 20;
        }
        return power;
    }
    float GetSpeed()
    {
        float a = (playerStat.CurHP / playerStat.HP.total) * 100f;
        float power = 0;
        if (a >= 40)
        {
            power = 1.5f;
        }
        else if (a >= 30)
        {
            power = 2.0f;
        }
        else if (a >= 20)
        {
            power = 2.5f;
        }
        else if (a >= 10)
        {
            power = 3;
        }
        return power;
    }
}
