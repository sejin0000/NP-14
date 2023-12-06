using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_Phase_1_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private EnemySO bossSO;

    private float percentHP;
    public BossAI_Turtle_Phase_1_Condition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override Status Update()
    {
        percentHP = (bossAI_Turtle.currentHP / bossSO.hp * 100);


        if (percentHP <= 30)//(���� ü���� 30% ����) => ���� �������
        {
            return Status.BT_Failure;
        }
        else
        {
            Debug.Log("1������ ���� �Ϸ�");
            return Status.BT_Success;
        }
    }
}
