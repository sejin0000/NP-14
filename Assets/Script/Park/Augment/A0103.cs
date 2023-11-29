using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0103 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowCoolGAM;
    float oldCoolGAM;
    private void Awake()//난사 탄퍼짐 ++ = 장전시간감소
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
            oldCoolGAM = 0;
            GameManager.Instance.OnStageStartEvent += SetCool;
            GameManager.Instance.OnBossStageStartEvent += SetCool;
        }
    }
    // Update is called once per frame
    void SetCool()
    {
        playerStat.ReloadCoolTime.added -= oldCoolGAM;
        nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
        playerStat.ReloadCoolTime.added += nowCoolGAM;
        oldCoolGAM = nowCoolGAM;
    }
}
