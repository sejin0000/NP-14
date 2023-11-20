using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class A3105 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowPower;
    float oldPower;
    bool Isfirst;
    bool ready;
    bool isLink;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowPower = 0;
            oldPower = 0;
            Isfirst = false;
            ready = true;
            controller.OnSkillEvent += SetPower;
            controller.OnEndAttackEvent += LostPower;

            controller.SkillReset();//여기부터참고
            controller.SkillMinusEvent += SkillLinkOff;
            isLink = true;
        }
    }
    // Update is called once per frame
    void SetPower()
    {
        if (ready) 
        {
            nowPower = playerStat.ATK.total;
            playerStat.ATK.added += nowPower;
            oldPower = nowPower;
            ready = false;
            Isfirst = true;
        }
        SkillEnd();

    }
    void LostPower()
    {
        if (Isfirst)
        {
            playerStat.ATK.added -= oldPower;
            ready = true;
        }
        Isfirst = false;

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
                controller.OnSkillEvent -= SetPower;
                controller.OnEndAttackEvent -= LostPower;
                isLink = false;
            }
        }
    }
}
