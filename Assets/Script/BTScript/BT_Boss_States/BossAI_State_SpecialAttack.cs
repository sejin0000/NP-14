using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_SpecialAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;
    private Transform target;

    private float currentTime;         // 시간 계산용
    int randomPattern = Random.Range(0, 2);
    public BossAI_State_SpecialAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.atkDelay;
        SetStateColor();
        target = bossAI_Dragon.currentTarget; //루트에서 받은 임의의 타겟 지정
    }

    public override Status Update()
    {
        //맨처음에 할거 -> 모든 플레이어의 위치를 받아서, 내가 지정한 피벗과 가까운지 확인
        //가깝다면? 특수 패턴 실행 ->멀다면? 실패 반환[노말 패턴으로 바로 넘어감]
       

        for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
        {
            float bossHeadToPlayers = Vector2.Distance(bossAI_Dragon.PlayersTransform[i].position, bossAI_Dragon.bossHead.position);

            if (bossHeadToPlayers > 0.3f)
                return Status.BT_Failure;
        }

        SetAim();

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // 공격 주기에 도달하면 공격 실행
            bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
            currentTime = bossSO.atkDelay;
        }



        float distanceToTarget = Vector2.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > bossSO.attackRange)
        {
            bossAI_Dragon.isAttaking = false;
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
        Vector3 direction = (target.transform.position - bossAI_Dragon.enemyAim.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        bossAI_Dragon.enemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    public override void Terminate()
    {
    }

    private void SetStateColor()
    {
        bossAI_Dragon.spriteRenderer.color = Color.black;
    }
}
