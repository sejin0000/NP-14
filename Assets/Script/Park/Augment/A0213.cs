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
            MainGameManager.Instance.OnGameStartedEvent += startReset;
            MainGameManager.Instance.OnOverCheckEvent += IAmLegend;
        }
    }
    // Update is called once per frame
    void startReset()
    {
        if (statupdesuka) 
        {
            playerStat.ATK.added -= 10f;
            playerStat.HP.added -= 10f;
            playerStat.Speed.added -= 10f;
            playerStat.AtkSpeed.added -= 10f;
            playerStat.BulletSpread.added -= 5f;
            playerStat.SkillCoolTime.added -= 10f;
            playerStat.Critical.added -= 20f;
        }
        statupdesuka=false;
    }
    void IAmLegend() 
    {
        if (MainGameManager.Instance.PartyDeathCount == 2) 
        {
            statupdesuka = true;
            playerStat.ATK.added += 10f;
            playerStat.HP.added += 10f;
            playerStat.Speed.added += 10f;
            playerStat.AtkSpeed.added += 10f;
            playerStat.BulletSpread.added += 5f;
            playerStat.SkillCoolTime.added += 10f;
            playerStat.Critical.added += 20f;
        }
    }
}
