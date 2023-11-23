using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviourPun
{
    public event Action<Vector2> OnMoveEvent;
    public event Action<Vector2> OnLookEvent;
    public event Action OnAttackEvent;
    public event Action OnEndAttackEvent;
    public event Action OnSkillEvent;
    public event Action OnEndSkillEvent;
    public event Action OnRollEvent;
    public event Action OnEndRollEvent;
    public event Action OnReloadEvent;
    public event Action OnEndReloadEvent;
    public event Action OnStartSkillEvent;
    public event Action OnSiegeModeEvent;
    public event Action OnFlashEvent;


    public event Action SkillMinusEvent;
    public event Action<bool> OnAttackKeepEvent;
    public event Action OnChargeAttackEvent;


    public PlayerStatHandler playerStatHandler;
    public TopDownMovement topDownMovement;
    public CoolTimeController coolTimeController;

    private bool AtkKeyhold = false;

    private void Awake()
    {
        coolTimeController = GetComponent<CoolTimeController>();
    }
    private void Update()
    {
        if (AtkKeyhold)
        {
            if
                (
                !topDownMovement.isRoll
                && playerStatHandler.CurAmmo > 0
                && playerStatHandler.CanFire
                && (playerStatHandler.CanReload  // �Ϲݰ��� ���Ǻ�
                    || (!playerStatHandler.CanReload && GetComponent<CoolTimeController>().isKeepCount)) // ������ ���Ǻ�
                )
            {
                OnAttackEvent?.Invoke();

            }
            else
            {
                //Debug.Log("���� �� �� �����ϴ�");
            }
        }
        else
        {
            if (
                !topDownMovement.isRoll
                && playerStatHandler.CurAmmo >= 0
                && playerStatHandler.CanFire
                && playerStatHandler.CanReload
                && coolTimeController.bulletNum > 0
                )
            {
                OnChargeAttackEvent?.Invoke();
            }
        }
    }

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent(bool hold)
    {
        AtkKeyhold = hold;
    }

    public void CallAttackKeepEvent(bool hold)
    {
        OnAttackKeepEvent?.Invoke(hold);
    }

    public void CallAttackEndEvent()
    {
        OnEndAttackEvent?.Invoke();
    }

    public void CallSkillEvent()
    {
        if (playerStatHandler.CanSkill)
        {
            Debug.Log("callSkillEvent ������");
            OnSkillEvent?.Invoke();
        }
    }
    public void SkillReset()
    {
        SkillMinusEvent?.Invoke();
    }
    public void CallEndSkillEvent()
    {
        OnEndSkillEvent?.Invoke();
    }

    public void CallRollEvent()
    {
        if (gameObject.GetComponent<A1207>()) 
        {
            A1207 a1207 = gameObject.GetComponent<A1207>();
            a1207.Change();
        }
        else if (playerStatHandler.CanRoll)
        {
            OnRollEvent?.Invoke();
            playerStatHandler.CurRollStack -= 1;
            Debug.Log($"������ ���� ���� : {playerStatHandler.CurRollStack} ����");
            playerStatHandler.CanRoll = false;
            playerStatHandler.Invincibility = true;
            Invoke("CallEndRollEvent", 0.6f);
        }
        else
        {
            Debug.Log("������ ��Ÿ�� �Դϴ�");
        }
    }
    public void CallEndRollEvent()
    {
        Debug.Log("������ �� �̺�Ʈ");
        playerStatHandler.CanRoll = true;
        playerStatHandler.Invincibility = false;
        OnEndRollEvent?.Invoke();
    }
    public void CallSiegeModeEvent()
    {
        OnSiegeModeEvent?.Invoke();
    }
    public void CallFlashEvent() 
    {

        if (playerStatHandler.CanRoll)
        {
            OnFlashEvent?.Invoke();
            playerStatHandler.CurRollStack -= 1;
            playerStatHandler.CanRoll = false;
            playerStatHandler.Invincibility = true;
            CallEndRollEvent();
        }
        else
        {
            Debug.Log("������ ��Ÿ�� �Դϴ�");
        }
    }

    public void CallReloadEvent()
    {
        if (playerStatHandler.CanReload&& playerStatHandler.CurAmmo != playerStatHandler.AmmoMax.total)
        {
            Debug.Log("������");
            OnReloadEvent?.Invoke();
        }
    }

    public void CallEndReloadEvent()
    {
        OnEndReloadEvent?.Invoke();
    }
}
