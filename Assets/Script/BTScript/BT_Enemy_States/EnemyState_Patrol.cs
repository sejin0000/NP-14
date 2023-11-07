using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using myBehaviourTree;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyState_Patrol : BTAction
{
    private GameObject owner;
    private EnemyAI enemyAI;
    private EnemySO enemySO;
    private Vector2 destination;   // ������
    private int patrolSpeed;       // ���� �ӵ�


    private float ActionTime;      // �ȱ� �ð�
    private float currentTime;     // �ð� ����


    float destinationX = 0f;
    float destinationY = 0f;

    float beforDestinationX = 0f;
    float beforDestinationY = 0f;
    //�ӽ�
    public GameObject tempTarget;

    public EnemyState_Patrol(GameObject _owner)
    {
        owner = _owner;

        enemyAI = owner.GetComponent<EnemyAI>();
        enemySO = enemyAI.enemySO;

        ActionTime = enemySO.actionTime;
        patrolSpeed = enemySO.patrolSpeed;


        enemyAI.nav.updateRotation = false;
        enemyAI.nav.updateUpAxis = false;

        //�ӽ� Ÿ������
        enemyAI.target = tempTarget;
    }


    //��� Start()
    public override void Initialize()
    {
        SetStateColor();
    }


    //��� ���� ���� ȣ��
    public override void Terminate()
    {

    }

    //��� Update()
    public override Status Update()
    {       
        Patrol();
        //PatrolView();
        ElapseTime();


        //���� Ž�� ������ �÷��̾ ���Դٸ� => ���� ��ȯ���� �׼� ������
        if (enemyAI.isChase)
            return Status.BT_Success;

        return Status.BT_Running;
    }


    //������ ����, �ִϸ��̼�, �׼�Ÿ��, ���ǵ�, �ø� ��� ��� �ʱ�ȭ ����
    private void Reset()
    {
        //anim.SetBool("isRun", true);
        currentTime = ActionTime;
        enemyAI.nav.speed = patrolSpeed;

        //������ ����
        enemyAI.nav.ResetPath();

        //�ִϸ��̼� �ʱ�ȭ
        //anim.SetBool("isRun", false); //anim.SetBool("Running", isRunning);

        beforDestinationX = destinationX;
        beforDestinationY = destinationY;



        destinationX = Random.Range(-6f, 6f);
        destinationY = Random.Range(-5f, 5f);


        destination.Set(destinationX, destinationY); // ������ ����


        //��������Ʈ ����(anim = �ִ� 4����[�밢] + 4����[������] ���� ����)

        if (destinationX < beforDestinationX)
        {
            enemyAI.spriteRenderer.flipX = true;
        }
        else
        {
            enemyAI.spriteRenderer.flipX = false;
        }

        Debug.Log("�ȱ�");
    }


    //���� �̵���
    private void Patrol()
    {
        if(enemyAI.nav.enabled)
        enemyAI.nav.SetDestination(destination);
        //rigid.MovePosition(transform.position + (transform.forward * applySpeed * Time.deltaTime));
        //������ٵ� �̵�(���� ��ġ���� ��������, 1�ʴ� walkSpeed ��ġ��ŭ �̵�;
    }

    //���� �ð� ����
    private void ElapseTime()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Reset();
        }
    }

    private void SetStateColor()
    {
        enemyAI.spriteRenderer.color = Color.green;
    }

    /*

    public void OnCollisionEnter2D(Collision2D collision)
    {
        var obj = collision.gameObject;
        if (collision.gameObject.tag == "Bullet")
        {
            Damage((int)obj.GetComponent<Bullet>()?.CurrentAtk);
        }
    }

    */


    /*
    //������ �޴°�� = Enemy Ÿ�Կ� ���� �������� �޴� ��쿡 �� �߰����� �ൿ(����) ���� ����
    //���� �����ͷ� �߰�����. �ֿ켱 ���� Damage, Attack, RotateArm
    public virtual void Damage(int _dmg)
    {
        if (!isDead)
        {
            HP -= _dmg;

            if (HP <= 0)
            {
                Dead();
                //switch case / enemy type�� ���� ��� +
                Destroy(gameObject);
                return;
            }

            // �ǰ� ���� ���
            //anim.SetTrigger("Hurt"); // �ǰݸ�� ����
        }
    }

    public void Attack() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        if (!isDead)
        {

            Debug.Log("����");
            isAttacking = true; // ���ݻ��� ON
            nav.ResetPath(); // ���ڸ����� �����ϵ��� �߰����� (������ ����/�׺���̼� �����Լ�)    


            //�÷��̾ �ٶ󺸵��� ����

            //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

            //����() ���� �ٲ��� �� -> ����
            Vector3 direction = (target.transform.position - bulletSpawnPoint.transform.position).normalized;

            RotateArm(direction);

            Instantiate(bullet, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);

            isAttacking = false;
        }
    }
    */

    /*
    private void RotateArm(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;

        enemyAI.EnemyAim.transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }

    */


    //�׺� ����ִ� �������� �þ߰� ���� TODO
    /*
    private void PatrolView()
    {
        Vector2 directionToDestination = (destination - (Vector2)enemyAI.transform.position).normalized;


        Vector2 rightBoundary = Quaternion.Euler(0, 0, -enemyAI.viewAngle * 0.5f) * directionToDestination;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, enemyAI.viewAngle * 0.5f) * directionToDestination;

        Debug.DrawRay(enemyAI.transform.position, rightBoundary * enemyAI.viewDistance, Color.black);
        Debug.DrawRay(enemyAI.transform.position, leftBoundary * enemyAI.viewDistance, Color.black);


        //�þ� �Ÿ�(viewDistance) ���� targetMask ����
        Collider2D _target = Physics2D.OverlapCircle(enemyAI.transform.position, enemyAI.viewDistance, enemyAI.targetMask);

        if (_target == null)
            return;




        if (_target.tag == "Player")
        {

            //�þ߰� ������ ���� Direction
            Vector2 middleDirection = (rightBoundary + leftBoundary).normalized;

            Debug.DrawRay(enemyAI.transform.position, middleDirection * enemyAI.viewDistance, Color.blue);

            //Enemy�� Player ������ ����
            Vector2 directionToPlayer = (_target.transform.position - enemyAI.transform.position).normalized;


            //�÷��̾� �þ� �߾�~Ÿ����ġ ������ ����
            float angle = Vector3.Angle(directionToPlayer, middleDirection);

            if (angle < enemyAI.viewAngle * 0.5f)
            {
                Debug.Log("�̵� �� �þ� ���� ����");
                _target = null;
            }
        }
    }
     */
}
