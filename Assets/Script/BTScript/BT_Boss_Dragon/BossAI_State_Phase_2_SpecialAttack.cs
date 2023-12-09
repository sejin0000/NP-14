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

    private float currentTime;         // �ð� ����   
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

            //Ư������ 1 : �÷��̾ �Ӹ� �������� �Ѿ ��� => ��ü ����
            for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
            {
                if (bossAI_Dragon.PlayersTransform[i].position.y > 0f)
                {
                    Debug.Log("�÷��̾ ���� ���� ���� Ȱ��ȭ: " + i);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    return Status.BT_Failure; //�̰� �� �����ϼ� (���� -> Ư�� ���� ���� �� �ٷ� �븻 / ���� -> �ٽ� ó������ ����)
                }
            }

            currentTime = bossSO.atkDelay; //�ð� �ʱ�ȭ
        }




        return Status.BT_Running;
    }

    public override void Terminate()
    {
    }
}
