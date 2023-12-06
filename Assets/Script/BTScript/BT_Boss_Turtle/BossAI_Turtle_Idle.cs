using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//��Ÿ�� ������(�ƹ��͵� ����)
public class BossAI_Turtle_Idle : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private EnemySO bossSO;

    public BossAI_Turtle_Idle(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
    }

    public override Status Update()
    {
        if (bossAI_Turtle.thornTornadoCoolTime <= 0 || bossAI_Turtle.missileCoolTime <= 0) //(bossAI_Turtle.rollingCooltime <= 0 || bossAI_Turtle.thornTornadoCoolTime <= 0 || bossAI_Turtle.missileCoolTime <= 0)
        {
            Debug.Log("��Ÿ�� üũ�� �Ϸ�Ǿ����ϴ�.");
            return Status.BT_Failure;
        }
        //��Ÿ�� üũ = ��� ��Ÿ�� �� �ϳ��� ��Ÿ���� �� �����ִ� ������ �ִٸ� ���� ��ȯ ��Ű�� ��Ʈ����
        
        //������Ʈ : ��� ��Ÿ�� �ð��� ���� ������Ʈ
        //bossAI_Turtle.rollingCooltime -= Time.deltaTime;
        bossAI_Turtle.thornTornadoCoolTime -= Time.deltaTime;
        bossAI_Turtle.missileCoolTime -= Time.deltaTime;







        //��� ��Ÿ���� 0 �ʰ���� ��� ���� 

        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //�� ���� ���� ���� �� �� �ϰ������ ���� �ϼ�
    }
}
