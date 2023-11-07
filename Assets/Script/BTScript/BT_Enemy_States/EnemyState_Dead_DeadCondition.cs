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

    //������ ���¸� �޾ƿ���, �׿����� Status ������Ʈ�� ����
    public override Status Update()
    {
        if (enemyAI.isLive)
            return Status.BT_Failure;
        else
            return Status.BT_Success;
    }
}
