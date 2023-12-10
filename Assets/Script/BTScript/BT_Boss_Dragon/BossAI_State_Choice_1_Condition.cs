using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//스페셜 어택 -> 노말 어택 순서로 체크
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
        Debug.Log($"현재 랜덤 난수는 {bossAI_Dragon.currentNomalAttackSquence} 입니다.");

        //노말 패턴 1
        if (bossAI_Dragon.currentNomalAttackSquence == 0)
        {
            Debug.Log($"노말 시퀀스 패턴 1을 실행합니다.");
            return Status.BT_Success;
        }
        else
        {
            Debug.Log($"실패가 실행되었습니다.");
            return Status.BT_Failure;
        }            
    }
    public override void Terminate()
    {
    }
}
