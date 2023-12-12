using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0213 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    public bool statupdesuka;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();
            statupdesuka = false;
            GameManager.Instance.OnStageStartEvent += startReset;
            GameManager.Instance.OnBossStageStartEvent += startReset;
            GameManager.Instance.PlayerLifeCheckEvent += IAmLegend;
        }
    }
    // Update is called once per frame
    void startReset()
    {
        if (statupdesuka) 
        {
            playerStat.ATK.added -= 10f;
            playerStat.Speed.added -= 0.3f;
            playerStat.AtkSpeed.added -= 0.5f;
            playerStat.BulletSpread.added -= 5f;
            playerStat.SkillCoolTime.added -= 2f;
            playerStat.Critical.added -= 40f;
        }
        statupdesuka=false;
    }
    void IAmLegend() 
    {
        if (GameManager.Instance.PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount-1) //데스카운터없음
        {
            statupdesuka = true;

            playerStat.ATK.added += 10f;
            playerStat.Speed.added += 0.3f;
            playerStat.AtkSpeed.added += 0.5f;
            playerStat.BulletSpread.added += 5f;
            playerStat.SkillCoolTime.added += 2f;
            playerStat.Critical.added += 40f;
        }
    }
}
