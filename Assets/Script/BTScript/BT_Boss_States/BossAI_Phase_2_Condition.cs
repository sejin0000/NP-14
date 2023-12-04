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


    private float percentHP;
    public BossAI_Phase_2_Condition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
    }

    public override Status Update()
    {
        percentHP = (bossAI_Dragon.currentHP / bossSO.hp * 100);

        if (percentHP > 50)//(현재 체력이 50% 미만) => 다음 페이즈로
            return Status.BT_Failure;

        return Status.BT_Success;
    }
}