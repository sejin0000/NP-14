using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_RightAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // 시간 계산용   
    public BossAI_State_RightAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.atkDelay;
    }

    public override Status Update()
    {
        //딜레이 만큼 대기 -> 공격 -> 성공반환(다시 페이즈/스페셜 어택 체크해야함)
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 0);
            Debug.Log("우측 공격 액션에 성공함");
            return Status.BT_Success;
        }

        //공격하다가 약점같은 부분 공격 시 fail스테이트 반환하면 패턴 정지 가능




        return Status.BT_Running;
    }
    public override void Terminate()
    {
        Debug.Log($"좌측공격 액션 노드가 성공적으로 종료되었습니다.");
        //각 공격 패턴 끝날 때 뭐 하고싶으면 여기 하셈
    }
}
