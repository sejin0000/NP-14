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
            Debug.Log("��ų �ߵ�");
        }
    }

    public virtual void SkillEnd()
    {
        if (photonView.IsMine)
        {
            //��ų�� ������ ��Ÿ���� ����ϰ� ��Ÿ���� ������  controller.playerStatHandler.CanSkill = ����; �� �ٲ���
            Debug.Log("��ų ����");
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
