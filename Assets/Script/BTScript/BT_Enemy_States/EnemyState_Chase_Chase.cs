using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;

public class EnemyState_Chase_Chase : BTAction
{
    private GameObject owner;

    private float speed = 5.0f;

    public EnemyState_Chase_Chase(GameObject _owner)
    {
        owner = _owner;
    }

    public override void Initialize()
    {
        SetStateColor();
    }

    public override Status Update()
    {
        OnChase();
        return Status.BT_Running;
    }

    private void OnChase()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player)
        {
            Vector3 dir = player.transform.position - owner.transform.position;

            owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 4.0f);

            owner.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void SetStateColor()
    {
        owner.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
