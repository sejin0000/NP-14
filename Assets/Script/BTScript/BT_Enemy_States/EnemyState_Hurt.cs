using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;
using Unity.VisualScripting;

public class EnemyState_Hurt : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;

    private float ActionTime;
    private float currentTime;
    private Vector3 destination;   // ������
    private Vector3 RunDirection;

    private float runSpeed;

    //�ӽ�
    public GameObject tempTarget;


    //��� ������ Awake()
    public EnemyState_Hurt(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

        ActionTime = enemySO.actionTime;
        runSpeed = enemySO.runSpeed;

        //�ӽ� Ÿ������
        enemyAI.target = tempTarget;
    }

    //��� Start() !�⺻ Update ���� �۾��� �Ͼ
    public override void Initialize()
    {
        if(!enemyAI.isLive)
            EnemyDead();


        if (enemySO.type == EnemyType.Melee || enemySO.type == EnemyType.Ranged)
            PassNord();

        else if (enemySO.type == EnemyType.Coward)
            ;

    }


    //��� ���� ���� ȣ��
    public override void Terminate()
    {

    }



    //�ǰ� �� ����
    public void DefaultHurt(int damage)
    {
        PassNord();
    }

    public void CowardHurt()
    {
        ;
        //���� ��ũ��Ʈ
    }



    //��� Update()
    public override Status Update()
    {
        return Status.BT_Running;
    }



    public Status PassNord()
    {
        return Status.BT_Success;
    }

    /*
    public void RunEnemy(Vector3 _targetPos)
    {
        RunDirection = Quaternion.LookRotation(enemyAI.transform.position - _targetPos).eulerAngles;

        currentTime = ActionTime;
        enemyAI.nav.speed = runSpeed;
        //���� anim = walk�� �Ȱ��� �����ϸ�ɵ�

        //�÷��̾� ��ġ�� �ް�, �� ����Ƽ���̼� �缳�� -> �̵�
        enemyAI.nav.SetDestination(destination);
    }

    */

    public void EnemyDead()
    {
        Debug.Log("�� ĳ���� ���");
    }


    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Reset();
        }
    }
}
