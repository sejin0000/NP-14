using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CoolTimeController : MonoBehaviour
{
    private TopDownCharacterController controller;


    public float curRollCool = 0;

    public float curReloadCool = 0;

    public float curAttackCool = 0;

    public float curSkillCool = 0;

    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        controller.OnEndRollEvent += RollCoolTime;
        controller.OnReloadEvent += ReloadCoolTime;
        controller.OnAttackEvent += AttackCoolTime;
        controller.OnEndSkillEvent += SkillCoolTime;
    }


    private void Update()
    {
        if(curRollCool > 0)
        {
            curRollCool -= Time.deltaTime;
            if (controller.playerStatHandler.CanRoll == false && curRollCool <= 0)
            {
                EndRollCoolTime();
            }
        }

        if (curReloadCool > 0)
        {
            curReloadCool -= Time.deltaTime;
            if (controller.playerStatHandler.CanReload == false && curReloadCool <= 0)
            {
                EndReloadCoolTime();
            }
        }

        if (curAttackCool > 0)
        {
            curAttackCool -= Time.deltaTime;
            if (controller.playerStatHandler.CanFire == false && curAttackCool <= 0)
            {
                EndAttackCoolTime();
            }
        }

        if (curSkillCool > 0)
        {
            curSkillCool -= Time.deltaTime;
            if (controller.playerStatHandler.CanSkill == false && curSkillCool <= 0)
            {
                EndSkillCoolTime();
            }
        }
    }

    private void RollCoolTime()
    {
        float coolTime = controller.playerStatHandler.RollCoolTime.total;
        controller.playerStatHandler.CanRoll = false;
        controller.playerStatHandler.Invincibility = true;
        curRollCool = coolTime;
    }
    private void EndRollCoolTime()
    {
        Debug.Log("구르기 쿨타임 종료 이벤트");
        controller.playerStatHandler.CanRoll = true;
        controller.playerStatHandler.Invincibility = false;
    }


    private void ReloadCoolTime()
    {
        float coolTime = controller.playerStatHandler.ReloadCoolTime.total;
        controller.playerStatHandler.CanReload = false;
        curReloadCool = coolTime;
    }
    private void EndReloadCoolTime()
    {
        controller.playerStatHandler.CanReload = true;
        controller.playerStatHandler.CurAmmo = controller.playerStatHandler.AmmoMax.total;
        controller.CallEndReloadEvent();
    }

    private void AttackCoolTime()
    {
        float coolTime = 1 / controller.playerStatHandler.AtkSpeed.total;
        controller.playerStatHandler.CanFire = false;
        curAttackCool = coolTime;
    }
    private void EndAttackCoolTime()
    {
        controller.playerStatHandler.CanFire = true;
        controller.CallAttackEndEvent();
    }

    private void SkillCoolTime()
    {
        float coolTime = controller.playerStatHandler.SkillCoolTime.total;
        controller.playerStatHandler.CanSkill = false;
        curSkillCool = coolTime;
        Debug.Log("스킬 쿨 타임 시작");

    }
    
    private void EndSkillCoolTime()
    {
        controller.playerStatHandler.CanSkill = true;
        Debug.Log("스킬 쿨 타임 종료");
    }
}
