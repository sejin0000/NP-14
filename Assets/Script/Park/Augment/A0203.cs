using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0203 : MonoBehaviourPun//현재체력비례 공격력
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowPower;
    float oldPower;
    float nowSpeed;
    float oldSpeed;
    private void Awake()//난사 탄퍼짐 ++ = 장전시간감소
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowPower = 0;
            oldPower = 0;
            nowSpeed = 0;
            oldSpeed = 0;
            playerStat.HitEvent += SetPower;
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
        Debug.Log(playerStat.ATK.added);
        Debug.Log(playerStat.AtkSpeed.added);
    }
    float GetPower() 
    {
        float hpPercentage = (playerStat.CurHP/ playerStat.HP.total) * 100;
        int power=0;
        if (hpPercentage >= 40)
        {
            power = 5;
        }
        else if (hpPercentage >= 30) 
        {
            power = 15;
        }
        else if (hpPercentage >= 20)
        {
            power = 15;
        }
        else if (hpPercentage >= 10)
        {
            power = 20;
        }
        return power;
    }
    float GetSpeed()
    {
        float hpPercentage = (playerStat.CurHP / playerStat.HP.total) * 100;
        float speed = 0;
        if (hpPercentage >= 40)
        {
            speed = 0.1f;
        }
        else if (hpPercentage >= 30)
        {
            speed = 0.5f;
        }
        else if (hpPercentage >= 20)
        {
            speed = 1f;
        }
        else if (hpPercentage >= 10)
        {
            speed = 1.5f;
        }
        return speed;
    }
}
