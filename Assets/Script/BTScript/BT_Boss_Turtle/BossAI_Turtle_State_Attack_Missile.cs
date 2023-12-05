using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Turtle_State_Attack_Missile : BTAction
{
    private GameObject owner;
    private BossAI_Turtle bossAI_Turtle;
    private Boss_Turtle_SO bossSO;
    public BossAI_Turtle_State_Attack_Missile(GameObject _owner)
    {
        owner = _owner;
        bossAI_Turtle = owner.GetComponent<BossAI_Turtle>();
        bossSO = bossAI_Turtle.bossSO;
    }

    public override void Initialize()
    {
        bossAI_Turtle.isEndMissile = false;
    }

    public override Status Update()
    {
        if (bossAI_Turtle.missileCoolTime > 0)
        {
            Debug.Log("�̻��� �߻� ��Ÿ�� ��");
            return Status.BT_Success;
        }




        if (!bossAI_Turtle.isEndMissile)
        {
            bossAI_Turtle.MissileOn();
            Debug.Log("�̻��� �߻� ����");
        }



        if(bossAI_Turtle.isEndMissile)
        {
            Debug.Log("�̻��� �߻� �Ϸ�, ��Ÿ�� ����");
            bossAI_Turtle.missileCoolTime = bossSO.missileCoolTime;
            return Status.BT_Success;
        }

        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //�� ���� ���� ���� �� �� �ϰ������ ���� �ϼ�
    }
}
