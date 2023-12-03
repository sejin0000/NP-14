using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_ReturnRoot : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // 시간 계산용   
    public BossAI_State_ReturnRoot(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
    }

    public override void Initialize()
    {
        //리프 노드에서 실패를 반환할 수 없고
        //리프 노드에서 트리 재실행이 필요한 경우 쓰셈
    }

    public override Status Update()
    {
        return Status.BT_Success;
    }
    public override void Terminate()
    {
        Debug.Log("리셋 성공");
        //루트로 올라가기전에 하고싶은 일 있으면 쓰셈
    }
}
