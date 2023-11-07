using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;

//����
public class EnemyState_Chase : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    private GameObject target;
    public NavMeshAgent nav;
    private float chaseSpeed = 5.0f;

    private float chaseTime = 4f;      // �ȱ� �ð�
    private float currentTime;         // �ð� ����


    public EnemyState_Chase(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        nav = owner.GetComponent<NavMeshAgent>();
        enemySO = enemyAI.enemySO;
        target = enemyAI.target;

        chaseTime = enemySO.actionTime;
        chaseSpeed = enemySO.chaseSpeed;

        enemyAI.nav.updateRotation = false;
        enemyAI.nav.updateUpAxis = false;
    }

    public override void Initialize()
    {
        SetStateColor();
        
        currentTime = chaseTime;
        enemyAI.isAttaking = false;
        enemyAI.nav.enabled = true;
    }

    public override Status Update()
    {
        OnChase();

        currentTime -= Time.deltaTime;

        Debug.Log(currentTime);

        if (currentTime <= 0.3f)
        {
            enemyAI.isChase = false;
            return Status.BT_Failure;
        }

        //�����߿�, �÷��̾��� �Ÿ��� ���� �Ÿ���  �����Ÿ� ���� �۴ٸ� BT_Success�� ���� �������� �����Ű��
        if (enemyAI.isAttaking)
            return Status.BT_Success;



        return Status.BT_Running;
    }

    //���� ������
    private void OnChase()
    {
        enemyAI.nav.SetDestination(target.transform.position);
        float distanceToTarget = Vector3.Distance(owner.transform.position, target.transform.position);

        if(distanceToTarget < enemySO.attackRange)
        {
            enemyAI.isAttaking = true;
            enemyAI.nav.enabled = false;
        }

        //��������Ʈ ����(anim = �ִ� 4����[�밢] + 4����[������] ���� ����)

        if (target.transform.position.x < owner.transform.position.x)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }
    }





    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.red;
    }
}
