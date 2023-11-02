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
        playerCool = GetComponent<GameObject>().GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnAttackEvent += RollingCoolTime;
    }

    void RollingCoolTime()
    {
        //playerCool.cooltime -= 0.1f; //��Ÿ���÷�Ʈ���� ���� ĳ���� ����
    }
}
