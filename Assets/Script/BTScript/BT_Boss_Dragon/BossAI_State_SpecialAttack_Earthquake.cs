using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_SpecialAttack_Earthquake : BTAction
{
    //�ִϸ��̼� - �� ���� ���ø���

    //���� ���� - �� ��ü�� ���׻����� �׷��ְ�, ������ �������� �Ѵ�(�߽ɿ��� �����ڸ��� ����) - ���ո޼���� ����

    //�ִϸ��̼� - �� ���� �������´�



    //���� ���� : ��� �÷��̾��� ī�޶� ����.(�÷��̾� - ī�޶�)

    //�÷��̾�� ��� ��ġ���� n��ŭ�� Ȯ������ ���� ������ (�÷��̾ �޼��� �߰� 1, 2)
    //[�⺻ �˹�Ÿ� + ���� �Ӹ����� n��ŭ�� �Ÿ�]�� �߰������� �з�����.

    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO; //�Ѿ� ���ݷ� �޾ƾ���
    private Transform target;

    private float currentTime;         // �ð� ����

    public BossAI_State_SpecialAttack_Earthquake(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;
    }

    public override void Initialize()
    {
        currentTime = enemySO.atkDelay;
        enemyAI.PV.RPC("SetStateColor", RpcTarget.All, Color.red);

        target = enemyAI.Target;
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // ���� �ֱ⿡ �����ϸ� ���� ����
            enemyAI.PV.RPC("Fire", RpcTarget.All);
            currentTime = enemySO.atkDelay;
        }



        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > enemySO.attackRange)
        {
            enemyAI.isAttaking = false;
            return Status.BT_Failure; // ��� ����
        }



        //�ڡڡڼ�����
        //enemyAI.PV.RPC("Filp", RpcTarget.All);;
        //enemyAI.Filp(owner.transform.position.x, target.transform.position.x);

        return Status.BT_Running;
    }

    public override void Terminate()
    {

    }
}
