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
using Unity.VisualScripting;
using UnityEngine.U2D.Animation;

//[Root ���] => �� �׼ǰ� �ٸ��� ��� �ȹ���?
//==>Ư�� AI ���۰� ���¿� �°� �����ϰ� �����ϱ� ���ؼ�
//��� BTRoot ��ü�� �����ϰ�, �� �Ʒ��� ���� ���� �׼� ��带 �߰��ؼ� Ʈ���� ������
//EnemyAI : AI�� ���¿� ������ ������ �����ϰ�, ������

//Enemy�� �ʿ��� ������Ʈ�� + ��Ÿ ��ҵ� ���⿡ �� �߰�

enum EnemyStateColor
{
    ColorRed,
    ColorYellow,
    ColorBlue,
    ColorBlack,
    ColorOrigin,
    ColorMagenta,
}


public class BT_Named_MiniDragon : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public bool CanFire;
    public bool CanWater;
    public bool CanIce;

    public float currentHP;                  // ���� ü�� ���
    private float viewAngle;                  // �þ߰� (�⺻120��)
    private float viewDistance;               // �þ� �Ÿ� (�⺻ 10)

    public int roomNum;                    // ���� ����(Ŭ���� ������ ���� ���� -����-)

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO enemySO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Color originColor;

    private Transform target;
    public Transform Target
    {
        get { return target; }
        set { if (target != value) { OnTargetChaged(value); target = value; } }
    }//���� Ÿ��[Palyer]
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
    public bool isGroggy;

    //�÷��̾� ����

    public int lastAttackPlayer;

    //���ӸŴ�������(����) �����ϴ� �÷��̾�� ������ ��û�ؼ� ���

    //���帹�� ���ظ� �� �÷��̾� Ÿ��-> �ҷ�(���� ����) ������ �˸�� ->�÷��� ���ݷ�->

    Vector2 nowEnemyPosition;
    Quaternion nowEnemyRotation;
    [SerializeField]
    private Image images_Gauge;              //���� UI : Status
    private SpriteLibrary spriteLibrary;     //��������Ʈ ���̺귯��(������ ��ȯ��)


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
    public float knockbackDuration = 0.3f;

    public float ViewDistanceThreshold = 0.2f;
    public float KnockbackLimitTime = 0.2f;



    //��ü�� �˹�Ÿ�
    public float knockbackDistance;

    public float GroggyCount = 100f;
    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        spriteLibrary = GetComponentInChildren<SpriteLibrary>();

        spriteLibrary.spriteLibraryAsset = enemySO.enemySpriteLibrary;
        enemyBulletPrefab = enemySO.enemyBulletPrefab;

        /*
        if (enemySO.type == EnemyType.Melee)
        {
            enemyBulletPrefab = Resources.Load<Bullet>(Enemy_PrefabPathes.BOSS_TURTLE_MELEE_ENEMY_BULLET);
        }

        if (enemySO.type == EnemyType.Ranged)
        {
            enemyBulletPrefab = Resources.Load<Bullet>(Enemy_PrefabPathes.BOSS_TURTLE_RANGED_ENEMY_BULLET);
        }
        */


        CanWater = true;
        CanFire = true;
        CanIce = true;

        originColor = spriteRenderer.color;
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

        if (TestGameManager.Instance != null)
        {
            //������ ��, ��� �÷��̾� Transform ������ ��´�.
            foreach (var _value in TestGameManager.Instance.playerInfoDictionary.Values)
            {
                PlayersTransform.Add(_value);
            }
        }
        else
        {
            //������ ��, ��� �÷��̾� Transform ������ ��´�.
            foreach (var _value in GameManager.Instance.playerInfoDictionary.Values)
            {
                PlayersTransform.Add(_value);
            }
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


        if (collision.gameObject.tag == "Bullet" && playerBullet.targets.ContainsValue((int)BulletTarget.Enemy) && playerBullet.IsDamage)
        {
            float atk = collision.transform.GetComponent<Bullet>().ATK;
            isChase = true;
            int ViewID = playerBullet.BulletOwner;
            //Debug.Log($"����̵� : {ViewID}");
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire)
            {
                Debuff.Instance.GiveFire(this.gameObject, atk, ViewID);
            }
            if (playerBullet.water)
            {
                Debuff.Instance.GiveWater(this.gameObject);
            }
            if (playerBullet.ice)
            {
                int random = UnityEngine.Random.Range(0, 100);
                if (random < 90)
                {
                    isGroggy = true;
                    Debug.Log("����üũ");
                    Debuff.Instance.GiveIce(this.gameObject);
                }
            }
            if (playerBullet.burn)
            {
                GameObject firezone = PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition, quaternion.identity);
                firezone.GetComponent<A0122_1>().Init(playerBullet.BulletOwner, atk);
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
        if ((knockbackDistance == 0 || collision.gameObject.tag != "player")
            && collision.gameObject.GetComponent<A0126>() == null
            && collision.gameObject.GetComponent<A3104>() == null)
            return;

        Transform PlayersTransform = collision.gameObject.transform;

        //�˹�(�浹 ����&Enemy ���� ����ȭ)
        Vector2 directionToPlayer = (collision.transform.position - transform.position).normalized * knockbackDistance;

        // �˹� ���� ��ġ�� ��ǥ ��ġ ���
        knockbackStartPosition = transform.position;
        knockbackTargetPosition = knockbackStartPosition - directionToPlayer;

        // �˹� ���� �ð� ����
        knockbackStartTime = Time.time;

        // ������Ʈ �˹� ����
        isKnockback = true;

        // ��� ����
        float damageCoeff = 0;

        if (collision.gameObject.GetComponent<A0126>() != null)
        {
            damageCoeff += collision.gameObject.GetComponent<A0126>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
        if (collision.gameObject.GetComponent<A3104>().isRoll)
        {
            damageCoeff += collision.gameObject.GetComponent<A3104>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
        knockbackDistance = 0f;
    }
    private void HandleKnockback()
    {
        //�˹� ���ӽð�
        float knockbackRatio = (Time.time - knockbackStartTime) / knockbackDuration;
        transform.position = Vector2.Lerp(knockbackStartPosition, knockbackTargetPosition, knockbackRatio);

        if (!PhotonNetwork.IsMasterClient)
        {
            //$�߰��� : ����ȭ�� ��ġ�� ���� ���� ó��
            transform.position = Vector2.Lerp(hostPosition, knockbackTargetPosition, Time.deltaTime * lerpSpeed);
            return;
        }

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
    public void DecreaseHP(float damage)
    {
        if (!isLive)
        {
            return;
        }
        PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorRed, PV.ViewID);
        currentHP -= damage;
        GaugeUpdate();
        if (currentHP <= 0)
        {
            isLive = false;
        }
    }


    //��Ȯ��
    [PunRPC]
    public void DecreaseHPByObject(float damage, int viewID)
    {
        if (!isLive)
        {
            return;
        }
        PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorRed, PV.ViewID);
        currentHP -= damage;
        GaugeUpdate();
        if (currentHP <= 0)
        {
            lastAttackPlayer = viewID;
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
        _bullet.targets["Player"] = (int)BulletTarget.Player;
        _bullet.BulletOwner = photonView.ViewID;

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

    //�����̻�
    [PunRPC]
    public void Groggy()
    {
        //�ൿ ����(�̰� �Ѿ� �´� �κп� ������)
        isGroggy = true;
        nav.isStopped = false;

        //����ȭ �ؾ��� �κ�
        //������ ����animSet�̳� ��Ÿ ��ƼŬ, ȿ�� ���...
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


        Vector2 rightBoundary = Quaternion.Euler(0, 0, -viewAngle * 0.5f) * directionToTarget;
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
            if (viewDistance >= Vector2.Distance(PlayersTransform[i].position, transform.position) &&
                PlayersTransform[i].gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                //�þ߰� ������ ���� Direction
                Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;
                //Enemy�� Player ������ ����
                Vector2 directionToPlayer = (PlayersTransform[i].position - transform.position).normalized;

                float angle = Vector3.Angle(directionToPlayer, middleDirection);
                if (angle < viewAngle * 0.5f)
                {
                    isChase = true;
                    Target = PlayersTransform[i];  // �þ߰� �ȿ� �ִ� �÷��̾�� currentTargetPlayer ����
                    Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
                    Debug.Log($"Ÿ�� ����{Target}");
                    break;
                }
                else
                    Target = null;
            }

        }
    }
    #endregion

    #region Ÿ��(Player) ���� 
    private void OnTargetChaged(Transform _target)
    {
        //������ Ŭ���̾�Ʈ�� ���͸� ��ȯ�ϰ�, �ش� ���͵���

        if (PhotonNetwork.IsMasterClient)
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

    //1.�̳����� Į�� ������ �ϰ� �ش� �̳ѿ� ���� ��Ʈ ������
    //2.�÷� 4�� ��Ҹ� �� ������
    [PunRPC]
    private void SetStateColor(int colorNum, int viewID)
    {
        //TODO
        PV = PhotonView.Find(viewID);
        PV.GetComponent<BT_Named_MiniDragon>().SetColor(colorNum);
    }

    private void SetColor(int colorNum)
    {
        switch (colorNum)
        {
            case (int)EnemyStateColor.ColorRed:
                spriteRenderer.color = Color.red;
                //Debug.Log($"���� ��������Ʈ ����{spriteRenderer.color}");
                break;
            case (int)EnemyStateColor.ColorYellow:
                spriteRenderer.color = Color.yellow;
                break;
            case (int)EnemyStateColor.ColorBlue:
                spriteRenderer.color = Color.blue;
                break;
            case (int)EnemyStateColor.ColorBlack:
                spriteRenderer.color = Color.black;
                break;
            case (int)EnemyStateColor.ColorOrigin:
                spriteRenderer.color = originColor;
                break;
            case (int)EnemyStateColor.ColorMagenta:
                spriteRenderer.color = Color.magenta;
                break;
        }
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
        //Debug.Log($"{animName}�� {set} ���·� ȣ���");
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
        if (isAttaking || !isLive || isGroggy)
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

        //�����̻� üũ [����....]
        BTSquence BTAbnormal = new BTSquence();
        EnemyState_GroggyCondition groggyConditon = new EnemyState_GroggyCondition(gameObject);
        BTAbnormal.AddChild(groggyConditon);



        //����+����
        //����� üũ -> �÷��̾� ���� & �÷��̾ ���� ���� �� -> ����(���� ��ȯ �� ���ʷ�)
        BTSquence BTChase = new BTSquence();

        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase state_Chase = new EnemyState_Chase(gameObject);
        BTChase.AddChild(state_Chase);

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
        BTMainSelector.AddChild(BTAbnormal);
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
            stream.SendNext(spriteRenderer.flipX);
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

    public override void OnDisable()
    {
        base.OnDisable();
        //Debug.Log("���� ������");
        PV.RPC("DeadSync", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void DeadSync()
    {
        GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].roomInMoster--;
        if (GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].roomInMoster == 0)
        {
            GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].thisRoomClear = true;
            //GameManager.Instance.CallRoomEndEvent();
            GameManager.Instance.PV.RPC("CallRoomEndEvent", RpcTarget.MasterClient);
        }
    }
}

