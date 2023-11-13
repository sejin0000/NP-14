using myBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyState_Attack : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO; //�Ѿ� ���ݷ� �޾ƾ���
    private GameObject target;

    private float currentTime;         // �ð� ����

    public EnemyState_Attack(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;      
    }

    public override void Initialize()
    {
        currentTime = enemySO.atkdelay;
        SetStateColor();

        target = enemyAI.target;

        SetAim();
        enemyAI.Shoot();
    }

    public override Status Update()
    {
        SetAim();

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            // ���� �ֱ⿡ �����ϸ� ���� ����
            enemyAI.Shoot();
            currentTime = enemySO.atkdelay;
        }

        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if (distanceToTarget > enemySO.attackRange)
        {
            enemyAI.isAttaking = false;
            return Status.BT_Failure; // ��� ����
        }

        if (target == null)
        {
            enemyAI.isAttaking = false;
            enemyAI.isChase = false;
            return Status.BT_Failure;
        }


        //�ڡڡڼ�����
        enemyAI.isFilp(owner.transform.position.x, target.transform.position.x);


        return Status.BT_Running;
    }

    public override void Terminate()
    {      
    }


    public void SetAim() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        //�÷��̾ �ٶ󺸵��� ����

        //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

        //����() ���� �ٲ��� �� -> ����
        Vector3 direction = (target.transform.position - enemyAI.enemyAim.transform.position).normalized;

        RotateArm(direction);        
    }
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        enemyAI.enemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    private void SetStateColor()
    {
       enemyAI.spriteRenderer.color = Color.black;
    }
}
