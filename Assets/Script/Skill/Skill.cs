using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    //���߾� �Լ��� �������̵� �� �� ���̽��� ȣ�� �ؾ���

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
        Debug.Log("��ų �ߵ�");
    }



    public virtual void SkillEnd()
    {
        Debug.Log("��ų ����");
        controller.CallEndSkillEvent();
    }
}
