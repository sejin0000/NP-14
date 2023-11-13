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
    private void Awake()//���� ź���� ++ = �����ð�����
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            nowCoolGAM = playerStat.BulletSpread.total * 0.2f;
            oldCoolGAM = 0;
            MainGameManager.Instance.OnGameStartedEvent += SetCool;//�̰� ��ȹ�� �̺�Ʈ���� 
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

    void opserber() //Ÿ�ݽ� ź���� ++ ��++ ���־����Ű����� ������ �����ϴ� �Լ� ���鿹��
    {

    }
}
