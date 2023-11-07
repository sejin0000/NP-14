using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_Dead_DeadCondition : BTCondition
{
    private GameObject owner;
    private EnemyAI enemyAI;

    public EnemyState_Dead_DeadCondition(GameObject _owner)
    {
        owner = _owner;
        enemyAI = owner.GetComponent<EnemyAI>();
    }

    //오너의 상태를 받아오고, 그에따른 Status 업데이트를 실행
    public override Status Update()
    {
        if (enemyAI.isLive)
            return Status.BT_Failure;
        else
            return Status.BT_Success;
    }
}
