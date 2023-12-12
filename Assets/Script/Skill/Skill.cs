using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviourPun
{
    //���߾� �Լ��� �������̵� �� �� ���̽��� ȣ�� �ؾ���

    protected TopDownCharacterController controller;
    protected PlayerStatHandler playerStats;
    public bool isLink;

    protected Sprite[] icons;
    protected Sprite skillIcon;
    public Sprite Skillicon { get { return skillIcon; } }

    public void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStats = GetComponent<PlayerStatHandler>();
        icons = Resources.LoadAll<Sprite>("Images/Skill_icon-Sheet");
    }

    public void SkillLinkOff()
    {
        if (photonView.IsMine)
        {
            if (isLink) 
            {
                Debug.Log("�����ϰ� ���ŉ�ٰ� ������");
                controller.OnSkillEvent -= SkillStart;
                isLink = false;
            }
        }
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
        if (NetworkManager.Instance != null)
        {
            return;
        }

        if (photonView.IsMine)
        {
            controller.OnSkillEvent -= SkillStart;
            playerStats.CurSkillStack = playerStats.MaxSkillStack;
        }        
    }
}
