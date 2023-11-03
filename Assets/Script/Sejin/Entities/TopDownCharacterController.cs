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
    public event Action OnRollEvent;
    public event Action OnReloadEvent;


    public PlayerStatHandler playerStatHandler;
    public TopDownMovement topDownMovement;

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallLookEvent(Vector2 direction)
    {
        OnLookEvent?.Invoke(direction);
    }

    public void CallAttackEvent()
    {
        if(!topDownMovement.isRoll && playerStatHandler.CurAmmo > 0)
        {
            OnAttackEvent?.Invoke();
            playerStatHandler.CurAmmo--;
        }
        else
        {

            Debug.Log(playerStatHandler.CurAmmo);
            Debug.Log("���� �� �� �����ϴ�");
        }
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
        }
        else
        {
            Debug.Log("������ ��Ÿ�� �Դϴ�");
        }
    }

    public void CallReloadEvent()
    {
        OnReloadEvent?.Invoke();
    }
}
