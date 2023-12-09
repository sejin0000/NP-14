using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_ChaseAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // 시간 계산용   
    public BossAI_State_ChaseAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {        
        currentTime = bossSO.atkDelay;
        bossAI_Dragon.isTrackingFurthestTarget = true;
    }

    public override Status Update()
    {
        //딜레이 만큼 대기 -> 공격 -> 성공반환(다시 페이즈/스페셜 어택 체크해야함)
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {

            FurthestTarget();

            bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 4);
            return Status.BT_Success;
        }

        //공격하다가 약점같은 부분 공격 시 fail스테이트 반환하면 패턴 정지 가능




        return Status.BT_Running;
    }

    private void FurthestTarget()
    {
        // 가장 먼 타겟 서치
        float maxDistance = float.MinValue;

        for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
        {
            if (bossAI_Dragon.PlayersTransform[i] == null)
                continue;

            float distanceToAllTarget = Vector2.Distance(owner.transform.position, bossAI_Dragon.PlayersTransform[i].transform.position);

            if (distanceToAllTarget > maxDistance)
            {
                maxDistance = distanceToAllTarget;
                bossAI_Dragon.currentTarget = bossAI_Dragon.PlayersTransform[i];
            }
        }
    }

    public override void Terminate()
    {
    }
}
