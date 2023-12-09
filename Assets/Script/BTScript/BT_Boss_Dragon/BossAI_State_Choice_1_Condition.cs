using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����� ���� -> �븻 ���� ������ üũ
public class BossAI_State_Choice_1_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;

    public BossAI_State_Choice_1_Condition(GameObject _owner)
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
        if (bossAI_Dragon.currentNomalAttackSquence == 0)
        {
            return Status.BT_Success;
        }
        else
        {
            return Status.BT_Failure;
        }            
    }
    public override void Terminate()
    {
    }
}
