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
    private Transform target;

    private float currentTime;         // �ð� ����   
    public BossAI_State_SpecialAttack(GameObject _owner)
    {
        owner = _owner;
        bossAI_Dragon = owner.GetComponent<BossAI_Dragon>();
        bossSO = bossAI_Dragon.bossSO;
    }

    public override void Initialize()
    {
        currentTime = bossSO.atkDelay;
        SetStateColor();
        target = bossAI_Dragon.currentTarget; //��Ʈ���� ���� ������ Ÿ�� ����
    }

    public override Status Update()
    {
        //��ó���� �Ұ� -> ��� �÷��̾��� ��ġ�� �޾Ƽ�, ���� ������ �ǹ��� ������� Ȯ��
        //�����ٸ�? Ư�� ���� ���� ->�ִٸ�? ���� ��ȯ[�븻 �������� �ٷ� �Ѿ]
        //��� �ǹ� ��ġ�� �Ѿ �����Ѵٸ�? -> ���� ���ϰ� ������� ����

        for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
        {
            float distanceToTargets = Vector2.Distance(bossAI_Dragon.PlayersTransform[i].position, bossAI_Dragon.bossHead.position);

            if (distanceToTargets > 0.3f)
                return Status.BT_Failure;
        }

        SetAim(); //Ư������ ���� �� ���� �Ӹ� ���� => �׻� �÷��̾� ������

        //bossAI_Dragon.bossHead.transform.LookAt(target.position, Vector3.forward);

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // ���� �ֱ⿡ �����ϸ� ���� ���� ����
            int randomPattern = Random.Range(0, 2);


            //������ ���� ���� RPC ���⿡ �Է�
            switch(randomPattern)
            {
                case 0:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 1:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 2:
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
            }            
            currentTime = bossSO.atkDelay; //�ð� �ʱ�ȭ
        }



        //float distanceToTarget = Vector2.Distance(owner.transform.position, target.transform.position);

        return Status.BT_Running;
    }
    public void SetAim() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        //�÷��̾ �ٶ󺸵��� ����

        //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

        //����() ���� �ٲ��� �� -> ����
        Vector3 direction = (target.transform.position - bossAI_Dragon.bossHeadPivot.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        bossAI_Dragon.bossHead.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
    public override void Terminate()
    {
    }

    private void SetStateColor()
    {
        bossAI_Dragon.spriteRenderer.color = Color.black;
    }
}
