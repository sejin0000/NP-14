using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_Dead : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    public BossAI_State_Dead(GameObject _owner)
    {
        owner = _owner;

        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;

    }

    public override void Initialize()
    {
        //��� ���⿡
        //��� �� ��ƼŬ�̳� ��Ÿ ȿ�� ���⿡
        PhotonView photonView = PhotonView.Find(bossAI_Dragon.lastAttackPlayer);
        if (!photonView.gameObject.GetComponent<PlayerStatHandler>())
        {
            return;
        }
        PlayerStatHandler targetPlayer = photonView.gameObject.GetComponent<PlayerStatHandler>(); ;
        targetPlayer.photonView.RPC("KillEvent", RpcTarget.All);

        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.currentMonsterCount -= 1;
        }
        bossAI_Dragon.PV.RPC("DestroyEnemy", RpcTarget.All);
    }

    public override Status Update()
    {
        //�� �κ� ���߿� �ڷ�ƾ���� ����
        return Status.BT_Success;
    }


    //public void ReturnGold

    public override void Terminate()
    {

    }
}
