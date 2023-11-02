using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//플레이어 탐색만 실행 => 컨디션 노드가 탐색의 [성공 / 실패]를 반환함
//★따라서 조건 노드의 정의를 상속받음
public class EnemyState_Chase_ChaseCondition: BTCondition
{
    private GameObject owner;

    public EnemyState_Chase_ChaseCondition(GameObject _owner)
    {
        owner = _owner;
    }

    //오너의 상태를 받아오고, 그에따른 Status 업데이트를 실행
    public override Status Update()
    {
        //플레이어 탐색 부분[TODO : nav로 처리하자]
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player) //오브젝트[플레이어]가 존재함
        {
            float distance = Vector3.Distance(player.transform.position, owner.transform.position);
            if(distance < 10.0f)
            {
                return Status.BT_Success;
            }
        }
        return Status.BT_Failure;
    }
}
