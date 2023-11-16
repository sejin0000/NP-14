using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using Photon.Pun;
using static UnityEngine.RuleTile.TilingRuleOutput;

//����
public class EnemyState_Chase : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    public NavMeshAgent nav;

    private float chaseTime;           // �ȱ� �ð�
    private float currentTime;         // �ð� ����


    public EnemyState_Chase(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        nav = owner.GetComponent<NavMeshAgent>();
        enemySO = enemyAI.enemySO;

        chaseTime = enemySO.chaseTime;

        //enemyAI.nav.updateRotation = false;
        //enemyAI.nav.updateUpAxis = false;
    }

    public override void Initialize()
    {
        SetStateColor();
        enemyAI.ChangeSpeed(enemySO.enemyChaseSpeed);
        currentTime = chaseTime;
        //������
        //enemyAI.nav.enabled = true;
    }

    public override void Terminate()
    {
    }

    public override Status Update()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0.3f)
        {
            enemyAI.isChase = false;
            enemyAI.Target = null;

            return Status.BT_Failure;
        }

        //�����߿�, �÷��̾��� �Ÿ��� ���� �Ÿ���  �����Ÿ� ���� �۴ٸ� BT_Success�� ���� �������� �����Ű��
        if (enemyAI.isAttaking)
            return Status.BT_Success;


        OnChase();

        return Status.BT_Running;
    }

    //���� ������
    //�׺�(��忡�� >>>>>>>>> ��� �ι��̻� = �޼��� == ��Ʈ���� �ι��̻� ����ϴ� ��ɵ��� �޼��� ȭ -> ��û�� �ϰ�, �޼��� ȣ��)
    //���� ��ǥ�� ����, �ø�, ....
    private void OnChase()
    {
        if (enemyAI.photonView.AmOwner)
            enemyAI.navTargetPoint = enemyAI.Target.transform.position;

        enemyAI.DestinationSet();


        float distanceToTarget = Vector3.Distance(owner.transform.position, enemyAI.Target.transform.position);

        if (distanceToTarget < enemySO.attackRange)
        {
            enemyAI.isAttaking = true;
            //������
            //enemyAI.nav.enabled = false;
        }

        //��������Ʈ ����(anim = �ִ� 4����[�밢] + 4����[������] ���� ����)

        //������ ���� & �� ��ġ~Ÿ����ġ & 

        //isflip(���� �ޱ� Ÿ�� Ʈ������ x, �� Ʈ������ x)


        //�ڡڡڼ�����
        //enemyAI.PV.RPC("Filp", RpcTarget.All);
        //enemyAI.Filp(owner.transform.position.x, enemyAI.Target.transform.position.x);

        /*
        if (target.transform.position.x < owner.transform.position.x)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }
        */
    }





    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.gray;
    }
}
