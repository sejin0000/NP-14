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
    private float damageCoeff;

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

            _ws.OnFinalDamageEvent += FinalAttackBonus;
            damageCoeff = 0.5f;
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

    private void FinalAttackBonus(float damage)
    {
        if (stats.CurAmmo == 0)
        {
            //TODO weaponSystem에 finalAttack 계수 만들기
        }        
    }
}
