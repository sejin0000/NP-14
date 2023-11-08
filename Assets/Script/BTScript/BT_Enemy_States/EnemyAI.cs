using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using static UnityEditor.PlayerSettings;
using UnityEditor.Rendering.LookDev;

//[Root ���] => �� �׼ǰ� �ٸ��� ��� �ȹ���?
//==>Ư�� AI ���۰� ���¿� �°� �����ϰ� �����ϱ� ���ؼ�
//��� BTRoot ��ü�� �����ϰ�, �� �Ʒ��� ���� ���� �׼� ��带 �߰��ؼ� Ʈ���� ������
//EnemyAI : AI�� ���¿� ������ ������ �����ϰ�, ������


//Enemy�� �ʿ��� ������Ʈ�� + ��Ÿ ��ҵ� ���⿡ �� �߰�
public class EnemyAI : MonoBehaviour
{
    private BTRoot TreeAIState;

    public float currentHP;                  // ���� ü�� ���
     

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO enemySO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D collider2D;
    public Animator anim;  
    public GameObject target;                //���� Ÿ��[Palyer]
    public NavMeshAgent nav;

    public GameObject enemyAim;
    public GameObject enemyBullet;

    public float viewAngle;                  // �þ߰� (�⺻120��)
    public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)
    public LayerMask targetMask;             // Ÿ�� ���̾�(Player)

    public bool isLive;
    public bool isChase;
    public bool isAttaking;          
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponentInChildren<CircleCollider2D>();

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeATState();
        currentHP = enemySO.hp;
        isLive = true;
    }
    void Update()
    {
        //AIƮ���� ��� ���¸� �� ������ ���� ����
        TreeAIState.Tick();
        View();
    }


    //��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("�Ѿ� �¾���");

            isChase = true;
            DecreaseHP(collision.transform.GetComponent<Bullet>().ATK);  
            
            //TODO������ �̹����� hp��ġ ����
        }
    }

    //��
    public void IncreaseHP(float damage)
    {
        currentHP += damage;

        if (currentHP > enemySO.hp)
            currentHP = enemySO.hp;
    }

    //��
    public void DecreaseHP(float damage)
    {
        //
        currentHP -= damage;    


        if (currentHP <= 0)
            isLive = false;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject, 1f);
    }

    public void Shoot()
    {
        Instantiate(enemyBullet, enemyAim.transform.position, enemyAim.transform.rotation);
    }

    private Vector2 BoundaryAngle(float angle)
    {
        // ���� ������Ʈ�� ȸ������ ����Ͽ� ���� ���͸� ���

        // ���� ������Ʈ�� ȸ���� + ���� ������ => �� ���� �������� ��ȯ
        float radAngle = (transform.eulerAngles.z + angle) * Mathf.Deg2Rad;

        // ���͸� radAngle���� x,y �������� ����Ͽ� 2D ���ͷ� ��ȯ
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }


    //�÷��̾� Ž����
    private void View()
    {
        Vector2 rightBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector2 leftBoundary = BoundaryAngle(viewAngle * 0.5f);

        // ��������Ʈ �������� flipX �����̸� ���� ������ �ݴ�� ����
        if (spriteRenderer.flipX)
        {
            rightBoundary = -rightBoundary;
            leftBoundary = -leftBoundary;
        }

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.yellow);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.green);



        //�þ� �Ÿ�(viewDistance) ���� targetMask ����
        Collider2D _target = Physics2D.OverlapCircle(transform.position, viewDistance, targetMask);
        target = _target.gameObject;

        if (_target == null)
            return;



        if (_target.tag == "Player")
        {

            //�þ߰� ������ ���� Direction
            Vector2 middleDirection = (rightBoundary + leftBoundary).normalized;

            //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);

            //Enemy�� Player ������ ����
            Vector2 directionToPlayer = (_target.transform.position - transform.position).normalized;

            //�÷��̾� �þ� �߾�~Ÿ����ġ ������ ����
            float angle = Vector3.Angle(directionToPlayer, middleDirection);

            if (angle < viewAngle * 0.5f)
            {
                isChase = true;

                Debug.Log("�þ� ���� ����");
                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
                _target = null;
            }
        }
    }


    //�ൿ Ʈ�� ���� ����
    void CreateTreeATState()
    {
        //�ʱ�ȭ&��Ʈ ���� ����
        TreeAIState = new BTRoot();

        //BTSelector�� BTSquence ���� : Ʈ�� ���� ����
        BTSelector BTMainSelector = new BTSelector();



        //Enemy ���� üũ
        //����� üũ -> ��� �� �ʿ��� �׼ǵ�(������Ʈ ����....)
        BTSquence BTDead = new BTSquence();
        EnemyState_Dead_DeadCondition deadCondition = new EnemyState_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        EnemyState_Dead state_Dead = new EnemyState_Dead(gameObject);
        BTDead.AddChild(state_Dead);


        //����+����
        //����� üũ -> �÷��̾� ���� & �÷��̾ ���� ���� �� -> ����(���� ��ȯ �� ���ʷ�)
        BTSquence BTChase = new BTSquence();

        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase state_Chase = new EnemyState_Chase(gameObject);
        BTChase.AddChild (state_Chase);
        EnemyState_Attack state_Attack = new EnemyState_Attack(gameObject);
        BTChase.AddChild(state_Attack);


        


        //����(������ : �ϳ��� �����ϸ� ���й�ȯ)
        //�Ұ� ������ �̵�
        BTSquence BTPatrol = new BTSquence();

        EnemyState_Patrol state_Patrol = new EnemyState_Patrol(gameObject);
        BTPatrol.AddChild(state_Patrol);


        //���(��� üũ)

        //�����ʹ� �켱���� ���� ������ ��ġ : ���� ���� -> Ư�� ���� -> �÷��̾� üũ(���� ����) -> �̵� ���� ������ ������ ��ġ 
        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���) 

        BTMainSelector.AddChild(BTDead);
        BTMainSelector.AddChild(BTChase);
        BTMainSelector.AddChild(BTPatrol);

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }
}
