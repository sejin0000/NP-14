using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_State_ReturnRoot : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // �ð� ����   
    public BossAI_State_ReturnRoot(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
    }

    public override void Initialize()
    {
        //���� ��忡�� ���и� ��ȯ�� �� ����
        //���� ��忡�� Ʈ�� ������� �ʿ��� ��� ����
    }

    public override Status Update()
    {
        return Status.BT_Success;
    }
    public override void Terminate()
    {
        Debug.Log("���� ����");
        //��Ʈ�� �ö󰡱����� �ϰ���� �� ������ ����
    }
}
