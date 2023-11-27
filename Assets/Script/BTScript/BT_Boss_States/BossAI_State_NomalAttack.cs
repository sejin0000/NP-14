using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스페셜 어택 -> 노말 어택 순서로 체크
public class BossAI_State_NomalAttack : BTCondition
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO; //총알 공격력 받아야함
    private Transform target;

    private float currentTime;         // 시간 계산용

    public BossAI_State_NomalAttack(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;
    }

    public override void Initialize()
    {
        currentTime = enemySO.atkDelay;
        enemyAI.PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorBlack, enemyAI.PV.ViewID);

        target = enemyAI.Target;
    }

    public override Status Update()
    {
        //맨처음에 할거 -> 모든 플레이어의 위치를 받아서, 내가 지정한 피벗과 가까운지 확인
        //가깝다면? 특수 패턴 실행 ->멀다면? 실패 반환[노말 패턴으로 바로 넘어감]


        SetAim();

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
    public void SetAim() // 피해량, 플레이어 위치 받아옴
    {
        //플레이어를 바라보도록 설정

        //anim.SetTrigger("Attack"); // 공격 애니메이션

        //공격() 각도 바꿔준 후 -> 생성
        Vector3 direction = (target.transform.position - enemyAI.enemyAim.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        enemyAI.enemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    public override void Terminate()
    {
        enemyAI.PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorOrigin, enemyAI.PV.ViewID);
    }
}
