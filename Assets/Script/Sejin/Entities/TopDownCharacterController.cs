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
    public event Action OnSkillEvent;
    public event Action OnEndSkillEvent;
    public event Action OnRollEvent;
    public event Action OnEndRollEvent;
    public event Action OnReloadEvent;


    public PlayerStatHandler playerStatHandler;
    public TopDownMovement topDownMovement;

    private bool AtkKeyhold = false;


    private void Update()
    {
        if (AtkKeyhold)
        {
            if (!topDownMovement.isRoll && playerStatHandler.CurAmmo > 0 && playerStatHandler.CanFire)
            {
                OnAttackEvent?.Invoke();
                playerStatHandler.CurAmmo--;
            }
            else
            {
                //Debug.Log(playerStatHandler.CurAmmo);
                //Debug.Log("공격 할 수 없습니다");
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

    public void CallSkillEvent()
    {
        OnSkillEvent?.Invoke();
    }

    public void CallRollEvent()
    {
        if (playerStatHandler.CanRoll)
        {
            OnRollEvent?.Invoke();
            Invoke("EndRollEvent", 0.6f);

        }
        else
        {
            Debug.Log("구르기 쿨타임 입니다");
        }
    }
    public void EndRollEvent()
    {
        OnEndRollEvent?.Invoke();
    }

    public void CallReloadEvent()
    {
        OnReloadEvent?.Invoke();
    }
}
