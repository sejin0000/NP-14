using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

//[Action ���]
//BTAction ��ӹ޴� ����?
//==>���� ���� ������Ʈ�� �ൿ�� �����ϱ� ����.
//override Status Update �޼��带 �����Ͽ� ���� �ൿ�� ������
public class EnemyState_Patrol_Rotation : BTAction
{
    private GameObject owner;

    private float intervalTime = 1.0f;
    private Vector3 direction;

    public EnemyState_Patrol_Rotation(GameObject currentOwner)
    {
        owner = currentOwner;
    }

    //�ൿ ��尡 ó�� ���� �� �� : �ʱ�ȭ �۾� (����� Start())
    public override void Initialize()
    {
        Debug.Log("������ ������");
        //������ ���⼳��        
        SetStateColor();
        InitDirection();
    }


    //����� �״��
    public override void Terminate()
    {
        base.Terminate();
    }

    //���� ������Ʈ �� ����
    public override Status Update()
    {
        Debug.Log("�����̵� ��");
        //���� ����
        OnRotationByDir();
        //���� ��ȯ
        return Status.BT_Running;
    }

    //���¿� ���� ��������Ʈ ������ ����(�׳� �ӽ� : �ൿ�� �ð������� �� ���̰�)
    private void SetStateColor()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.green;
    }

    //������ �������� ����&����ȭ (���͸�)
    private void InitDirection()
    {
        float x = Random.Range(-1.0f, 1.0f);
        float y = Random.Range(-1.0f, 1.0f);

        direction = new Vector3(x, y, 0);
        direction.Normalize();
    }

    //intervalTime�� ����Ͽ� ���� �������� ������ ����&ȸ��
    //���ο� ���⵵ �������� ���� 
    //Quaternion.Slerp�� ����Ͽ� �ε巴�� ȸ��
    public void OnRotationByDir()
    {
        intervalTime += Time.deltaTime;

        if(intervalTime < 0.0f)
        {
            float x = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(-1.0f, 1.0f);

            //����ó��(���� ��ǥ ������ 0,0���� ������)
            if(x == 0.0f && y == 0.0f)
            {
                x = 1.0f;
            }

            direction = new Vector3(x, y, 0);

            //interval �缳��
            intervalTime = Random.Range(0.5f, 3.0f);
        }

        //���� ��ȯ �� �ε巴�� ȸ��
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime);
    }
}