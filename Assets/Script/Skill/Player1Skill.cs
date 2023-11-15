using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player1Skill : Skill
{
    public int applicationTime = 5;
    public float applicationspeed = 2f;
    public float applicationAtkSpeed =2f;
    //디버프 클래스 안에 절반효과를 주는 열광전염이 있음 1f기준으로 설계되있기에 수정시 같이 수정바람
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
