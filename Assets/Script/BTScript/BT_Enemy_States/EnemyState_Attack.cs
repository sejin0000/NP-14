using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyState_Attack : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO; //총알 공격력 받아야함
    private GameObject target;

    private float currentTime;         // 시간 계산용

    public EnemyState_Attack(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;      
    }

    public override void Initialize()
    {
        currentTime = enemySO.atkdelay;
        SetStateColor();

        target = enemyAI.target;

        SetAim();
        enemyAI.Shoot();
    }

    public override Status Update()
    {
        SetAim();

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // 공격 주기에 도달하면 공격 실행
            enemyAI.Shoot();
            currentTime = enemySO.atkdelay;
        }

        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > enemySO.attackRange)
        {
            enemyAI.isAttaking = false;
            return Status.BT_Failure; // 노드 종료
        }

        if (target == null)
        {
            enemyAI.isAttaking = false;
            enemyAI.isChase = false;
            return Status.BT_Failure;
        }


        //★★★수정함
        enemyAI.isFilp(owner.transform.position.x, target.transform.position.x);


        return Status.BT_Running;
    }

    public override void Terminate()
    {      
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

    private void SetStateColor()
    {
       enemyAI.spriteRenderer.color = Color.black;
    }
}
