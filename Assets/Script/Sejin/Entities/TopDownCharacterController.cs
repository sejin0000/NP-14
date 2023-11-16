using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCharacterController : MonoBehaviour
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
                && (playerStatHandler.CanReload  // 일반공격 조건부
                    || (!playerStatHandler.CanReload && GetComponent<CoolTimeController>().isKeepCount)) // 차지샷 조건부
                )
            {
                OnAttackEvent?.Invoke();
                
            }
            else
            {
                //Debug.Log("공격 할 수 없습니다");
            }
        }
        else
        {
            if (
                !topDownMovement.isRoll 
                && playerStatHandler.CurAmmo >=0 
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
        if(playerStatHandler.CanSkill)
        {
            Debug.Log("callSkillEvent 실행중");
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
        if (playerStatHandler.CanRoll)
        {
            OnRollEvent?.Invoke();
            playerStatHandler.CanRoll = false;
            playerStatHandler.Invincibility = true;
            Invoke("CallEndRollEvent", 0.6f);
        }
        else
        {
            Debug.Log("구르기 쿨타임 입니다");
        }
    }
    public void CallEndRollEvent()
    {
        Debug.Log("구르기 끝 이벤트");

        OnEndRollEvent?.Invoke();
    }

    public void CallReloadEvent()
    {
        if (playerStatHandler.CanReload&& playerStatHandler.CurAmmo != playerStatHandler.AmmoMax.total)
        {
            Debug.Log("재장전");
            OnReloadEvent?.Invoke();
        }
    }

    public void CallEndReloadEvent()
    {
        OnEndReloadEvent?.Invoke();
    }
}
