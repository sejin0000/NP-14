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
        if (photonView.IsMine) 
        {
            playerStat = GetComponent<PlayerStatHandler>();
            viewID = photonView.ViewID;

            playerStat.HitEvent2 += DefPose;

            shieldSkill = GetComponent<Player2Skill>();
            playerStat.CanReflect = true;
            playerStat.OnDamageReflectEvent += ReflectDamage;

            reflectCoeff = 0.3f;
            shieldSkill.ReflectCoeff = reflectCoeff;
            playerStat.ReflectCoeff = reflectCoeff;
            shieldSkill.OnGiveReflectCoeffEvent += shieldSkill.SetReflectCoeff;
        }

    }

    private void DefPose(float damage)
    {
        playerStat.HPadd(damage * 0.2f);
    }

    private void ReflectDamage(float damage, int targetViewID)
    {
        PhotonView enemyPV = PhotonView.Find(targetViewID);
        // TODO targetViewID가 Player일 경우 따로 처리해야함.
        float reflectDamage = damage * reflectCoeff;
        enemyPV.RPC("DecreaseHPByObject", RpcTarget.All, reflectDamage, viewID);
        Debug.Log($"{viewID}에게 {reflectDamage}만큼의 데미지를 반사함.");
    }

        //// 쉴드 반사계수 부여
        //shield = GetComponentInChildren<Shield>();
        //shield.ReflectCoeff = 0.3f;
}
