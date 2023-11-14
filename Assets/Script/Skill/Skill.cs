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
        if (photonView.IsMine)
        {
            controller.OnSkillEvent += SkillStart;
        }
    }


    public virtual void SkillStart()
    {
        if (photonView.IsMine)
        {
            controller.playerStatHandler.CanSkill = false;
            Debug.Log("스킬 발동");
        }
    }

    public virtual void SkillEnd()
    {
        if (photonView.IsMine)
        {
            //스킬이 끝나면 쿨타임을 계산하고 쿨타임이 끝나면  controller.playerStatHandler.CanSkill = 진실; 로 바꿔줌
            Debug.Log("스킬 종료");
            controller.CallEndSkillEvent();
        }
    }

    public void OnDestroy()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent -= SkillStart;
        }
    }
}
