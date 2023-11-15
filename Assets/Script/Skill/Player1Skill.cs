using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player1Skill : Skill
{
    public int applicationTime = 5;
    public float applicationspeed = 2f;
    public float applicationAtkSpeed =2f;
    //����� Ŭ���� �ȿ� ����ȿ���� �ִ� ���������� ���� 1f�������� ������ֱ⿡ ������ ���� �����ٶ�
    public override void Start()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent += SkillStart;
        }
    }
    public override void SkillStart()
    {
        base.SkillStart();        
        playerStats.Speed.added += applicationspeed;
        playerStats.AtkSpeed.added += applicationAtkSpeed;
        Invoke("SkillEnd", applicationTime);
    }


    public override void SkillEnd()
    {
        base.SkillEnd();
        playerStats.Speed.added -= applicationspeed;
        playerStats.AtkSpeed.added -= applicationAtkSpeed;        
    }
}
