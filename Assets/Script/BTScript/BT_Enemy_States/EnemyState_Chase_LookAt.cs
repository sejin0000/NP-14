using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

public class EnemyState_Chase_LookAt : BTAction
{
    private GameObject owner;

    public EnemyState_Chase_LookAt(GameObject _owner)
    {
        owner = _owner;
    }

    //���� �׼� ���� �ٲ� �� ����
    public override void Initialize()
    {
        SetStateColor();
    }

    //���� �׼� ��尡 ���� �� ����
    public override void Terminate()
    {

    }

    public override Status Update()
    {
        OnLookAt();

        return Status.BT_Running;
    }

    private void SetStateColor()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    private void OnLookAt()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            Vector3 dir = player.transform.position = owner.transform.position;


            //ȸ��
            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 4.0f);
        }
    }

}
