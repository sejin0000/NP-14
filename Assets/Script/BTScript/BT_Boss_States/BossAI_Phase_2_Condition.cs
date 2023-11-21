using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//2������ : �ϴ� ����->������ ������ 1������ �ϼ� ��ǥ


public class BossAI_Phase_2_Condition : BTCondition
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float bossPatternTime;           // ���� ���� : �ϴ� 3~5�� ���� �ٱ�?
    private float currentTime;         // �ð� ����
    private float percentHP;
    public BossAI_Phase_2_Condition(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
        bossPatternTime = bossSO.bossPatternTime;
    }

    public override void Initialize()
    {
        //���� ����ó��
        currentTime = bossPatternTime;
        //������
        //enemyAI.nav.enabled = true;
    }

    public override Status Update()
    {
        if (percentHP >= 50)//(���� ü���� 50% �̸�) => ���� �������
            return Status.BT_Failure;

        currentTime -= Time.deltaTime;

        percentHP = (bossAI_Dragon.currentHP / bossSO.hp * 100);



        //���� �ֱ�

        if (currentTime <= 0.3f)
        {
            return Status.BT_Running;
        }
        else
            return Status.BT_Success;
    }
}