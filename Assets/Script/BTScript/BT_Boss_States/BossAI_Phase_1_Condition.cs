using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI_Phase_1_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float bossPatternTime;           // 패턴 간격 : 일단 3~5초 정도 줄까?
    private float currentTime;         // 시간 계산용
    private float percentHP;
    public BossAI_Phase_1_Condition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
        bossPatternTime = bossSO.bossPatternTime;
    }

    public override void Initialize()
    {
        currentTime = bossPatternTime;
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        percentHP = (bossAI_Dragon.currentHP / bossSO.hp * 100);


        if (percentHP > 50)//(현재 체력이 50% 미만) => 다음 페이즈로
            return Status.BT_Failure;



        //1페이즈의 패턴 주기(패턴 간 최소 대기시간 : 0.3f)

        if (currentTime <= 0.3f)
        {
            return Status.BT_Running;
        }
        else
            return Status.BT_Success;
    }
}
