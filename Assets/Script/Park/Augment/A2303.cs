using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A2303 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private PlayerInputController playerInputController;
    private WeaponSystem weaponSystem;
    bool isLink;
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<PlayerInputController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController= GetComponent<CoolTimeController>();
            playerInputController = GetComponent<PlayerInputController>();
            weaponSystem =GetComponent<WeaponSystem>();

            controller.OnSkillEvent += AcroboticShot;

            controller.SkillReset();//여기부터참고
            controller.SkillMinusEvent += SkillLinkOff;
            isLink = true;
        }
    }

    private void AcroboticShot() 
    {
        playerStat.CurSkillStack -= 1;
        controller.playerStatHandler.CanSkill = false;
        controller.playerStatHandler.useSkill = true;

        coolTimeController.EndRollCoolTime();//구르기 쿨타임 초기화 
        playerInputController.CallRollEvent(); // 구르기 시전 
        Invoke("shoting",0.6f);
        //controller.CallEndSkillEvent();
        SkillEnd();
    }
    private void shoting() 
    {
        int n = 0;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, n));
        for (int i = 0; i < 18; ++i)
        {
            weaponSystem.burstCall(rot);
            n += 20;
            rot = Quaternion.Euler(new Vector3(0, 0, n));
        }
    }


    public void SkillEnd()
    {
        if (photonView.IsMine)
        {
            //스킬이 끝나면 쿨타임을 계산하고 쿨타임이 끝나면  controller.playerStatHandler.CanSkill = 진실; 로 바꿔줌
            Debug.Log("스킬 종료");
            controller.playerStatHandler.useSkill = false;
            if (controller.playerStatHandler.CurSkillStack > 0)
            {
                controller.playerStatHandler.CanSkill = true;
            }
            controller.CallEndSkillEvent();
        }
    }
    public void SkillLinkOff()
    {
        if (photonView.IsMine)
        {
            if (isLink)
            {
                controller.OnSkillEvent -= AcroboticShot;
                isLink = false;
            }
        }
    }
}
