using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_Dead_DeadCondition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;

    public BossAI_State_Dead_DeadCondition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
    }

    //오너의 상태를 받아오고, 그에따른 Status 업데이트를 실행
    public override Status Update()
    {
        if (bossAI_Dragon.isLive)
            return Status.BT_Failure;
        else
            return Status.BT_Success;
    }
}
