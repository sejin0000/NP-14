using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1303 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler stats;
    private CoolTimeController coolTimeController;
    private WeaponSystem _ws;

    private void Awake()
    {
        if (photonView.IsMine
            && _ws.weaponType != WeaponSystem.WeaponType.Charging)
        {
            controller = GetComponent<TopDownCharacterController>();
            stats = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();
            _ws = GetComponent<WeaponSystem>();
            _ws.weaponType = WeaponSystem.WeaponType.Charging;
            SetCharge();
        }
    }

    private void SetCharge()
    {
        controller.OnAttackKeepEvent += coolTimeController.TimeCount;
        controller.OnAttackEvent -= coolTimeController.AttackCoolTime;
        controller.OnAttackEvent -= _ws.Shooting;
        controller.OnChargeAttackEvent += _ws.Charging;
        controller.playerStatHandler.IsChargeAttack = true;
    }
}
