using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2201 : MonoBehaviour
{
    //기본공격시 구르기 쿨감
    private TopDownCharacterController controller;
    private CoolTimeController playerCool;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerCool = GetComponent<GameObject>().GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnAttackEvent += RollingCoolTime;
    }

    void RollingCoolTime()
    {
        //playerCool.cooltime -= 0.1f; //쿨타임플로트값이 아직 캐릭에 없음
    }
}
