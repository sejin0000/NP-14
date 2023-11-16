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

    // �߰�
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
        // ���ݸ� �߰� (���� �߰��� ���� ���)
        //controller.OnAttackKeepEvent += TimeCount;
    }


    private void Update()
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

        if (curReloadCool > 0)
        {
            curReloadCool -= Time.deltaTime;
        }
        if (controller.playerStatHandler.CanReload == false && curReloadCool <= 0 && !isKeepCount)
        {
            EndReloadCoolTime();
        }

        if (curAttackCool > 0)
        {
            curAttackCool -= Time.deltaTime;
        }
        if (controller.playerStatHandler.CanFire == false && curAttackCool <= 0)
        {
            EndAttackCoolTime();
        }

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

        if (isKeepCount)
        {
            stackedTime += Time.deltaTime;
            if (!isCharging)
            {
                StartCoroutine(CountBullets());            
            }    
        }
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
        controller.playerStatHandler.Invincibility = true;
        curRollCool = coolTime;
        controller.playerStatHandler.UseRoll = false;
    }
    private void EndRollCoolTime()
    {
        Debug.Log("������ ��Ÿ�� ���� �̺�Ʈ");
        controller.playerStatHandler.CurRollStack += 1;
        controller.playerStatHandler.CanRoll = true;
        controller.playerStatHandler.UseRoll = true;
        controller.playerStatHandler.Invincibility = false;
        if (controller.playerStatHandler.CurRollStack < controller.playerStatHandler.MaxRollStack)
        {
            SkillCoolTime();
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

    public void AttackCoolTime()
    {
        float coolTime = 1 / controller.playerStatHandler.AtkSpeed.total;
        controller.playerStatHandler.CanFire = false;
        curAttackCool = coolTime;
    }

    //public void ChargeAttackCoolTime(bool isCharged)
    //{
    //    if (isCharged)
    //    {
    //        Debug.Log("Returned");
    //        return;
    //    }
    //    float coolTime = 0.1f;
    //    controller.playerStatHandler.CanFire = false;
    //    Debug.Log("������ ������");
    //    curAttackCool = coolTime;
    //}

    private void EndAttackCoolTime()
    {
        controller.playerStatHandler.CanFire = true;
        controller.CallAttackEndEvent();
    }

    private void SkillCoolTime()
    {
        float coolTime = controller.playerStatHandler.SkillCoolTime.total;
        Debug.Log($"��ü ��Ÿ�� : {coolTime}");
        if (controller.playerStatHandler.CurSkillStack > 0)
        {
            controller.playerStatHandler.CanSkill = true;
        }
        else
        {
            controller.playerStatHandler.CanSkill = false;
        }
        curSkillCool = coolTime;
       
        Debug.Log("��ų �� Ÿ�� ����");
    }
    
    private void EndSkillCoolTime()
    {
        controller.playerStatHandler.CurSkillStack += 1;
        controller.playerStatHandler.CanSkill = true;
        Debug.Log($"��ų ��Ÿ�� ��� ��, ���� ��ų ���� �� : {controller.playerStatHandler.CurSkillStack}");
        if (controller.playerStatHandler.CurSkillStack < controller.playerStatHandler.MaxSkillStack)
        {
            SkillCoolTime();
        }
        Debug.Log("��ų �� Ÿ�� ����");
    }

    public void TimeCount(bool isCount)
    {
        if (isCount)
        {
            isKeepCount = true;
            stackedTime = 0;
            bulletNum = 0;
            controller.playerStatHandler.CanReload = false;
            Debug.Log("�ð� ���� ����");
        }
        else
        {
            isKeepCount = false;
            controller.playerStatHandler.CanReload = true;
            Debug.Log($"���� ������ �ð� : {stackedTime}");
            Debug.Log($"���� �Ҹ� �� : {bulletNum}");
            Debug.Log($"���� �Ѿ� �� : {controller.playerStatHandler.CurAmmo}");            
            //GetComponent<WeaponSystem>().ChargeCalculate(stackedTime);
            // ���⼭ ���� �̺�Ʈ�� �Ķ���ͷν�? ���� �����ؾ���.
        }
    }

    public IEnumerator CountBullets()
    {
        isCharging = true;
        Debug.Log($"���� ��ź �� : {controller.playerStatHandler.CurAmmo}");
        if (controller.playerStatHandler.CurAmmo >= 1)
        {
            controller.playerStatHandler.CurAmmo--;
            bulletNum++;
            Debug.Log($"�Ҹ� �״� �� {bulletNum}");
        }
        yield return new WaitForSeconds(0.15f); 
        isCharging = false;
    }
}
