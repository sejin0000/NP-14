using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_State_Attack_InfinityRolling : BTAction

{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private Boss_Turtle_SO bossSO;

    public BossAI_Turtle_State_Attack_InfinityRolling(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
        bossAI_Turtle.PV.RPC("RollStart", RpcTarget.All);
        bossAI_Turtle.isPhase1 = false;
    }
    public override void Terminate()
    {
        //각 공격 패턴 끝날 때 뭐 하고싶으면 여기 하셈
    }
}
