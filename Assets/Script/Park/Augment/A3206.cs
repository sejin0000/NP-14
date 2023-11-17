using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

        Vector2 dir = (mouse - player) * 0.3f;

        //Vector2 spawnPosition = new Vector2(mouse.y - player.y + 2.5f, mouse.x - player.x +2.5f);
        float angle = Mathf.Atan2(mouse.y - player.y, mouse.x - player.x) * Mathf.Rad2Deg;
        PhotonNetwork.Instantiate("AugmentList/A3206", dir, Quaternion.AngleAxis(angle - 90, Vector2.right));
        controller.CallEndSkillEvent();
        //������ �Ǵ°ɷ� ���� �Ǵ� ������ ��������������  �÷��̾���� ���콺������ �������� �Ÿ� ++�̰������ ��������
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
