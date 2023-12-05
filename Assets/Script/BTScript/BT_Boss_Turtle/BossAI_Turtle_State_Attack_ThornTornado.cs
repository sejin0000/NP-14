using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_State_Attack_ThornTornado : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private Boss_Turtle_SO bossSO;

    public BossAI_Turtle_State_Attack_ThornTornado(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
        bossAI_Turtle.isEndThornTornado = false;
    }

    public override Status Update()
    {
        if (bossAI_Turtle.thornTornadoCoolTime > 0)
        {
            Debug.Log("���� �߻� ��Ÿ�� ��");
            return Status.BT_Success;
        }            




        if(!bossAI_Turtle.isEndThornTornado)
        {
            bossAI_Turtle.ThornTornado1();
            bossAI_Turtle.thornTornadoCoolTime = bossSO.thornTornadoCoolTime;
            Debug.Log($"���� �߻� ���� ���� ���� ���� : ���� {bossAI_Turtle.isEndThornTornado} ");
            return Status.BT_Success;
        }


        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //�� ���� ���� ���� �� �� �ϰ������ ���� �ϼ�
    }
}
