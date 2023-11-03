using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;

public class EnemyState_Patrol_Move : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    protected Vector2 destination;   // ������
    private int speed;


    [SerializeField]
    protected float ActionTime;      // �ȱ� �ð�
    protected float currentTime;     //�ð� ����



    int randomX;
    int randomY;

    //��������Ʈ
    private List<Vector2> wayPointList = new List<Vector2>();
    private int currentWayPoint = 0;

    public EnemyState_Patrol_Move(GameObject _owner)
    {
        owner = _owner;       
    }

    public override void Initialize()
    {
        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

        ActionTime = enemySO.actionTime;
        speed = enemySO.wanderSpeed;

        currentTime = ActionTime;
        AddWayPoint();
    }

    public override void Terminate()
    {
        
    }

    public override Status Update()
    {
        Debug.Log(enemyAI.isWall);
        TryMove();
        return Status.BT_Running;
    }

    //�̵� ����Ʈ ����
    private void AddWayPoint()
    {
        wayPointList.Add(new Vector3(-10.0f, 20.0f, 0.0f));
        wayPointList.Add((new Vector3(10.0f, 20.0f, 0.0f)));
        wayPointList.Add((new Vector3(0.0f, 20.0f, 0.0f)));
    }
    

    //���� �̵�
    private void OnMove()
    {
        Vector2 wayPoint = wayPointList[currentWayPoint];

        float distance = Vector2.Distance(destination, owner.transform.position); //������������ �Ÿ�

              

        //ȸ��[�ø� ����/�̵� �� distance�� �������� ��������� ���� x�ø�] 
    }


    //�� ���� ��ο� �Ҵ�� �ð�
    private void TryMove()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0) // �̵� ���� �ð�
        {
            Debug.Log("�̵� ���� ����");

            randomX = Random.Range(-5, 5);
            randomY = Random.Range(-5, 5);
            currentTime = ActionTime;

            if(enemyAI.isWall)
            {
                randomX = -(randomX);
                randomY = -(randomY);

                enemyAI.isWall = false;
            }
        }

        //���� �������� ���� ���� �̵�[����� ���� ����]
        if (!enemyAI.isWall)
        {
            owner.transform.Translate(new Vector2(randomX, randomY) * speed * Time.deltaTime);
        }
    }
}
