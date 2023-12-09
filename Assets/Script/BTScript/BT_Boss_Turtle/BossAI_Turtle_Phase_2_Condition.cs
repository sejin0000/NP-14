using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_Phase_2_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private EnemySO bossSO;


    private float percentHP;
    public BossAI_Turtle_Phase_2_Condition(GameObject _owner)
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
        percentHP = (bossAI_Turtle.currentHP / bossSO.hp * 100);

        if (percentHP > 30)//(���� ü���� 30% �ʰ�) => 1������� (Ȥ�� �ε�ȣ�� ��ġ �������� 3������ ��������)
            return Status.BT_Failure;




        return Status.BT_Success;
    }
}
