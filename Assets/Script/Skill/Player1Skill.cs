using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static PlayerDebuffControl;

public class Player1Skill : Skill
{
    public int applicationTime = 5;
    public float applicationspeed = 1f;
    public float applicationAtkSpeed = 1f;
    private PlayerStatHandler statHandler;
    private PlayerDebuffControl debuffControl;

    //����� Ŭ���� �ȿ� ����ȿ���� �ִ� ���������� ���� 1f�������� ������ֱ⿡ ������ ���� �����ٶ�
    public void Start()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent += SkillStart;
            isLink = true;
            controller.SkillMinusEvent += SkillLinkOff;
            debuffControl= GetComponent<PlayerStatHandler>()._DebuffControl;       
        }
        skillIcon = icons[0]; // Soldier skill icon
    }
    public override void SkillStart()
    {
        base.SkillStart();        
        playerStats.Speed.added += applicationspeed;
        playerStats.AtkSpeed.added += applicationAtkSpeed;
        debuffControl.Init(PlayerDebuffControl.buffName.Speed, applicationTime);
        Invoke("SkillEnd", applicationTime);
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
        playerStats.Speed.added -= applicationspeed;
        playerStats.AtkSpeed.added -= applicationAtkSpeed;        
    }
}
