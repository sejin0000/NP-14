using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0103 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    float nowCoolGAM;
    float oldCoolGAM;
    private void Awake()//난사 탄퍼짐 ++ = 장전시간감소
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
            oldCoolGAM = 0;
            MainGameManager.Instance.OnGameStartedEvent += SetCool;//이걸 돈획득 이벤트에검 
        }
    }
    // Update is called once per frame
    void SetCool()
    {
        nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
        playerStat.ReloadCoolTime.added += nowCoolGAM;
        playerStat.ReloadCoolTime.added -= oldCoolGAM;
        oldCoolGAM = nowCoolGAM;
    }

    void opserber() //타격시 탄퍼짐 ++ 뎀++ 이있었던거같은데 있으면 대응하는 함수 만들예정
    {

    }
}
