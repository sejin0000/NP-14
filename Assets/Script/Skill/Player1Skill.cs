using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player1Skill : Skill
{
    public static int applicationTime = 5;
    public static float applicationspeed = 5f;
    public static float applicationAtkSpeed =5f;
    //����� Ŭ���� �ȿ� ����ȿ���� �ִ� ���������� ���� 1f�������� ������ֱ⿡ ������ ���� �����ٶ�
    public override void SkillStart()
    {
        base.SkillStart();
        playerStats.Speed.added += applicationspeed;
        playerStats.AtkSpeed.added += applicationAtkSpeed;
        Invoke("SkillEnd", applicationTime);
    }


    public override void SkillEnd()
    {
        playerStats.Speed.added -= applicationspeed;
        playerStats.AtkSpeed.added -= applicationAtkSpeed;
        base.SkillEnd();
    }
}
