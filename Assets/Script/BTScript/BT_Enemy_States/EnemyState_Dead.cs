using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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
        MainGameManager.Instance.currentMonsterCount -= 1;
        enemyAI.PV.RPC("DestroyEnemy", RpcTarget.AllBuffered);
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
