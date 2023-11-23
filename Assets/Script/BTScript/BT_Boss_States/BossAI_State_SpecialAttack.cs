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
        bossAI_Dragon.SetStateColor(Color.yellow);
        target = bossAI_Dragon.currentTarget; //��Ʈ���� ���� ������ Ÿ�� ����
    }

    public override Status Update()
    {
        //��ó���� �Ұ� -> ��� �÷��̾��� ��ġ�� �޾Ƽ�, ���� ������ �ǹ��� ������� Ȯ��
        //�����ٸ�? Ư�� ���� ���� ->�ִٸ�? ���� ��ȯ[�븻 �������� �ٷ� �Ѿ]
        //��� �ǹ� ��ġ�� �Ѿ �����Ѵٸ�? -> ���� ���ϰ� ������� ����


        SetAim(); //Ư������ ���� �� ���� �Ӹ� ���� => �׻� �÷��̾� ������

        //bossAI_Dragon.bossHead.transform.LookAt(target.position, Vector3.forward);

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            //���� �Ӹ� ���κ� ��ġ�� y�� ���� ���ʿ� ��ġ�ϴ� �÷��̾ �����Ѵٸ� => ���������� ��ġ�� ������ �����
            for (int i = 0; i < bossAI_Dragon.PlayersTransform.Count; i++)
            {
                if (bossAI_Dragon.PlayersTransform[i].position.y > 0f)
                {
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 3);
                    return Status.BT_Success;
                }
            }
            // ���� �ֱ⿡ �����ϸ� ���� ���� ����
            int randomPattern = Random.Range(0, 3);


            //������ ���� ���� RPC ���⿡ �Է� if else�� �ѹ� �� �б�(Ư�� ������ Ȯ�� ���ϰ� ���� ������ �ʿ���)

            switch (randomPattern)
            {
                case 0:
                    //�� �� ����
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 0);                  
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 1:
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 1);
                    //bossAI_Dragon.PV.RPC("Fire", RpcTarget.All);
                    break;
                case 2:
                    bossAI_Dragon.PV.RPC("ActiveAttackArea", RpcTarget.All, 2);
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

        //���� �Ӹ��� ������ ���� �� �ش� �������� RotateArm
        Vector3 direction = (target.transform.position - bossAI_Dragon.bossHead.transform.position).normalized;



        RotateArm(direction);
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;
        rotZ += 90f;

        if (rotZ > 40 || rotZ < -40f)
        {
            ReturnOriginRotate();
            //����� ��������[�� ���� �� ��������] �־ �ļ� ����
            return;
        }

        // ���ϴ� ȸ�� ���� ����
        float minRotation = -25f;
        float maxRotation = 25f;
        //270~61 == ȸ���ϸ� �ȵ�
        rotZ = Mathf.Clamp(rotZ, minRotation, maxRotation);


        // ���� ȸ�� ����
        Quaternion currentRotation = bossAI_Dragon.bossHead.transform.rotation;

        // ��ǥ ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotZ);

        // ȸ�� ����
        float interpolationFactor = 0.005f; // ���� ���
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossAI_Dragon.bossHead.transform.rotation = interpolatedRotation;
    }

    private void ReturnOriginRotate()
    {
        // ��ǥ ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        // ���� ȸ�� ����
        Quaternion currentRotation = bossAI_Dragon.bossHead.transform.rotation;

        // ȸ�� ����
        float interpolationFactor = 0.005f; // ���� ���
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossAI_Dragon.bossHead.transform.rotation = interpolatedRotation;
    }

    public override void Terminate()
    {
        ReturnOriginRotate();
    }
}
