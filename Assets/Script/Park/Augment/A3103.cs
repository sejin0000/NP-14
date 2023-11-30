using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A3103 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private WeaponSystem weaponSystem;
    private PlayerInput playerInput;
    private bool IsSiegeMode;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();
            weaponSystem= GetComponent<WeaponSystem>();
            playerInput = GetComponent<PlayerInput>();
            IsSiegeMode = false;
            controller.OnSiegeModeEvent += ChangeMode; // 중요한부분
        }
    }
    // Update is called once per frame
    void ChangeMode()
    {
        if (!IsSiegeMode) 
        {
            IsSiegeMode = !IsSiegeMode;

            playerInput.actions.FindAction("Move2").Disable();
            playerInput.actions.FindAction("Move").Disable();

            playerStat.BulletLifeTime.coefficient *= 3f;
            playerStat.BulletSpread.coefficient /= 3f;
        }
        else
        {
            if (playerStat.isNoramlMove)
            {
                playerInput.actions.FindAction("Move").Enable();
            }
            else
            {
                playerInput.actions.FindAction("Move2").Enable();
            }
            IsSiegeMode = !IsSiegeMode;
            playerStat.BulletLifeTime.coefficient /= 3f;
            playerStat.BulletSpread.coefficient *= 3f;
        }

    }
}
