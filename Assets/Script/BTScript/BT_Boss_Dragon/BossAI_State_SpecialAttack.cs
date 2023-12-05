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

    private float currentTime;         // 시간 계산용   
    public BossAI_State_SpecialAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.SpecialAttackDelay;
    }

    public override Status Update()
    {
        //맨처음에 할거 -> 모든 플레이어의 위치를 받아서, 내가 지정한 피벗과 가까운지 확인
        //가깝다면? 특수 패턴 실행 ->멀다면? 실패 반환[노말 패턴으로 바로 넘어감]
        //헤드 피벗 위치를 넘어서 존재한다면? -> 랜덤 패턴값 비명으로 고정


        //조건 부분 스페셜 어택 컨디션으로 이관할것 Failure 까지
        float distanceToTarget = Vector2.Distance(owner.transform.position, bossAI_Dragon.currentTarget.transform.position);

        if(distanceToTarget > 7f)
        {
            return Status.BT_Failure;
        }


        Debug.Log("특수 공격 조건 충족 중");

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            
            //특수패턴 1 : 플레이어가 머리 뒤쪽으로 넘어간 경우 => 전체 공격
            for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
            {
                if (bossAI_Dragon.PlayersTransform[i].position.y > 0f)
                {
                    Debug.Log("플레이어에 대한 공격 영역 활성화: " + i);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    return Status.BT_Failure; //이거 잘 생각하셈 (실패 -> 특수 패턴 실행 후 바로 노말 / 성공 -> 다시 처음부터 시작)
                }
            }


            //특수패턴 2 : 보류



            //특수 패턴 3 : 플레이어가 특수 패턴 범위 안에 있는 경우 [통상] => 브레스
            bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);



            /*
            // 공격 주기에 도달하면 랜덤 특수 공격 실행
            int randomPattern = Random.Range(0, 4);


            //난수에 따른 패턴 RPC 여기에 입력 if else로 한번 더 분기(특수 패턴은 확정 패턴과 랜덤 패턴이 필요함)

            switch (randomPattern)
            {
                case 0:
                    //양 팔 공격
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);                   
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);                  
                    break;
                case 1:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;
                case 2:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;  
                case 3:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;
            }            
            */
            currentTime = bossSO.atkDelay; //시간 초기화
        }


        

        return Status.BT_Running;
    }

    public override void Terminate()
    {
    }
}
