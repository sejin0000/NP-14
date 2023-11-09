using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using static UnityEditor.PlayerSettings;
using UnityEditor.Rendering.LookDev;
using Photon.Pun;
using UnityEngine.UI;

//[Root ���] => �� �׼ǰ� �ٸ��� ��� �ȹ���?
//==>Ư�� AI ���۰� ���¿� �°� �����ϰ� �����ϱ� ���ؼ�
//��� BTRoot ��ü�� �����ϰ�, �� �Ʒ��� ���� ���� �׼� ��带 �߰��ؼ� Ʈ���� ������
//EnemyAI : AI�� ���¿� ������ ������ �����ϰ�, ������


//Enemy�� �ʿ��� ������Ʈ�� + ��Ÿ ��ҵ� ���⿡ �� �߰�
public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // ���� ü�� ���
     

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO enemySO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D collider2D;
    public Animator anim;  
    public GameObject target;                //���� Ÿ��[Palyer]
    public Collider2D targetColl;
    public NavMeshAgent nav;

    public GameObject enemyAim;
    public GameObject enemyBullet;

    public float viewAngle;                  // �þ߰� (�⺻120��)
    public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)
    public LayerMask targetMask;             // Ÿ�� ���̾�(Player)

    public bool isLive;
    public bool isChase;
    public bool isAttaking;

    
    [SerializeField]
    private Image images_Gauge;              //���� UI : Status

    
    public PhotonView PV;                    //����ȭ


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponentInChildren<CircleCollider2D>();
        PV = GetComponent<PhotonView>();

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeATState();
        currentHP = enemySO.hp;
        isLive = true;

        nav.updateRotation = false;
        nav.updateUpAxis = false;

        //�ڽ̱� �׽�Ʈ �� if else �ּ�ó�� �Ұ�
        
        if (photonView.AmOwner)
        {
            nav.enabled = true;
        }
        else
        {
            nav.enabled = false;
        }
        
    }
    void Update()
    {
        //AIƮ���� ��� ���¸� �� ������ ���� ����
        TreeAIState.Tick();       
        GaugeUpdate();


        if (photonView.AmOwner)
        {
            IsNavAbled();


            if (isAttaking || isChase)
                ChaseView();
            else
                NomalView();
        }        
    }


    //��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ȣ��Ʈ������ �浹 ó����
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (collision.gameObject.tag == "Bullet")
        {
            isChase = true;

            //��� �÷��̾�� ���� ���� ü�� ����ȭ
            PV.RPC("DecreaseHP", RpcTarget.AllBuffered, collision.transform.GetComponent<Bullet>().ATK);

            Debug.Log("���� ü�� :" + currentHP);
            //TODO������ �̹����� hp��ġ ����
        }
    }



    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / enemySO.hp; //ü��
    }

        //��
        public void IncreaseHP(float damage)
    {
        currentHP += damage;

        if (currentHP > enemySO.hp)
            currentHP = enemySO.hp;
    }

    [PunRPC]
    public void DecreaseHP(float damage)
    {
        //
        SetStateColor();
        currentHP -= damage;    


        if (currentHP <= 0)
            isLive = false;
    }

    [PunRPC]
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


    //�÷��̾� Ž���� ���⼭ ������&���ݽ� �þ߰��� ��������
    private void NomalView()
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
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.yellow);


        FindPlayer(rightBoundary, leftBoundary);
    }


    //����, ���ݽ� �÷��̾ �ٶ󺸴� �þ߰����� ��ȯ
    private void ChaseView()
    {
        Vector2 directionToTarget = (target.transform.position - transform.position).normalized;


        Vector2 rightBoundary = Quaternion.Euler(0, 0,-viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.black);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.black);

        FindPlayer(rightBoundary, leftBoundary);
    }

    private void FindPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        targetColl = Physics2D.OverlapCircle(transform.position, viewDistance, targetMask);


        if (targetColl == null)
            return;

        target = targetColl.gameObject;

        if (targetColl.tag == "Player")
        {

            //�þ߰� ������ ���� Direction
            Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;

            //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);

            //Enemy�� Player ������ ����
            Vector2 directionToPlayer = (targetColl.transform.position - transform.position).normalized;

            //�÷��̾� �þ� �߾�~Ÿ����ġ ������ ����
            float angle = Vector3.Angle(directionToPlayer, middleDirection);

            if (angle < viewAngle * 0.5f)
            {
                isChase = true;

                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
            }
        }
    }


    private void SetStateColor()
    {
        spriteRenderer.color = Color.red;
    }

    public void isFilp(float myX, float otherX)
    {
        if (otherX < myX)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void DestinationSet(Vector3 targetPoint)
    {
        if (!isAttaking || isLive)
        {
            nav.SetDestination(targetPoint);
        }
    }

    public void IsNavAbled()
    {
        if (isAttaking || !isLive)
            nav.isStopped = true;
        else
            nav.isStopped = false; // Ȱ��ȭ
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

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� ����
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            Debug.Log("��ġ ������ ����");
        }
        else if (stream.IsReading)
        {
            // �����͸� ����
            transform.position = (Vector2)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
