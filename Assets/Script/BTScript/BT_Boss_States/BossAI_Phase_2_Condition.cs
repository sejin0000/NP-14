using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//2페이즈 : 일단 보류->수요일 전까지 1페이즈 완성 목표


public class BossAI_Phase_2_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float bossPatternTime;           // 패턴 간격 : 일단 3~5초 정도 줄까?
    private float currentTime;         // 시간 계산용
    private float percentHP;
    public BossAI_Phase_2_Condition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
        bossPatternTime = bossSO.bossPatternTime;
    }

    public override void Initialize()
    {
        //패턴 간격처리
        currentTime = bossPatternTime;
        //수정됨
        //enemyAI.nav.enabled = true;
    }

    public override Status Update()
    {
        if (percentHP >= 50)//(현재 체력이 50% 미만) => 다음 페이즈로
            return Status.BT_Failure;

        currentTime -= Time.deltaTime;

        percentHP = (bossAI_Dragon.currentHP / bossSO.hp * 100);



        //패턴 주기

        if (currentTime <= 0.3f)
        {
            return Status.BT_Running;
        }
        else
            return Status.BT_Success;
    }
}