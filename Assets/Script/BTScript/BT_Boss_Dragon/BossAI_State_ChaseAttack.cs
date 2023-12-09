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

    private float currentTime;         // �ð� ����   
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
        //������ ��ŭ ��� -> ���� -> ������ȯ(�ٽ� ������/����� ���� üũ�ؾ���)
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {

            FurthestTarget();

            bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 4);
            return Status.BT_Success;
        }

        //�����ϴٰ� �������� �κ� ���� �� fail������Ʈ ��ȯ�ϸ� ���� ���� ����




        return Status.BT_Running;
    }

    private void FurthestTarget()
    {
        // ���� �� Ÿ�� ��ġ
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
