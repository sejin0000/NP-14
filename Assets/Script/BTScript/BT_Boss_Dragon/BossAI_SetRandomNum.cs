using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_SetRandomNum : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;



    int randomNum;
    int beforRandomNum;


    private float currentTime;         // 시간 계산용   
    public BossAI_SetRandomNum(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
    }

    public override void Initialize()
    {
        do
        {
            randomNum = Random.Range(0, 3);
        }
        while (randomNum == beforRandomNum);



        beforRandomNum = randomNum;
        bossAI_Dragon.currentNomalAttackSquence = randomNum;
    }
}
