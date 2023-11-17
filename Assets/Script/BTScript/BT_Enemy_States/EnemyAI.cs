using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using Photon.Pun;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using TMPro;
using Unity.Mathematics;

//[Root ���] => �� �׼ǰ� �ٸ��� ��� �ȹ���?
//==>Ư�� AI ���۰� ���¿� �°� �����ϰ� �����ϱ� ���ؼ�
//��� BTRoot ��ü�� �����ϰ�, �� �Ʒ��� ���� ���� �׼� ��带 �߰��ؼ� Ʈ���� ������
//EnemyAI : AI�� ���¿� ������ ������ �����ϰ�, ������


//Enemy�� �ʿ��� ������Ʈ�� + ��Ÿ ��ҵ� ���⿡ �� �߰�
public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // ���� ü�� ���
    public float viewAngle;                  // �þ߰� (�⺻120��)
    public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO enemySO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D collider2D;
    public Animator anim;

    private Transform target;
    public Transform Target { get { return target; } 
                               set { if (target != value) { OnTargetChaged(value); target = value; } } }//���� Ÿ��[Palyer]
    //public Collider2D target;
    public NavMeshAgent nav;
    public Vector3 navTargetPoint;              //nav ������
    public List<Transform> PlayersTransform;

    public GameObject enemyAim;
    public Bullet enemyBulletPrefab;


    public LayerMask targetMask;             // Ÿ�� ���̾�(Player)

    public float SpeedCoefficient = 1f;      // �̵��ӵ� ���
   
    public bool isLive;
    public bool isChase;
    public bool isAttaking;

    //�÷��̾� ����

    int lastAttackPlayer;

    //���ӸŴ�������(����) �����ϴ� �÷��̾�� ������ ��û�ؼ� ���

    //���帹�� ���ظ� �� �÷��̾� Ÿ��-> �ҷ�(���� ����) ������ �˸�� ->�÷��� ���ݷ�->

    Vector2 nowEnemyPosition;
    Quaternion nowEnemyRotation;
    [SerializeField]
    private Image images_Gauge;              //���� UI : Status


    //����ȭ
    public PhotonView PV;                    
    private Vector3 hostPosition;
    public float lerpSpeed = 10f; // ������ �ʿ��� ��ġ(���� �ʿ�)


    //�˹�
    public bool isKnockback = false;

    //�˹� ���� ���� & �˹��� �ִ� Ÿ��
    public Vector2 knockbackStartPosition;
    public Vector2 knockbackTargetPosition;

    //�˹� ���۽ð� & �˹� ���� �ð�
    public float knockbackStartTime;
    public float knockbackDuration = 0.2f;

    public float ViewDistanceThreshold = 0.2f; 
    public float KnockbackLimitTime = 0.3f;


    //��ü�� �˹�Ÿ�
    public float knockbackDistance;


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        PV = GetComponent<PhotonView>();

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeAIState();
        currentHP = enemySO.hp;
        viewAngle = enemySO.viewAngle;
        viewDistance = enemySO.viewDistance;
        isLive = true;

        nav.updateRotation = false;
        nav.updateUpAxis = false;

        //�ڽ̱� �׽�Ʈ �� if else �ּ�ó�� �Ұ�
        //�Ѵ� �÷��̾ ȣ��Ʈ�� �Ǻ�?

        nowEnemyPosition = this.gameObject.transform.position;
        knockbackDistance = 0f;

        nav.speed = enemySO.enemyMoveSpeed;
        navTargetPoint = transform.position;


        //ȣ��Ʈ�� navȰ��ȭ �ϵ��� ����
        if (!PhotonNetwork.IsMasterClient)
            nav.enabled = false;
        else
            nav.enabled = true;

        //������ ��, ��� �÷��̾� Transform ������ ��´�.
        foreach (var _value in TestGameManager.Instance.playerInfoDictionary.Values)
        {
            PlayersTransform.Add(_value);
        }
    }
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //$�߰��� : ����ȭ�� ��ġ�� ���� ���� ó��
            transform.position = Vector3.Lerp(transform.position, hostPosition, Time.deltaTime * lerpSpeed);
            return;
        }

        hostPosition = transform.position;
        

        //AIƮ���� ��� ���¸� �� ������ ���� ����
        TreeAIState.Tick();

        if (!isLive)
            return;

        IsNavAbled();

        if (isAttaking || isChase)
        {
           //PV.RPC("Filp", RpcTarget.All);
           ChaseView();
        }           
        else
        {
           NomalView();
        }


        // �˹� ���� ���
        if (isKnockback)
        {
            HandleKnockback();
        }



        //�������� �� �Ÿ��� �����Ÿ� ���ϰų� / nav�� ���� ����(�׳� ����) �� �ƴѰ��
        if (!IsNavAbled() || nav.remainingDistance < 0.2f)
        {          
            SetAnim("isWalk", false);
            SetAnim("isUpWalk", false);
            return;
        }

        UpdateAnimation();
    }


    //Enemy �̵� �ӵ����� ����
    public void ChangeSpeed(float statSpeed)
    {
        nav.speed = statSpeed * SpeedCoefficient;
    }

    #region Enemy �ǰ�, ���, �˹�, ���� ����
    //�ڸ��� & ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ȣ��Ʈ������ �浹 ó����
        if (!PhotonNetwork.IsMasterClient)
            return;

        Bullet playerBullet = collision.gameObject.GetComponent<Bullet>();


        if (collision.gameObject.tag == "Bullet" && playerBullet.target == BulletTarget.Enemy && playerBullet.IsDamage)
        {
            float atk = collision.transform.GetComponent<Bullet>().ATK;
            isChase = true;
            int ViewID = playerBullet.BulletOwner;
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire) 
            {
                Debuff.GiveFire(this.gameObject, atk);
            }
            if (playerBullet.water)
            {
                Debuff.GiveIce(this.gameObject);
            }
            if (playerBullet.burn)
            {
                PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition,quaternion.identity);
            }
            if (playerBullet.gravity)
            {
                int a = UnityEngine.Random.Range(0, 10);
                if (a >= 8) 
                {
                    PhotonNetwork.Instantiate("AugmentList/A0218", transform.localPosition, quaternion.identity);
                }
            }
            //��� �÷��̾�� ���� ���� ü�� ����ȭ
            PV.RPC("DecreaseHP", RpcTarget.All, atk);

            float BulletknockbackDistance = 2.0f;


            //����� �ҷ� ��ò��� ���
            lastAttackPlayer = playerBullet.BulletOwner;

            // ��ID�� ����Ͽ� ���� �÷��̾� ã��&�ش� �÷��̾�� Ÿ�� ����
            PhotonView photonView = PhotonView.Find(playerBullet.BulletOwner);
            if (photonView != null)
            {
                Transform playerTransform = photonView.transform;

                Target = playerTransform;
            }

            //�˹�(�浹 ����&Enemy ���� ����ȭ)
            Vector2 directionToBullet = (collision.transform.position - transform.position).normalized;

            // �˹� ���� ��ġ�� ��ǥ ��ġ ���
            knockbackStartPosition = transform.position;
            knockbackTargetPosition = knockbackStartPosition - directionToBullet * BulletknockbackDistance;

            // �˹� ���� �ð� ����
            knockbackStartTime = Time.time;

            // ������Ʈ �˹� ����
            isKnockback = true;
            if (!playerBullet.Penetrate) 
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (knockbackDistance == 0 || collision.gameObject.tag != "player")
            return;

        //TODO : ��� ���� - 0.15f
        PV.RPC("DecreaseHP", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * 0.15f);

        Transform PlayersTransform = collision.gameObject.transform;

        //�˹�(�浹 ����&Enemy ���� ����ȭ)
        Vector2 directionToPlayer = (collision.transform.position - transform.position).normalized;

        // �˹� ���� ��ġ�� ��ǥ ��ġ ���
        knockbackStartPosition = transform.position;
        knockbackTargetPosition = knockbackStartPosition - directionToPlayer;

        // �˹� ���� �ð� ����
        knockbackStartTime = Time.time;

        // ������Ʈ �˹� ����
        isKnockback = true;
    }
    private void HandleKnockback()
    {
        //�˹� ���ӽð�
        float knockbackRatio = (Time.time - knockbackStartTime) / knockbackDuration;
        transform.position = Vector2.Lerp(knockbackStartPosition, knockbackTargetPosition, knockbackRatio);

        if (knockbackRatio >= KnockbackLimitTime)
        {
            isKnockback = false;
        }
    }
    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / enemySO.hp; //ü��
    }

    [PunRPC]
    public void IncreaseHP(float damage)
    {
        currentHP += damage;

        if (currentHP > enemySO.hp)
            currentHP = enemySO.hp;

        GaugeUpdate();
    }

    [PunRPC]
    public void DecreaseHP(float damage,int playerid)
    {
        SetStateColor();
        currentHP -= damage;
        GaugeUpdate();
        if (currentHP <= 0)
        {
            //�÷��̾��� �� ���̵� �������
            //lastAttackPlayer
            isLive = false;
        }
    }

    [PunRPC]
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    [PunRPC]
    public void Fire()
    {
        var _bullet = Instantiate(enemyBulletPrefab, enemyAim.transform.position, enemyAim.transform.rotation);

        _bullet.IsDamage = true;
        _bullet.ATK = enemySO.atk;
        _bullet.BulletLifeTime = enemySO.bulletLifeTime;
        _bullet.BulletSpeed = enemySO.bulletSpeed;
        _bullet.target = BulletTarget.Player;

        /*
        //���� : gameObject ���� Bullet���� ->���� ���¿� �뵵�� ������
        Bullet _bullet = Instantiate<Bullet>(enemyBulletPrefab, enemyAim.transform.position, enemyAim.transform.rotation);



        _bullet.IsDamage = true;
        _bullet.ATK = enemySO.atk;
        _bullet.BulletLifeTime = enemySO.bulletLifeTime;
        _bullet.BulletSpeed = enemySO.bulletSpeed;
        _bullet.target = BulletTarget.Player;
        */

        //���� : gameObject ���� Bullet���� ->���� ���¿� �뵵�� ������            
    }
    #endregion

    #region �þ߰�(Ÿ�� ��ġ) ����
    private Vector2 BoundaryAngle(float angle)
    {
        // ���� ������Ʈ�� ȸ������ ����Ͽ� ���� ���͸� ���

        // ���� ������Ʈ�� ȸ���� + ���� ������ => �� ���� �������� ��ȯ
        float radAngle = (transform.eulerAngles.z + angle) * Mathf.Deg2Rad;

        // ���͸� radAngle���� x,y �������� ����Ͽ� 2D ���ͷ� ��ȯ
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }


    private void NomalView()
    {
        Vector2 rightBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector2 leftBoundary = BoundaryAngle(viewAngle * 0.5f);

        // ��������Ʈ ������ flipX ���¿� ���� ���� ������ �ݴ�� ����
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
        if (Target == null)
        {           
            isAttaking = false;
            isChase = false;
            return;
        }


        Vector2 directionToTarget = (Target.position - transform.position).normalized;


        Vector2 rightBoundary = Quaternion.Euler(0, 0,-viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.black);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.black);

        FindPlayer(rightBoundary, leftBoundary);
    }


    private void FindPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (!PhotonNetwork.IsMasterClient || Target != null || isChase)
            return;
        //viewDistance > Vector2.Distance(playerTransform.position, transform.position


        //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);


        //�޾ƿ� ��� �÷��̾� Ʈ�������� �޾ƿ´�.
        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (viewDistance >= Vector2.Distance(PlayersTransform[i].position, transform.position))
            {
                //�þ߰� ������ ���� Direction
                Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;

                //Enemy�� Player ������ ����
                Vector2 directionToPlayer = (PlayersTransform[i].position - transform.position).normalized;

                float angle = Vector3.Angle(directionToPlayer, middleDirection);
                if (angle < viewAngle * 0.5f)
                {
                    isChase = true;
                    Target = PlayersTransform[i]; // �þ߰� �ȿ� �ִ� �÷��̾�� currentTargetPlayer ����
                    Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);

                    Debug.Log($"Ÿ�� ����{Target}");
                    break;
                }
            }

        }
    }
