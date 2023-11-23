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

        // �޴� ������ ����
        playerStat.HitEvent2 += DefPose;
        
        // ���� �ݻ� ó��
        shieldSkill = GetComponent<Player2Skill>();
        playerStat.CanReflect = true;
        playerStat.OnDamageReflectEvent += ReflectDamage;

        // �ݻ� ��� ����
        reflectCoeff = 0.3f;
        shieldSkill.ReflectCoeff = reflectCoeff;
        playerStat.ReflectCoeff = reflectCoeff;
        shieldSkill.OnGiveReflectCoeffEvent += shieldSkill.SetReflectCoeff;
    }

    private void DefPose(float damage)
    {
        playerStat.HPadd(damage * 0.2f);
    }

    private void ReflectDamage(float damage, int targetViewID)
    {
        PhotonView enemyPV = PhotonView.Find(targetViewID);
        // TODO targetViewID�� Player�� ��� ���� ó���ؾ���.
        float reflectDamage = damage * reflectCoeff;
        enemyPV.RPC("DecreaseHPByObject", RpcTarget.All, reflectDamage, viewID);
        Debug.Log($"{viewID}���� {reflectDamage}��ŭ�� �������� �ݻ���.");
    }

        //// ���� �ݻ��� �ο�
        //shield = GetComponentInChildren<Shield>();
        //shield.ReflectCoeff = 0.3f;
}
