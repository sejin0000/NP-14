using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;

//추적
public class EnemyState_Chase : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    private GameObject target;
    public NavMeshAgent nav;
    private float chaseSpeed = 5.0f;

    private float chaseTime = 4f;      // 걷기 시간
    private float currentTime;         // 시간 계산용


    public EnemyState_Chase(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        nav = owner.GetComponent<NavMeshAgent>();
        enemySO = enemyAI.enemySO;
        target = enemyAI.target;

        chaseTime = enemySO.actionTime;
        chaseSpeed = enemySO.chaseSpeed;

        enemyAI.nav.updateRotation = false;
        enemyAI.nav.updateUpAxis = false;
    }

    public override void Initialize()
    {
        SetStateColor();
        
        currentTime = chaseTime;
        enemyAI.isAttaking = false;
        enemyAI.nav.enabled = true;
    }

    public override Status Update()
    {
        OnChase();

        currentTime -= Time.deltaTime;

        Debug.Log(currentTime);

        if (currentTime <= 0.3f)
        {
            enemyAI.isChase = false;
            return Status.BT_Failure;
        }

        //추적중에, 플레이어의 거리와 적의 거리가  사정거리 보다 작다면 BT_Success로 만들어서 어택으로 통과시키자
        if (enemyAI.isAttaking)
            return Status.BT_Success;



        return Status.BT_Running;
    }

    //실제 추적★
    private void OnChase()
    {
        enemyAI.nav.SetDestination(target.transform.position);
        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if(distanceToTarget < enemySO.attackRange)
        {
            enemyAI.isAttaking = true;
            enemyAI.nav.enabled = false;
        }

        //스프라이트 조정(anim = 최대 4방향[대각] + 4방향[정방향] 지정 가능)

        if (target.transform.position.x < owner.transform.position.x)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }
    }





    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.red;
    }
}
