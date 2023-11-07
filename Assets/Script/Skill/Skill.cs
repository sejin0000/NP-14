using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    //버추얼 함수는 오버라이딩 후 꼭 베이스를 호출 해야함

    private TopDownCharacterController controller;
    private PlayerStatHandler playerStats;

    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStats = GetComponent<PlayerStatHandler>();
    }

    private void Start()
    {
        controller.OnSkillEvent += SkillStart;
        controller.OnEndSkillEvent += SkillEnd;
    }


    public virtual void SkillStart()
    {
        controller.playerStatHandler.CanSkill = false;
        Debug.Log("스킬 발동");
        Invoke("SkillEnd",3);
    }



    public virtual void SkillEnd()
    {
        Debug.Log("스킬 종료");
    }
}
