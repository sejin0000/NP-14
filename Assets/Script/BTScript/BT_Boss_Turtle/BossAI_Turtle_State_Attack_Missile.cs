using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_State_Attack_Missile : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private Boss_Turtle_SO bossSO;
    public BossAI_Turtle_State_Attack_Missile(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
        bossAI_Turtle.isEndMissile = false;
        Debug.Log($"미사일들어옴체크");

        if (bossAI_Turtle.missileCoolTime <= 0)
        {
            bossAI_Turtle.MissileOn();
        }
    }

    public override Status Update()
    {
        if (bossAI_Turtle.missileCoolTime > 0)
        {
            Debug.Log("미사일 발사 쿨타임 중");
            return Status.BT_Failure;
        }

        if (!bossAI_Turtle.isEndMissile)
        {
            return Status.BT_Success;
        }


        //Debug.Log("미사일실행중");
        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //각 공격 패턴 끝날 때 뭐 하고싶으면 여기 하셈
    }
}
