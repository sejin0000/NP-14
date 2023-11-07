using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;
using Unity.VisualScripting;

public class EnemyState_Hurt : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;

    private float ActionTime;
    private float currentTime;
    private Vector3 destination;   // 목적지
    private Vector3 RunDirection;

    private float runSpeed;

    //임시
    public GameObject tempTarget;


    //노드 생성자 Awake()
    public EnemyState_Hurt(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

        ActionTime = enemySO.actionTime;
        runSpeed = enemySO.runSpeed;

        //임시 타겟지정
        enemyAI.target = tempTarget;
    }

    //노드 Start() !기본 Update 이후 작업이 일어남
    public override void Initialize()
    {
        if(!enemyAI.isLive)
            EnemyDead();


        if (enemySO.type == EnemyType.Melee || enemySO.type == EnemyType.Ranged)
            PassNord();

        else if (enemySO.type == EnemyType.Coward)
            ;

    }


    //노드 종료 순간 호출
    public override void Terminate()
    {

    }



    //피격 시 반응
    public void DefaultHurt(int damage)
    {
        PassNord();
    }

    public void CowardHurt()
    {
        ;
        //도주 스크립트
    }



    //노드 Update()
    public override Status Update()
    {
        return Status.BT_Running;
    }



    public Status PassNord()
    {
        return Status.BT_Success;
    }

    /*
    public void RunEnemy(Vector3 _targetPos)
    {
        RunDirection = Quaternion.LookRotation(enemyAI.transform.position - _targetPos).eulerAngles;

        currentTime = ActionTime;
        enemyAI.nav.speed = runSpeed;
        //도주 anim = walk랑 똑같이 설정하면될듯

        //플레이어 위치를 받고, 셋 데스티네이션 재설정 -> 이동
        enemyAI.nav.SetDestination(destination);
    }

    */

    public void EnemyDead()
    {
        Debug.Log("적 캐릭터 사망");
    }


    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Reset();
        }
    }
}
