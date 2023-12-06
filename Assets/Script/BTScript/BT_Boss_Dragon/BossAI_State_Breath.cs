using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_Breath : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // �ð� ����   
    public BossAI_State_Breath(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.atkDelay;
    }

    public override Status Update()
    {
        //������ ��ŭ ��� -> ���� -> ������ȯ(�ٽ� ������/����� ���� üũ�ؾ���)
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);           
        }

        if(currentTime <= -2f)
        {
            return Status.BT_Success;
        }
        //�����ϴٰ� �������� �κ� ���� �� fail������Ʈ ��ȯ�ϸ� ���� ���� ����




        return Status.BT_Running;
    }
    public override void Terminate()
    {
        //�� ���� ���� ���� �� �� �ϰ������� ���� �ϼ�
    }
}