using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using Photon.Pun;
using static UnityEngine.RuleTile.TilingRuleOutput;

//추적
public class EnemyState_Chase : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    public NavMeshAgent nav;

    private float chaseTime;           // 걷기 시간
    private float currentTime;         // 시간 계산용


    public EnemyState_Chase(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        nav = owner.GetComponent<NavMeshAgent>();
        enemySO = enemyAI.enemySO;

        chaseTime = enemySO.chaseTime;

        //enemyAI.nav.updateRotation = false;
        //enemyAI.nav.updateUpAxis = false;
    }

    public override void Initialize()
    {
        SetStateColor();
        enemyAI.ChangeSpeed(enemySO.enemyChaseSpeed);
        currentTime = chaseTime;
        //수정됨
        //enemyAI.nav.enabled = true;
    }

    public override void Terminate()
    {
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.3f)
        {
            enemyAI.isChase = false;
            enemyAI.Target = null;

            return Status.BT_Failure;
        }

        //추적중에, 플레이어의 거리와 적의 거리가  사정거리 보다 작다면 BT_Success로 만들어서 어택으로 통과시키자
        if (enemyAI.isAttaking)
            return Status.BT_Success;


        OnChase();

        return Status.BT_Running;
    }

    //실제 추적★
    //네브(노드에서 >>>>>>>>> 기능 두번이상 = 메서드 == 루트에서 두번이상 사용하는 기능들을 메서드 화 -> 요청만 하고, 메서드 호출)
    //현재 목표점 수정, 플립, ....
    private void OnChase()
    {
        if (enemyAI.photonView.AmOwner)
            enemyAI.navTargetPoint = enemyAI.Target.transform.position;

        enemyAI.DestinationSet();


        float distanceToTarget = Vector3.Distance(owner.transform.position, enemyAI.Target.transform.position);

        if (distanceToTarget < enemySO.attackRange)
        {
            enemyAI.isAttaking = true;
            //수정됨
            //enemyAI.nav.enabled = false;
        }

        //스프라이트 조정(anim = 최대 4방향[대각] + 4방향[정방향] 지정 가능)

        //목적지 지정 & 내 위치~타겟위치 & 

        //isflip(값만 받기 타겟 트랜스폼 x, 내 트랜스폼 x)


        //★★★수정함
        //enemyAI.PV.RPC("Filp", RpcTarget.All);
        //enemyAI.Filp(owner.transform.position.x, enemyAI.Target.transform.position.x);

        /*
        if (target.transform.position.x < owner.transform.position.x)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }
        */
    }





    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.gray;
    }
}
