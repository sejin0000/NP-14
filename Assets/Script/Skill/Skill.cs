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

    public virtual void SkillStart()
    {
        if (photonView.IsMine)
        {
            playerStats.CurSkillStack -= 1;
            Debug.Log($"��ų ��� ����, ���� ��ų ���� �� : {controller.playerStatHandler.CurSkillStack}");
            controller.playerStatHandler.CanSkill = false;
            controller.playerStatHandler.useSkill = true;

            Debug.Log("��ų �ߵ�");
        }
    }

    public virtual void SkillEnd()
    {
        if (photonView.IsMine)
        {            
            //��ų�� ������ ��Ÿ���� ����ϰ� ��Ÿ���� ������  controller.playerStatHandler.CanSkill = ����; �� �ٲ���
            Debug.Log("��ų ����");
            controller.playerStatHandler.useSkill = false;
            if (controller.playerStatHandler.CurSkillStack > 0)
            {
                controller.playerStatHandler.CanSkill = true;
            }
            controller.CallEndSkillEvent();
        }
    }

    public void OnDestroy()
    {
        if (photonView.IsMine)
        {
            controller.OnSkillEvent -= SkillStart;
            playerStats.CurSkillStack = playerStats.MaxSkillStack;
        }
    }
}
