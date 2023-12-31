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

    public float stackedTime = 0;
    public bool isKeepCount;
    private bool isCharging;
    public int bulletNum;

    // 추가
    //public event Action CallTimeCountEvent;

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
        // 지금만 추가 (증강 추가시 제거 요망)
        //controller.OnAttackKeepEvent += TimeCount;
    }


    private void Update()
    {
        CountRollCoolTime();
        CountReloadCoolTime();
        CountAttackCoolTime();
        CountSkillCoolTime();
        CountTimeNBullets();
    }

    private void RollCoolTime()
    {
        float coolTime = controller.playerStatHandler.RollCoolTime.total;        
        if (controller.playerStatHandler.CurRollStack > 0)
        {
            controller.playerStatHandler.CanRoll = true;
        }
        else
        {
            controller.playerStatHandler.CanRoll = false;
        }
        //controller.playerStatHandler.Invincibility = true;
        curRollCool = coolTime;
        controller.playerStatHandler.UseRoll = false;
    }
    public void EndRollCoolTime()
    {
        //Debug.Log("구르기 쿨타임 종료 이벤트");
        controller.playerStatHandler.CurRollStack += 1;
        controller.playerStatHandler.CanRoll = true;
        controller.playerStatHandler.UseRoll = true;
        //controller.playerStatHandler.Invincibility = false;
        if (controller.playerStatHandler.CurRollStack < controller.playerStatHandler.MaxRollStack)
        {
            RollCoolTime();
        }
    }

    private void CountRollCoolTime()
    {
        if (curRollCool > 0)
        {
            curRollCool -= Time.deltaTime;
        }
        if (
            curRollCool <= 0
            && controller.playerStatHandler.UseRoll == false
            && (controller.playerStatHandler.CurRollStack < controller.playerStatHandler.MaxRollStack || controller.playerStatHandler.CanRoll == false)
            )
        {
            EndRollCoolTime();
        }
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

    private void CountReloadCoolTime()
    {
        if (curReloadCool > 0)
        {
            curReloadCool -= Time.deltaTime;
        }
        if (controller.playerStatHandler.CanReload == false && curReloadCool <= 0 && !isKeepCount)
        {
            EndReloadCoolTime();
        }
    }
    public void AttackCoolTime()
    {
        float coolTime = 1 / controller.playerStatHandler.AtkSpeed.total;
        controller.playerStatHandler.CanFire = false;
        curAttackCool = coolTime;
    }

    private void CountAttackCoolTime()
    {
        if (curAttackCool > 0)
        {
            curAttackCool -= Time.deltaTime;
        }
        if (controller.playerStatHandler.CanFire == false && curAttackCool <= 0)
        {
            EndAttackCoolTime();
        }
    }

    private void EndAttackCoolTime()
    {
        controller.playerStatHandler.CanFire = true;
        controller.CallAttackEndEvent();
    }

    private void SkillCoolTime()
    {
        float coolTime = controller.playerStatHandler.SkillCoolTime.total;
        if (controller.playerStatHandler.CurSkillStack > 0)
        {
            controller.playerStatHandler.CanSkill = true;
        }
        else
        {
            controller.playerStatHandler.CanSkill = false;
        }
        curSkillCool += coolTime;
    }
    
    private void EndSkillCoolTime()
    {
        controller.playerStatHandler.CurSkillStack += 1;
        controller.playerStatHandler.CanSkill = true;
        if (controller.playerStatHandler.CurSkillStack < controller.playerStatHandler.MaxSkillStack)
        {
            SkillCoolTime();
        }
    }

    private void CountSkillCoolTime()
    {
        if (curSkillCool > 0)
        {
            curSkillCool -= Time.deltaTime;
        }
        if (curSkillCool <= 0
            && controller.playerStatHandler.useSkill == false
            && (controller.playerStatHandler.CurSkillStack < controller.playerStatHandler.MaxSkillStack || controller.playerStatHandler.CanSkill == false))
        {
            EndSkillCoolTime();
        }
    }

    public void TimeCount(bool isCount)
    {
        if (isCount)
        {
            isKeepCount = true;
            stackedTime = 0;
            bulletNum = 0;
            controller.playerStatHandler.CanReload = false;
            Debug.Log("시간 세기 시작");
        }
        else
        {
            isKeepCount = false;
            controller.playerStatHandler.CanReload = true;
            Debug.Log($"공격 유지한 시간 : {stackedTime}");
            Debug.Log($"쌓인 불릿 수 : {bulletNum}");
            Debug.Log($"남은 총알 수 : {controller.playerStatHandler.CurAmmo}");            
            //GetComponent<WeaponSystem>().ChargeCalculate(stackedTime);
            // 여기서 공격 이벤트에 파라미터로써? 숫자 제공해야함.
        }
    }

    public IEnumerator CountBullets()
    {
        isCharging = true;
        Debug.Log($"남은 장탄 수 : {controller.playerStatHandler.CurAmmo}");
        if (controller.playerStatHandler.CurAmmo >= 1)
        {
            controller.playerStatHandler.CurAmmo--;
            bulletNum++;
            Debug.Log($"불릿 쌓는 중 {bulletNum}");
        }
        yield return new WaitForSeconds(0.15f); 
        isCharging = false;
    }

    private void CountTimeNBullets()
    {
        if (isKeepCount)
        {
            stackedTime += Time.deltaTime;
            if (!isCharging)
            {
                StartCoroutine(CountBullets());
            }
        }
    }
}
