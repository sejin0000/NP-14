using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_Choice_3_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;

    public BossAI_State_Choice_3_Condition(GameObject _owner)
    {
        owner = _owner;

        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
    }

    public override void Initialize()
    {
    }

    public override Status Update()
    {



        //�븻 ���� 1
        if (bossAI_Dragon.currentNomalAttackSquence == 2)
        {
            return Status.BT_Success;
        }
        else
            return Status.BT_Failure;
    }
    public override void Terminate()
    {
    }
}