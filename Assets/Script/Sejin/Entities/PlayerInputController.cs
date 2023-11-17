using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public class PlayerInputController : TopDownCharacterController
{
    private bool IsAtking = false;

    private Camera _camera;
    public PlayerInput playerInput;
    public int atkPercent;
    public bool IsMove = false;
    PlayerStatHandler playerstatHnadler;

    private void Awake()
    {
        // 추가함
        coolTimeController = GetComponent<CoolTimeController>();

        playerstatHnadler = GetComponent<PlayerStatHandler>();
        playerstatHnadler.OnDieEvent += InputOff;
        playerstatHnadler.OnRegenEvent += InputOn;
        atkPercent = 100;


        playerInput = GetComponent<PlayerInput>();
        playerInput.actions.FindAction("Move2").Disable();
        _camera = Camera.main;


        if(!GetComponent<PhotonView>().IsMine)
        {
            Destroy(GetComponent<PlayerInputController>());
        }
    }
    private void OnEnable()
    {
        if (playerstatHnadler.isNoramlMove)
        {
            playerInput.actions.FindAction("Move2").Disable();
            playerInput.actions.FindAction("Move").Enable();
        }
        else
        {
            playerInput.actions.FindAction("Move2").Enable();
            playerInput.actions.FindAction("Move").Disable();
        }
        if (playerstatHnadler.isCanSkill)
        {
            playerInput.actions.FindAction("Skill").Enable();
        }
        else 
        {
            playerInput.actions.FindAction("Skill").Disable();
        }
        if (playerstatHnadler.isCanAtk)
        {
            playerInput.actions.FindAction("Attack").Enable();
        }
        else
        {
            playerInput.actions.FindAction("Attack").Disable();
        }
    }
    public void OnMove(InputValue value)
    {
        // Debug.Log("OnMove" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }
    public void OnIsMove()
    {
        if (playerInput.actions["IsMove"].ReadValue<float>() == 1)
        {
            playerStatHandler.MoveStartCall();
        }
        else
        {
            playerStatHandler.MoveEndCall();
        }
    }
    public void OnMove2(InputValue value)
    {
        Debug.Log("무브2작동중" + value.ToString());
        Vector2 moveInput = value.Get<Vector2>().normalized;
        CallMoveEvent(moveInput);
    }

    public void OnLook(InputValue value)
    {
        // Debug.Log("OnLook" + value.ToString());
        Vector2 newAim = value.Get<Vector2>();
        Vector2 worldPos = _camera.ScreenToWorldPoint(newAim);
        newAim = (worldPos - (Vector2)transform.position).normalized;

        CallLookEvent(newAim);
    }

    public void OnAttack(InputValue value)
    {
        int random = Random.Range(0, 100);
        if (atkPercent >= random) 
        {
            Debug.Log("OnAttack" + value.ToString());
            if (EventSystem.current != null)
            {
                if (!IsAtking && !EventSystem.current.IsPointerOverGameObject() && playerInput.actions["Attack"].ReadValue<float>() == 1)//playerInput.actions["Attack"].ReadValue<float>()마우스 눌리는거 확인하는 변수
                {
                    CallAttackEvent(true);
                    //추가함
                    CallAttackKeepEvent(true);
                }
                else
                {
                    CallAttackEvent(false);
                    //추가함
                    CallAttackKeepEvent(false);
                }
            }
        }

    }

    public void OnSkill(InputValue value)
    {
        Debug.Log("OnSkill" + value.ToString());
        CallSkillEvent();
    }

    public void OnRoll(InputValue value)
    {
        Debug.Log("OnRoll" + value.ToString());
        CallRollEvent();
    }

    public void OnReload(InputValue value)
    {
        Debug.Log("OnReload" + value.ToString());
        CallReloadEvent();
    }

    public void InputOff() 
    {
        playerInput.DeactivateInput();
    }

    public void InputOn()
    {
        playerInput.ActivateInput();
    }

}
