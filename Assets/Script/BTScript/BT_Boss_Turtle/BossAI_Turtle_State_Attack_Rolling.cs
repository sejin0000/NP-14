using ExitGames.Client.Photon.StructWrapping;
using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_State_Attack_Rolling : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private Boss_Turtle_SO bossSO;

    public BossAI_Turtle_State_Attack_Rolling(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
        //회전 전 세팅
        bossAI_Turtle.RollStart();
    }

    public override Status Update()
    {
        if (bossAI_Turtle.rollingCooltime > 0)
        {
            Debug.Log("구르기 쿨타임 중");
            return Status.BT_Success;
        }




        //롤링 업데이트
        if (bossAI_Turtle.rolling)
        {
            Debug.Log("구르기 성공");
            bossAI_Turtle._rigidbody2D.velocity = bossAI_Turtle.direction * bossSO.enemyMoveSpeed * Time.deltaTime;
        }

        if (!bossAI_Turtle.rolling)
        {
            Debug.Log("구르기 완료, 쿨타임 리셋");
            bossAI_Turtle.rollingCooltime = bossSO.rollingCooltime;
            return Status.BT_Success;
        }

        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //각 공격 패턴 끝날 때 뭐 하고싶으면 여기 하셈
    }
}
