using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    //���߾� �Լ��� �������̵� �� �� ���̽��� ȣ�� �ؾ���

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
        Debug.Log("��ų �ߵ�");
        Invoke("SkillEnd",3);
    }



    public virtual void SkillEnd()
    {
        Debug.Log("��ų ����");
    }
}
