using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//특수 공격 노드 : 브레스 / 비명 / 깨물기
public class BossAI_State_SpecialAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;
    private Transform target;

    private float currentTime;         // 시간 계산용   
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
        //헤드 피벗 위치를 넘어서 존재한다면? -> 랜덤 패턴값 비명으로 고정

        for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
        {
            float distanceToTargets = Vector2.Distance(bossAI_Dragon.PlayersTransform[i].position, bossAI_Dragon.bossHead.position);

            if (distanceToTargets > 0.3f)
                return Status.BT_Failure;
        }

        SetAim(); //특수패턴 시작 시 보스 머리 방향 => 항상 플레이어 쪽으로

        //bossAI_Dragon.bossHead.transform.LookAt(target.position, Vector3.forward);

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // 공격 주기에 도달하면 랜덤 공격 실행
            int randomPattern = Random.Range(0, 2);


            //난수에 따른 패턴 RPC 여기에 입력
            switch(randomPattern)
            {
                case 0:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 1:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 2:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
            }            
            currentTime = bossSO.atkDelay; //시간 초기화
        }



        //float distanceToTarget = Vector2.Distance(owner.transform.position, target.transform.position);

        return Status.BT_Running;
    }
    public void SetAim() // 피해량, 플레이어 위치 받아옴
    {
        //플레이어를 바라보도록 설정

        //anim.SetTrigger("Attack"); // 공격 애니메이션

        //공격() 각도 바꿔준 후 -> 생성
        Vector3 direction = (target.transform.position - bossAI_Dragon.bossHeadPivot.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        bossAI_Dragon.bossHead.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    public override void Terminate()
    {
    }

    private void SetStateColor()
    {
        bossAI_Dragon.spriteRenderer.color = Color.black;
    }
}
