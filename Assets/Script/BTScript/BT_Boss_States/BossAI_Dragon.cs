using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossAI_Dragon : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // ���� ü�� ���
    public float viewAngle;                  // �þ߰� (�⺻120��)
    public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO bossSO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderer;
    public Animator anim;


    private Transform target;
    public Transform currentTarget
    {
        get { return target; }
        set { if (target != value) { OnTargetChaged(value); target = value; } }
    }
    //public Collider2D target;
    public List<Transform> PlayersTransform;

    public Bullet enemyBulletPrefab;
    public Transform bossHead;
    public Transform bossHeadPivot;

    public LayerMask targetMask;             // Ÿ�� ���̾�(Player)

    public float SpeedCoefficient = 1f;      // �̵��ӵ� ���


    public bool isLive;
    public bool isAttaking;
    public bool isGroggy;

    //�÷��̾� ����

    public int lastAttackPlayer;

    //���ӸŴ�������(����) �����ϴ� �÷��̾�� ������ ��û�ؼ� ���

    //���帹�� ���ظ� �� �÷��̾� Ÿ��-> �ҷ�(���� ����) ������ �˸�� ->�÷��� ���ݷ�->



    [SerializeField]
    private Image images_Gauge;              //���� UI : Status



    //����ȭ
    public PhotonView PV;
    private Vector3 hostPosition;
    public float lerpSpeed = 10f; // ������ �ʿ��� ��ġ(���� �ʿ�)




    //��ü�� �˹�Ÿ�
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeAIState();
        currentHP = bossSO.hp;
        viewAngle = bossSO.viewAngle;
        viewDistance = bossSO.viewDistance;
        isLive = true;


        //�ڽ̱� �׽�Ʈ �� if else �ּ�ó�� �Ұ�
        //�Ѵ� �÷��̾ ȣ��Ʈ�� �Ǻ�?


        knockbackDistance = 0f;



        //������ ��, ��� �÷��̾� Transform ������ ��´�.
        foreach (var _value in TestGameManagerWooMin.Instance.playerInfoDictionary.Values)
        {
            PlayersTransform.Add(_value);
        }

        //���� �� ���� Ÿ�� ����
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget]; 
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





        /*
        //�������� �� �Ÿ��� �����Ÿ� ���ϰų� / nav�� ���� ����(�׳� ����) �� �ƴѰ��
        if (!IsNavAbled())
        {
            SetAnim("isWalk", false);
            SetAnim("isUpWalk", false);
            return;
        }

        UpdateAnimation();
        */
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
            int ViewID = playerBullet.BulletOwner;
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire)
            {
                Debuff.Instance.GiveFire(this.gameObject, atk, ViewID);
            }
            if (playerBullet.water)
            {
                Debuff.Instance.GiveIce(this.gameObject);
            }
            if (playerBullet.burn)
            {
                PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition, Quaternion.identity);
            }
            if (playerBullet.gravity)
            {
                int a = UnityEngine.Random.Range(0, 10);
                if (a >= 8)
                {
                    PhotonNetwork.Instantiate("AugmentList/A0218", transform.localPosition, Quaternion.identity);
                }
            }
            //��� �÷��̾�� ���� ���� ü�� ����ȭ
            PV.RPC("DecreaseHP", RpcTarget.All, atk);



            //����� �ҷ� ��ò��� ���
            lastAttackPlayer = playerBullet.BulletOwner;

            // ��ID�� ����Ͽ� ���� �÷��̾� ã��&�ش� �÷��̾�� Ÿ�� ����
            PhotonView photonView = PhotonView.Find(playerBullet.BulletOwner);
            if (photonView != null)
            {
                Transform playerTransform = photonView.transform;

                currentTarget = playerTransform;
            }
            if (!playerBullet.Penetrate)
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((knockbackDistance == 0 || collision.gameObject.tag != "player")
            && collision.gameObject.GetComponent<A0126>() == null)
            return;

        Transform PlayersTransform = collision.gameObject.transform;


        //TODO : ��� ���� - 0.15f
        PV.RPC("DecreaseHP", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * 0.15f);
    }
    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / bossSO.hp;
    }

    [PunRPC]
    public void IncreaseHP(float damage)
    {
        currentHP += damage;

        if (currentHP > bossSO.hp)
            currentHP = bossSO.hp;

        GaugeUpdate();
    }

    [PunRPC]
    public void DecreaseHP(float damage)
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
        var _bullet = Instantiate(enemyBulletPrefab, bossHeadPivot.transform.position, bossHeadPivot.transform.rotation);

        _bullet.IsDamage = true;
        _bullet.ATK = bossSO.atk;
        _bullet.BulletLifeTime = bossSO.bulletLifeTime;
        _bullet.BulletSpeed = bossSO.bulletSpeed;
        _bullet.targets["Player"] = (int)BulletTarget.Player;

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


    //Ư�� ���� ��, ��� �÷��̾ �ٶ󺸵��� ����
    private void ChaseView()
    {
        if (currentTarget == null)
        {
            isAttaking = false;
            return;
        }

        //�Ӹ��� Ÿ���� ����
        Vector2 directionToTarget = (currentTarget.position - bossHead.transform.position).normalized;

        //�극�� ����
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.red);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.red);

        LookPlayer(rightBoundary, leftBoundary);
    }

    //Ư�� ���� ��, ��� �÷��̾ �ٶ󺸵��� ����
    private void LookPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (!PhotonNetwork.IsMasterClient || currentTarget != null)
            return;
        //viewDistance > Vector2.Distance(playerTransform.position, transform.position


        //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);


        //�þ߰� ������ ���� Direction
        Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;
        //Enemy�� Player ������ ����
        Vector2 directionToPlayer = (currentTarget.position - transform.position).normalized;

        float angle = Vector3.Angle(directionToPlayer, middleDirection);
        if (angle < viewAngle * 0.5f)
        {
            Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
        }

    }
    #endregion

    #region ���� �׼�[�극��]

    public void Breath()
    {

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
        currentTarget = targetPV.transform;
    }

    [PunRPC]
    private void SendTargetNull()
    {
        currentTarget = null;
    }


    private void SetStateColor()
    {
        spriteRenderer.color = Color.red;
    }
    #endregion

    #region �ִϸ��̼� ����    

    /*
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
    */

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

    #region BehaviourTree ���� 
    void CreateTreeAIState()
    {
        //�ʱ�ȭ&��Ʈ ���� ����
        TreeAIState = new BTRoot();

        //BTSelector�� BTSquence ���� : Ʈ�� ���� ����
        BTSelector BTMainSelector = new BTSelector();


        /*
        //Enemy ���� üũ
        //����� üũ -> ��� �� �ʿ��� �׼ǵ�(������Ʈ ����....)
        BTSquence BTDead = new BTSquence();
        EnemyState_Dead_DeadCondition deadCondition = new EnemyState_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        EnemyState_Dead state_Dead = new EnemyState_Dead(gameObject);
        BTDead.AddChild(state_Dead);
        */


        /*
        //�����̻� üũ [����....]
        BTSquence BTAbnormal = new BTSquence();
        EnemyState_GroggyCondition groggyConditon = new EnemyState_GroggyCondition(gameObject);
        BTAbnormal.AddChild(groggyConditon);
        */


        //������ �Ǻ� <������ �� �����>[�⺻ = 1������  ||  ü�� 50% '�̸�' = 2������]
        BTSelector Phase_One = new BTSelector();

        BossAI_State_SpecialAttack specialAttack_Condition = new BossAI_State_SpecialAttack(gameObject);
        Phase_One.AddChild(specialAttack_Condition);

        //���⿡�� �븻�׼� �������� ����� ���� ����  ���ֱ�
        BTSelector nomalAttack_Selector = new BTSelector();
        Phase_One.AddChild(nomalAttack_Selector);


        BTSquence nomalAttack_Squence_1 = new BTSquence();
        //���� �븻 ���� 1
        //���� �븻 ���� 2
        //����
        //BossAI_State_NomalAttackSequence_1 nomalAttack_Sequence_1 = new BossAI_State_NomalAttackSequence_1(gameObject);
        //nomalAttack_Squence_1.AddChild(�׼ǳ�� ������);
        BTSquence nomalAttack_Squence_2 = new BTSquence();
        //���� �븻 ���� 1
        //���� �븻 ���� 2
        //���� �븻 ���� 3
        //����
        //BossAI_State_NomalAttackSequence_2 nomalAttack_Sequence_2 = new BossAI_State_NomalAttackSequence_2(gameObject);
        //nomalAttack_Squence_2.AddChild(�׼ǳ�� ������);
        BTSquence nomalAttack_Squence_3 = new BTSquence();
        //���� �븻 ���� 1
        //���� �븻 ���� 2
        //���� �븻 ���� 3
        //���� �븻 ���� 4
        //����
        //BossAI_State_NomalAttackSequence3 nomalAttack_Sequence_3 = new BossAI_State_NomalAttackSequence_2(gameObject);
        //nomalAttack_Squence_3.AddChild(�׼ǳ�� ������);

        nomalAttack_Selector.AddChild(nomalAttack_Squence_1);
        nomalAttack_Selector.AddChild(nomalAttack_Squence_2);
        nomalAttack_Selector.AddChild(nomalAttack_Squence_3);

        // ���� ���:
        // EnemyState_Action1 action1 = new EnemyState_Action1(gameObject);
        // EnemyState_Action2 action2 = new EnemyState_Action2(gameObject);
        // specialAttackSequence.AddChild(action1);
        // specialAttackSequence.AddChild(action2);


        BTSelector Phase_Two = new BTSelector();
        








        //�����ʹ� �켱���� ���� ������ ��ġ : ���� ���� -> Ư�� ���� -> �÷��̾� üũ(���� ����) -> �̵� ���� ������ ������ ��ġ 
        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���) 

        //BTMainSelector.AddChild(BTDead);
        //BTMainSelector.AddChild(BTAbnormal);

        //����(������) ������
        BTMainSelector.AddChild(Phase_One);
        BTMainSelector.AddChild(Phase_Two);

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }



    //����ȭ ���� 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� ����
            stream.SendNext(hostPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(spriteRenderer.flipX);
            stream.SendNext(bossHeadPivot.transform.rotation);

        }
        else if (stream.IsReading)
        {
            // �����͸� ����
            hostPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            spriteRenderer.flipX = (bool)stream.ReceiveNext();
            bossHeadPivot.transform.rotation = (Quaternion)stream.ReceiveNext();
        }

    }

    public void SetStateColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

    #endregion

}
