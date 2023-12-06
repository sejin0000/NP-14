using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//쿨타임 돌리기(아무것도 안함)
public class BossAI_Turtle_Idle : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private EnemySO bossSO;

    public BossAI_Turtle_Idle(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
    }

    public override Status Update()
    {
        if (bossAI_Turtle.thornTornadoCoolTime <= 0 || bossAI_Turtle.missileCoolTime <= 0) //(bossAI_Turtle.rollingCooltime <= 0 || bossAI_Turtle.thornTornadoCoolTime <= 0 || bossAI_Turtle.missileCoolTime <= 0)
        {
            Debug.Log("쿨타임 체크가 완료되었습니다.");
            return Status.BT_Failure;
        }
        //쿨타임 체크 = 모든 쿨타임 중 하나라도 쿨타임이 다 돌아있는 패턴이 있다면 성공 반환 시키고 루트리턴
        
        //업데이트 : 모든 쿨타임 시간에 따라 업데이트
        //bossAI_Turtle.rollingCooltime -= Time.deltaTime;
        bossAI_Turtle.thornTornadoCoolTime -= Time.deltaTime;
        bossAI_Turtle.missileCoolTime -= Time.deltaTime;







        //모든 쿨타임이 0 초과라면 계속 런닝 

        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //각 공격 패턴 끝날 때 뭐 하고싶으면 여기 하셈
    }
}
