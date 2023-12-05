using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_Phase_2_SpecialAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // 시간 계산용   
    public BossAI_State_Phase_2_SpecialAttack(GameObject _owner)
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

            currentTime = bossSO.atkDelay; //시간 초기화
        }




        return Status.BT_Running;
    }

    public override void Terminate()
    {
    }
}
