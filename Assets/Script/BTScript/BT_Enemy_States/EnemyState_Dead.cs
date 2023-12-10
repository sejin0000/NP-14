using myBehaviourTree;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyState_Dead : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;

    public EnemyState_Dead(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

    }

    public override void Initialize()
    {
        //��� ���⿡
        //��� �� ��ƼŬ�̳� ��Ÿ ȿ�� ���⿡
        ParticleManager.Instance.PlayEffect("Droplet_PS", owner.transform.position);

        PhotonView photonView = PhotonView.Find(enemyAI.lastAttackPlayer);
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
        enemyAI.PV.RPC("DestroyEnemy", RpcTarget.All);
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
