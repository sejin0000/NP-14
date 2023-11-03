using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2202 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();
        coolTimeController= GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnRollEvent += Cooltime;
    }

    // Update is called once per frame
    void Cooltime()
    {
        //coolTimeController.cur += 0.01f;스킬쿨타임현재x
    }
}
