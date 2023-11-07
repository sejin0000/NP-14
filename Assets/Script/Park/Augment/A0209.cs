using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0209 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private CoolTimeController coolTime;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        coolTime = GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnRollEvent += Reloading;
    }
    // Update is called once per frame
    void Reloading()
    {
        coolTime.curReloadCool = 0f;
        controller.CallReloadEvent();
    }
}
