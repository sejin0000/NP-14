using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CoolTimeController : MonoBehaviour
{
    private TopDownCharacterController controller;
    public float curRollCool = 0;
    private float curReloadCool = 0;
    private bool isReloadCool = false;

    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        controller.OnRollEvent += RollCoolTime;
        controller.OnReloadEvent += ReloadCoolTime;
    }


    private void Update()
    {
        if(curRollCool > 0)
        {
            curRollCool -= Time.deltaTime;
        }
        if (controller.playerStatHandler.CanRoll == false && curRollCool < 0)
        {
            EndRollCoolTime();
        }

        if (curReloadCool > 0)
        {
            curReloadCool -= Time.deltaTime;
        }
        if (isReloadCool == false && curReloadCool < 0)
        {
            EndReloadCoolTime();
        }
    }

    private void RollCoolTime()
    {
        float coolTime = controller.playerStatHandler.RollCoolTime.total;
        controller.playerStatHandler.CanRoll = false;
        curRollCool = coolTime;
    }
    private void EndRollCoolTime()
    {
        controller.playerStatHandler.CanRoll = true;
    }
    private void ReloadCoolTime()
    {
        float coolTime = controller.playerStatHandler.ReloadCoolTime.total;
        isReloadCool = false;
        curReloadCool = coolTime;
    }

    private void EndReloadCoolTime()
    {
        isReloadCool = true;
        controller.playerStatHandler.CurAmmo = controller.playerStatHandler.AmmoMax.total;
    }
}
