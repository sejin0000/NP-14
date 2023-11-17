using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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

        Vector2 dir = (mouse - player) * 0.3f;

        //Vector2 spawnPosition = new Vector2(mouse.y - player.y + 2.5f, mouse.x - player.x +2.5f);
        float angle = Mathf.Atan2(mouse.y - player.y, mouse.x - player.x) * Mathf.Rad2Deg;
        PhotonNetwork.Instantiate("AugmentList/A3206", dir, Quaternion.AngleAxis(angle - 90, Vector2.right));
        controller.CallEndSkillEvent();
        //문제가 되는걸로 예상 되는 부위는 스폰포지션으로  플레이어기준 마우스벡터쪽 에가깝게 거리 ++이고싶은데 쉽지않음
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
