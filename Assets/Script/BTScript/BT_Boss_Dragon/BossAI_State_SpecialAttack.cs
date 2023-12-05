using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Ư�� ���� ��� : �극�� / ��� / ������
public class BossAI_State_SpecialAttack : BTAction
{
    private GameObject owner;
    private BossAI_Dragon bossAI_Dragon;
    private EnemySO bossSO;

    private float currentTime;         // �ð� ����   
    public BossAI_State_SpecialAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.SpecialAttackDelay;
    }

    public override Status Update()
    {
        //��ó���� �Ұ� -> ��� �÷��̾��� ��ġ�� �޾Ƽ�, ���� ������ �ǹ��� ������� Ȯ��
        //�����ٸ�? Ư�� ���� ���� ->�ִٸ�? ���� ��ȯ[�븻 �������� �ٷ� �Ѿ]
        //��� �ǹ� ��ġ�� �Ѿ �����Ѵٸ�? -> ���� ���ϰ� ������� ����


        //���� �κ� ����� ���� ��������� �̰��Ұ� Failure ����
        float distanceToTarget = Vector2.Distance(owner.transform.position, bossAI_Dragon.currentTarget.transform.position);

        if(distanceToTarget > 7f)
        {
            return Status.BT_Failure;
        }


        Debug.Log("Ư�� ���� ���� ���� ��");

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            
            //Ư������ 1 : �÷��̾ �Ӹ� �������� �Ѿ ��� => ��ü ����
            for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
            {
                if (bossAI_Dragon.PlayersTransform[i].position.y > 0f)
                {
                    Debug.Log("�÷��̾ ���� ���� ���� Ȱ��ȭ: " + i);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    return Status.BT_Failure; //�̰� �� �����ϼ� (���� -> Ư�� ���� ���� �� �ٷ� �븻 / ���� -> �ٽ� ó������ ����)
                }
            }


            //Ư������ 2 : ����



            //Ư�� ���� 3 : �÷��̾ Ư�� ���� ���� �ȿ� �ִ� ��� [���] => �극��
            bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);



            /*
            // ���� �ֱ⿡ �����ϸ� ���� Ư�� ���� ����
            int randomPattern = Random.Range(0, 4);


            //������ ���� ���� RPC ���⿡ �Է� if else�� �ѹ� �� �б�(Ư�� ������ Ȯ�� ���ϰ� ���� ������ �ʿ���)

            switch (randomPattern)
            {
                case 0:
                    //�� �� ����
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);                   
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);                  
                    break;
                case 1:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;
                case 2:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;  
                case 3:
                    //bossAI_Dragon.PV.RPC("StartBreathCoroutine", RpcTarget.All);
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    break;
            }            
            */
            currentTime = bossSO.atkDelay; //�ð� �ʱ�ȭ
        }


        

        return Status.BT_Running;
    }

    public override void Terminate()
    {
    }
}
