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


        Debug.Log($"현재 랜덤 난수는 {bossAI_Dragon.currentNomalAttackSquence} 입니다.");

        //노말 패턴 1
        if (bossAI_Dragon.currentNomalAttackSquence == 2)
        {
            Debug.Log($"노말 시퀀스 패턴 3을 실행합니다.");
            return Status.BT_Success;
        }
        else
            return Status.BT_Failure;
    }
    public override void Terminate()
    {
    }
}