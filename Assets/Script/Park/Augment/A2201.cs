using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2201 : MonoBehaviour
{
    //�⺻���ݽ� ������ ��
    private TopDownCharacterController controller;
    private CoolTimeController playerCool;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerCool = GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnAttackEvent += RollingCoolTime;
    }

    void RollingCoolTime()
    {
        playerCool.curRollCool -= 2f;
        //Debug.Log($"{playerCool.curRollCool}");
    }
}
