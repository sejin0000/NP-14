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
    protected Vector2 destination;   // 목적지
    private int speed;


    [SerializeField]
    protected float ActionTime;      // 걷기 시간
    protected float currentTime;     //시간 계산용



    int randomX;
    int randomY;

    //웨이포인트
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

    //이동 포인트 지정
    private void AddWayPoint()
    {
        wayPointList.Add(new Vector3(-10.0f, 20.0f, 0.0f));
        wayPointList.Add((new Vector3(10.0f, 20.0f, 0.0f)));
        wayPointList.Add((new Vector3(0.0f, 20.0f, 0.0f)));
    }
    

    //실제 이동
    private void OnMove()
    {
        Vector2 wayPoint = wayPointList[currentWayPoint];

        float distance = Vector2.Distance(destination, owner.transform.position); //목적지까지의 거리

              

        //회전[플립 넣자/이동 시 distance가 음수인지 양수인지에 따라 x플립] 
    }


    //각 순찰 경로에 할당된 시간
    private void TryMove()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0) // 이동 리셋 시간
        {
            Debug.Log("이동 조건 만족");

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

        //랜던 목적지에 따른 실제 이동[여기다 조건 달자]
        if (!enemyAI.isWall)
        {
            owner.transform.Translate(new Vector2(randomX, randomY) * speed * Time.deltaTime);
        }
    }
}
