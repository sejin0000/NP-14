using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A3206 : MonoBehaviourPun // 공병 생성형
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowPower;
    float oldPower;
    bool Isfirst;
    bool ready;
    bool isLink;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowPower = 0;
            oldPower = 0;
            Isfirst = false;
            ready = true;
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
        Vector2 spawnPosition = new Vector2(mouse.y - player.y + 2.5f, mouse.x - player.x +2.5f);
        float angle = Mathf.Atan2(mouse.y - player.y, mouse.x - player.x) * Mathf.Rad2Deg;
        PhotonNetwork.Instantiate("AugmentList/A3206", spawnPosition, Quaternion.AngleAxis(angle - 90, Vector2.right));
        controller.CallEndSkillEvent();
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
