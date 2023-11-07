using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Player1Skill : Skill
{
    public override void SkillStart()
    {
        playerStats.Speed.coefficient *= 2;
        playerStats.AtkSpeed.coefficient *= 2;
        Invoke("SkillEnd", 3);
        base.SkillStart();
    }


    public override void SkillEnd()
    {
        playerStats.Speed.coefficient  *= 0.5f;
        playerStats.AtkSpeed.coefficient *= 0.5f;
        base.SkillEnd();
    }
}
