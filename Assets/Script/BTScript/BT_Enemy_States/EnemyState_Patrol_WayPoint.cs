using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;
using Unity.VisualScripting;

public class EnemyState_Patrol_WayPoint : BTAction
{
    private GameObject owner;

    private float speed = 3.0f;

    //��������Ʈ
    private List<Vector3> wayPointList = new List<Vector3>();
    private int currentWayPoint = 0;
    private float wayPointRadius = 2.0f;

    public EnemyState_Patrol_WayPoint(GameObject _owner)
    {
        owner = _owner;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Terminate()
    {
        
    }

    public override Status Update()
    {
        OnMove();
        return Status.BT_Running;
    }

    private void AddWayPoint()
    {
        wayPointList.Add(new Vector3(-10.0f, 20.0f, 0.0f));
        wayPointList.Add((new Vector3(10.0f, 20.0f, 0.0f)));
    }

    private void OnMove()
    {
        Vector3 wayPoint = wayPointList[currentWayPoint];

        float distance = Vector3.Distance(wayPoint, owner.transform.position);

        //���� ��������Ʈ
        if(distance < wayPointRadius)
        {
            if (++currentWayPoint >= wayPointList.Count)
                currentWayPoint = 0;
            wayPoint = wayPointList[currentWayPoint];
        }
        Vector3 dir = wayPoint - owner.transform.position;

        //ȸ��
        owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 4.0f);
        //�̵�
        owner.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
