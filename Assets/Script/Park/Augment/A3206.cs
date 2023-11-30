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
    private PlayerStatHandler playerStat;

    bool isLink;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnSkillEvent += MakeWall;
            controller.SkillReset();//여기부터참고
            controller.SkillMinusEvent += SkillLinkOff;
            isLink = true;
        }
    }
    private void MakeWall() 
    {
        playerStat.CurSkillStack -= 1;
        controller.playerStatHandler.CanSkill = false;
        controller.playerStatHandler.useSkill = true;
        Vector2 player =  transform.position;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (mouse - player).normalized * 1.5f;
        float angle = Mathf.Atan2(mouse.y - player.y, mouse.x - player.x) * Mathf.Rad2Deg;
        PhotonNetwork.Instantiate("AugmentList/A3206", transform.position + dir, Quaternion.Euler(new Vector3(0,0,angle-90)));
        SkillEnd();
    }

    public void SkillEnd()
    {
        if (photonView.IsMine)
        {
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
