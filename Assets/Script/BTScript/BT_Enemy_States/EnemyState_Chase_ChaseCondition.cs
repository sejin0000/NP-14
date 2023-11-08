using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//플레이어 탐색만 실행 => 컨디션 노드가 탐색의 [성공 / 실패]를 반환함
//★따라서 조건 노드의 정의를 상속받음


//추적 1번 노드-컨디션 노드
public class EnemyState_Chase_ChaseCondition: BTCondition
{
    private GameObject owner;
    private EnemyAI enemyAI;

    public EnemyState_Chase_ChaseCondition(GameObject _owner)
    {
        owner = _owner;
        enemyAI = owner.GetComponent<EnemyAI>();
    }

    //오너의 상태를 받아오고, 그에따른 Status 업데이트를 실행
    public override Status Update()
    {
        if(enemyAI.isChase || enemyAI.target != null)
            return Status.BT_Success;
        else
            return Status.BT_Failure;
    }
}
