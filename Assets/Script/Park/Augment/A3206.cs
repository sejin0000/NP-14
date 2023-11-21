using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class A3206 : MonoBehaviourPun // ���� ������
{
    private TopDownCharacterController controller;

    bool isLink;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            controller.OnSkillEvent += MakeWall;
            controller.SkillReset();//�����������
            controller.SkillMinusEvent += SkillLinkOff;
            isLink = true;
        }
    }
    private void MakeWall() 
    {
        Vector2 player =  transform.position;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (mouse - player).normalized * 1.5f;
        //transform.position + dir;

        //Vector2 spawnPosition = new Vector2(mouse.y - player.y + 2.5f, mouse.x - player.x +2.5f);
        float angle = Mathf.Atan2(mouse.y - player.y, mouse.x - player.x) * Mathf.Rad2Deg;
        //Quaternion.AngleAxis(angle - 90, Vector2.right)
        PhotonNetwork.Instantiate("AugmentList/A3206", transform.position + dir, Quaternion.Euler(new Vector3(0,0,angle-90)));
        //controller.CallEndSkillEvent();
        SkillEnd();
    }

    public void SkillEnd()
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
    public void SkillLinkOff()
    {
        if (photonView.IsMine)
        {
            if (isLink)
            {
                controller.OnSkillEvent -= MakeWall;
                isLink = false;
            }
        }
    }
}
