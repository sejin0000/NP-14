using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class A3206 : MonoBehaviourPun // 공병 생성형
{
    private TopDownCharacterController controller;

    bool isLink;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            controller.OnSkillEvent += MakeWall;
            controller.SkillReset();//여기부터참고
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
            //스킬이 끝나면 쿨타임을 계산하고 쿨타임이 끝나면  controller.playerStatHandler.CanSkill = 진실; 로 바꿔줌
            Debug.Log("스킬 종료");
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
