using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//�÷��̾� Ž���� ���� => ����� ��尡 Ž���� [���� / ����]�� ��ȯ��
//�ڵ��� ���� ����� ���Ǹ� ��ӹ���


//���� 1�� ���-����� ���
public class EnemyState_Chase_ChaseCondition: BTCondition
{
    private GameObject owner;
    private EnemyAI enemyAI;

    public EnemyState_Chase_ChaseCondition(GameObject _owner)
    {
        owner = _owner;
        enemyAI = owner.GetComponent<EnemyAI>();
    }

    //������ ���¸� �޾ƿ���, �׿����� Status ������Ʈ�� ����
    public override Status Update()
    {
        if(enemyAI.isChase || enemyAI.target != null)
            return Status.BT_Success;
        else
            return Status.BT_Failure;
    }
}
