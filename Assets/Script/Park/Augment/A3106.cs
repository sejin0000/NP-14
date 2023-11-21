using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3106 : MonoBehaviourPun
{
    PlayerStatHandler playerStat;
    Player2Skill shieldSkill;
    private float reflectCoeff;
    private int viewID;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();
        viewID = photonView.ViewID;

        // 받는 데미지 감소
        playerStat.HitEvent2 += DefPose;
        
        // 쉴드 반사 처리
        shieldSkill = GetComponent<Player2Skill>();
        playerStat.CanReflect = true;
        playerStat.OnDamageReflectEvent += ReflectDamage;

        // 반사 계수 적용
        reflectCoeff = 0.3f;
        shieldSkill.ReflectCoeff = reflectCoeff;
        shieldSkill.OnGiveReflectCoeffEvent += shieldSkill.SetReflectCoeff;
    }

    private void DefPose(float damage)
    {
        playerStat.HPadd(damage * 0.2f);
    }

    private void ReflectDamage(float damage, int targetViewID)
    {
        PhotonView enemyPV = PhotonView.Find(targetViewID);
        float reflectDamage = damage * reflectCoeff;
        enemyPV.RPC("DecreaseHPByObject", RpcTarget.All, reflectDamage, viewID);
    }

        //// 쉴드 반사계수 부여
        //shield = GetComponentInChildren<Shield>();
        //shield.ReflectCoeff = 0.3f;
}
