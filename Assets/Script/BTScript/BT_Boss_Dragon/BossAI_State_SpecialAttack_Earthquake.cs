using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_SpecialAttack_Earthquake : BTAction
{
    //애니메이션 - 두 팔을 들어올린다

    //워닝 사인 - 맵 전체에 워닝사인을 그려주고, 서서히 차오르게 한다(중심에서 가장자리로 진행) - 통합메서드로 관리

    //애니메이션 - 두 팔을 내려놓는다



    //실제 영향 : 모든 플레이어의 카메라를 흔든다.(플레이어 - 카메라)

    //플레이어는 모든 위치에서 n만큼의 확정적인 피해 받으며 (플레이어에 메서드 추가 1, 2)
    //[기본 넉백거리 + 보스 머리기준 n만큼의 거리]를 추가적으로 밀려난다.

    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO; //총알 공격력 받아야함
    private Transform target;

    private float currentTime;         // 시간 계산용

    public BossAI_State_SpecialAttack_Earthquake(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;
    }

    public override void Initialize()
    {
        currentTime = enemySO.atkDelay;
        enemyAI.PV.RPC("SetStateColor", RpcTarget.All, Color.red);

        target = enemyAI.Target;
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // 공격 주기에 도달하면 공격 실행
            enemyAI.PV.RPC("Fire", RpcTarget.All);
            currentTime = enemySO.atkDelay;
        }



        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > enemySO.attackRange)
        {
            enemyAI.isAttaking = false;
            return Status.BT_Failure; // 노드 종료
        }



        //★★★수정함
        //enemyAI.PV.RPC("Filp", RpcTarget.All);;
        //enemyAI.Filp(owner.transform.position.x, target.transform.position.x);

        return Status.BT_Running;
    }

    public override void Terminate()
    {

    }
}