#endregion

    #region Ÿ��(Player) ���� 
    private void OnTargetChaged(Transform _target)
    {
        //������ Ŭ���̾�Ʈ�� ���͸� ��ȯ�ϰ�, �ش� ���͵���
        if(PhotonNetwork.IsMasterClient)
        {
            if (_target == null)
                photonView.RPC("SendTargetNull", RpcTarget.Others);
            else
            {
                int viewID = _target.gameObject.GetPhotonView().ViewID; //���ϴ� viewID
                photonView.RPC("SendTarget", RpcTarget.Others, viewID);
            }            
        }
    }

   
    [PunRPC]
    private void SendTarget(int viewID)
    {
        PhotonView targetPV = PhotonView.Find(viewID);
        Target = targetPV.transform;
    }

    [PunRPC]
    private void SendTargetNull()
    {
        Target = null;
    }


    private void SetStateColor()
    {
        spriteRenderer.color = Color.red;
    }
    #endregion

    #region player�ִϸ��̼� ����    

    private void UpdateAnimation()
    {
        if (navTargetPoint.y > transform.position.y)
        {
            SetAnim("isUpWalk", true);
            SetAnim("isWalk", false);
        }
        else
        {
            SetAnim("isWalk", true);
            SetAnim("isUpWalk", false);
        }
    }
    private void SetAnim(string animName, bool set)
    {
        if (PV.IsMine == false)
            return;

        //���� ����
        bool prev = anim.GetBool(animName);

        if (prev == set)
            return;

        anim.SetBool(animName, set);
        PV.RPC(nameof(SyncAnimation), RpcTarget.All, animName, set);
    }

    [PunRPC]
    public void SyncAnimation(string animName, bool set)
    {
        Debug.Log($"{animName}�� {set} ���·� ȣ���");
        anim.SetBool(animName, set);
    }

    #endregion

    #region NavAgent ����    
    public void DestinationSet()
    {
        if (!isAttaking || isLive)
        {
            nav.SetDestination(navTargetPoint);
        }

        if (navTargetPoint.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (navTargetPoint.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
            return;
            
        //MoveToHostPosition();
    }

    public bool IsNavAbled()
    {
        if (isAttaking || !isLive)
        {
            nav.isStopped = true;
            return false;
        }            
        else
        {
            nav.isStopped = false; // Ȱ��ȭ
            return true;
        }           
    }
    #endregion

    #region BehaviourTree ���� 
    void CreateTreeAIState()
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

        EnemyState_Attack_AttackCondition attackCondition = new EnemyState_Attack_AttackCondition(gameObject);
        BTChase.AddChild(attackCondition);
        EnemyState_Attack state_Attack = new EnemyState_Attack(gameObject);
        BTChase.AddChild(state_Attack);


        


        //����(������ : �ϳ��� �����ϸ� ���й�ȯ)
        //�Ұ� ������ �̵�
        BTSquence BTPatrol = new BTSquence();

        EnemyState_Patrol state_Patrol = new EnemyState_Patrol(gameObject);
        BTPatrol.AddChild(state_Patrol);



        //�����ʹ� �켱���� ���� ������ ��ġ : ���� ���� -> Ư�� ���� -> �÷��̾� üũ(���� ����) -> �̵� ���� ������ ������ ��ġ 
        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���) 

        BTMainSelector.AddChild(BTDead);
        BTMainSelector.AddChild(BTChase);
        BTMainSelector.AddChild(BTPatrol);

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }


    private void MoveToHostPosition()
    {
        // ���� ��ġ���� ��Ʈ��ũ�κ��� ���� ��ǥ ��ġ�� �ε巴�� �̵�
        transform.position = Vector3.Lerp(transform.position, navTargetPoint, Time.deltaTime * lerpSpeed);
    }
    #endregion

    //����ȭ ���� 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� ����
            stream.SendNext(hostPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(spriteRenderer.flipX); // �̰� �³�?
            stream.SendNext(enemyAim.transform.rotation);

        }
        else if (stream.IsReading)
        {
            // �����͸� ����
            hostPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            spriteRenderer.flipX = (bool)stream.ReceiveNext();
            enemyAim.transform.rotation = (Quaternion)stream.ReceiveNext();
        }   
    }
}
