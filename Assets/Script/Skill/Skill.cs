using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    //버추얼 함수는 오버라이딩 후 꼭 베이스를 호출 해야함

    protected TopDownCharacterController controller;
    protected PlayerStatHandler playerStats;

    public virtual void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStats = GetComponent<PlayerStatHandler>();
    }

    public virtual void Start()
    {
        controller.OnSkillEvent += SkillStart;
    }


    public virtual void SkillStart()
    {
        controller.playerStatHandler.CanSkill = false;
        Debug.Log("스킬 발동");
    }



    public virtual void SkillEnd()
    {
        Debug.Log("스킬 종료");
        controller.CallEndSkillEvent();
    }
}
