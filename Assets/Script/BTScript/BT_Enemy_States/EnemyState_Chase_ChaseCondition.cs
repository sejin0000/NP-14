using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//�÷��̾� Ž���� ���� => ����� ��尡 Ž���� [���� / ����]�� ��ȯ��
//�ڵ��� ���� ����� ���Ǹ� ��ӹ���
public class EnemyState_Chase_ChaseCondition: BTCondition
{
    private GameObject owner;

    public EnemyState_Chase_ChaseCondition(GameObject _owner)
    {
        owner = _owner;
    }

    //������ ���¸� �޾ƿ���, �׿����� Status ������Ʈ�� ����
    public override Status Update()
    {
        //�÷��̾� Ž�� �κ�[TODO : nav�� ó������]
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player) //������Ʈ[�÷��̾�]�� ������
        {
            float distance = Vector3.Distance(player.transform.position, owner.transform.position);
            if(distance < 10.0f)
            {
                return Status.BT_Success;
            }
        }
        return Status.BT_Failure;
    }
}
