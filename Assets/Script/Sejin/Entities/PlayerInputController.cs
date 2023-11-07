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

    private void Awake()
    {
        _camera = Camera.main;


        if(!GetComponent<PhotonView>().IsMine)
        {
            Destroy(GetComponent<PlayerInputController>());
        }
    }

    public void OnMove(InputValue value)
    {
        // Debug.Log("OnMove" + value.ToString());
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
        Debug.Log("OnAttack" + value.ToString());
        if (EventSystem.current != null)
        {

            if (!IsAtking && !EventSystem.current.IsPointerOverGameObject())
            {
                IsAtking = true;
            }
            else
            {
                IsAtking = false;
            }
        }
        else
        {
            if (!IsAtking)
            {
                IsAtking = true;
            }
            else
            {
                IsAtking = false;
            }
        }

        CallAttackEvent(IsAtking);
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

}
