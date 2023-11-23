using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState_GroggyCondition : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;

    private float currentTime;         // 시간 계산용
    public EnemyState_GroggyCondition(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

    }

    public override void Initialize()
    {
        currentTime = enemySO.chaseTime;
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            enemyAI.isGroggy = false;
            enemyAI.nav.isStopped = false;
            //여기에 RPC메서드
            currentTime = enemySO.chaseTime;
        }




        if (enemyAI.isGroggy) 
        {
            SetStateColor();
            return Status.BT_Running;
        }
        else
            return Status.BT_Failure;
    }


    //public void ReturnGold

    public override void Terminate()
    {
        enemyAI.spriteRenderer.color = new Color(255 / 255f, 255 / 255f, 255 / 255f ,255/255f);
    }

    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.blue;
    }
}
