using ExitGames.Client.Photon.StructWrapping;
using myBehaviourTree;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class BossAI_Turtle : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    //�ڹ����̸����Լ�
    public Bullet MissilePrefab;
    private int missileCount;
    private int rollCount;
    public Bullet thornPrefab;
    public bool rolling = false;
    public bool isPhase1;

    [HideInInspector] public Vector3 direction;

    public Rigidbody2D _rigidbody2D;
    public float time;
    public float thornTime = 0.2f;
    private float thornAngle = 0;


    public float currentHP;                  // ���� ü�� ���
    [HideInInspector] public float viewAngle;                  // �þ߰� (�⺻120��)
    [HideInInspector] public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public Boss_Turtle_SO bossSO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
    public SpriteRenderer spriteRenderers;
    public Animator anim;


    private Transform target;
    public Transform currentTarget
    {
        get { return target; }
        set { if (target != value) { OnTargetChaged(value); target = value; } }
    }

    //public Collider2D target;
    [HideInInspector] public List<Transform> PlayersTransform;



    public Transform bossAim;
    public GameObject makeMissileZone;
    public Color originColor;


    public float SpeedCoefficient = 1f;      // �̵��ӵ� ���


    //�� �׼� ���� ��Ÿ��
    [HideInInspector] public float rollingCooltime;
    [HideInInspector] public float thornTornadoCoolTime;
    [HideInInspector] public float missileCoolTime;



    public bool isLive = true;
    public bool isAttaking = false;
    public bool isGroggy = false;
    public bool isTrackingFurthestTarget = false;
    public bool isEndThornTornado = false;
    public bool isEndMissile = false;
    //�÷��̾� ����

    [HideInInspector] public int lastAttackPlayer;



    [SerializeField]
    private Image image_Gauge;              //���� UI : Status
    [SerializeField]
    private TextMeshProUGUI txt_Gauge;


    //����ȭ
    public PhotonView PV;
    private Quaternion hostAimRotation;
    private Vector3 hostPosition;
    private Vector2 hostKnckbackPosition;
    public float lerpSpeed = 10f; // ������ �ʿ��� ��ġ(���� �ʿ�)




    //��ü�� �˹�Ÿ�
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderers = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeAIState();
        currentHP = bossSO.hp;
        viewAngle = bossSO.viewAngle;
        viewDistance = bossSO.viewDistance;
        isLive = true;

        originColor = spriteRenderers.color;

        //�ڽ̱� �׽�Ʈ �� if else �ּ�ó�� �Ұ�
        //�Ѵ� �÷��̾ ȣ��Ʈ�� �Ǻ�?

        //rollingCooltime = bossSO.rollingCooltime;
        //thornTornadoCoolTime = bossSO.thornTornadoCoolTime;
        //missileCoolTime = bossSO.missileCoolTime;
        rollingCooltime = 1;
        thornTornadoCoolTime = 0;
        missileCoolTime = 0;

        knockbackDistance = 0f;


        //TODO ������ ��, ��� �÷��̾� Transform ������ ��´�.TestGameManagerWooMin
        foreach (var _value in GameManager.Instance.playerInfoDictionary.Values)
        {
            PlayersTransform.Add(_value);
        }



        //���� �� ���� Ÿ�� ����
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget];

        rolling = false;
        isPhase1 = true;
    }
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //$�߰��� : ����ȭ�� �Ӹ� ��ġ�� ���� ���� ó��
            transform.position = Vector3.Lerp(transform.position, hostPosition, Time.deltaTime * lerpSpeed);
            bossAim.transform.rotation = Quaternion.Slerp(bossAim.transform.rotation, hostAimRotation, Time.deltaTime * lerpSpeed);
            return;
        }
        //AIƮ���� ��� ���¸� �� ������ ���� ����

        TreeAIState.Tick();

        if (!isLive)
            return;

        if (rolling)
        {
            _rigidbody2D.velocity = direction * bossSO.enemyMoveSpeed * Time.deltaTime;
            if (!isPhase1)
            {
                time += Time.deltaTime;
                if (time > thornTime) //0.2�ʸ���
                {
                    thornAngle += 2.5f;
                    photonView.RPC("Thorn", RpcTarget.All, thornAngle, 1);
                    if (thornAngle >= 360)
                    {
                        thornAngle = 0;
                    }

                }
            }
        }
        else 
        {
            _rigidbody2D.velocity = Vector3.zero;
            Vector3 directiontarget = (currentTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(directiontarget.y, directiontarget.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            bossAim.rotation = rotation;


        }

        hostPosition = transform.position;
        hostAimRotation = bossAim.transform.rotation;


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
    private void GaugeUpdate()
    {
        image_Gauge.fillAmount = (float)currentHP / bossSO.hp;
        txt_Gauge.text = currentHP + " / " + bossSO.hp; // ���� ü�� / �ִ� ü�� ǥ��
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
        if (!isLive)
        {
            return;
        }
        //PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorRed, PV.ViewID);
        currentHP -= damage;
        GaugeUpdate();
        if (currentHP <= 0)
        {
            isLive = false;
        }
    }

    [PunRPC]
    public void DecreaseHPByObject(float damage, int viewID)
    {
        if (!isLive)
        {
            return;
        }
        //PV.RPC("SetStateColor", RpcTarget.All, (int)EnemyStateColor.ColorRed, PV.ViewID);
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
        GameManager.Instance.CallBossStageEndEvent();
        Destroy(gameObject);
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
    #endregion


    #region Ÿ��(Player) ���� 
    private void OnTargetChaged(Transform _target)
    {
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

    /*
    public void SetHead() // ���ط�, �÷��̾� ��ġ �޾ƿ�
    {
        //�÷��̾ �ٶ󺸵��� ����

        //anim.SetTrigger("Attack"); // ���� �ִϸ��̼�

        //���� �Ӹ��� ������ ���� �� �ش� �������� Rotat
        Vector3 direction = (currentTarget.transform.position - bossHead.transform.position).normalized;



        RotateHead(direction);
    }
    private void RotateHead(Vector2 newAim)
    {
        float rotZ = Mathf.Atan2(newAim.y, newAim.x) * Mathf.Rad2Deg;
        rotZ += 90f;

        if (rotZ > 40 || rotZ < -40f)
        {
            ReturnOriginRotate();
            //����� ��������[�� ���� �� ��������] �־ �ļ� ����
            return;
        }

        // ���ϴ� ȸ�� ���� ����
        float minRotation = -25f;
        float maxRotation = 25f;
        //270~61 == ȸ���ϸ� �ȵ�
        rotZ = Mathf.Clamp(rotZ, minRotation, maxRotation);


        // ���� ȸ�� ����
        Quaternion currentRotation = bossHead.transform.rotation;

        // ��ǥ ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotZ);

        // ȸ�� ����
        float interpolationFactor = 0.005f; // ���� ���
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossHead.transform.rotation = interpolatedRotation;
    }
    
    private void ReturnOriginRotate()
    {
        // ��ǥ ȸ�� ����
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        // ���� ȸ�� ����
        Quaternion currentRotation = bossHead.transform.rotation;

        // ȸ�� ����
        float interpolationFactor = 0.005f; // ���� ���
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossHead.transform.rotation = interpolatedRotation;
    }
    */


    //���� ����� Ÿ�� ��ġ
    private void SetNearestTarget()
    {


        float minDistance = float.MaxValue;


        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (PlayersTransform[i] == null)
                continue;

            float distanceToAllTarget = Vector2.Distance(transform.position, PlayersTransform[i].transform.position);

            if (distanceToAllTarget < minDistance)
            {
                minDistance = distanceToAllTarget;
                currentTarget = PlayersTransform[i];
            }
        }
    }


    // ���� �� Ÿ�� ��ġ
    private void FurthestTarget()
    {
        float maxDistance = float.MinValue;

        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (PlayersTransform[i] == null)
                continue;

            float distanceToAllTarget = Vector2.Distance(transform.position, PlayersTransform[i].transform.position);

            if (distanceToAllTarget > maxDistance)
            {
                maxDistance = distanceToAllTarget;
                currentTarget = PlayersTransform[i];
            }
        }
    }









    private void SetColor(int colorNum)
    {
        switch (colorNum)
        {
            case (int)BossStateColor.ColorRed:
                spriteRenderers.color = Color.red;
                break;
            case (int)BossStateColor.ColorYellow:
                spriteRenderers.color = Color.yellow;
                break;
            case (int)BossStateColor.ColorBlue:
                spriteRenderers.color = Color.blue;
                break;
            case (int)BossStateColor.ColorBlack:
                spriteRenderers.color = Color.black;
                break;
            case (int)BossStateColor.ColorOrigin:
                spriteRenderers.color = originColor;
                break;
            case (int)BossStateColor.ColorMagenta:
                spriteRenderers.color = Color.magenta;
                break;
        }
    }

    [PunRPC]
    public void SetStateColor(Color _color)
    {
        spriteRenderers.color = _color;
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



        //Enemy ���� üũ
        //����� üũ -> ��� �� �ʿ��� �׼ǵ�(������Ʈ ����....)
        BTSquence BTDead = new BTSquence();
        BossAI_Turtle_State_Dead_DeadCondition deadCondition = new BossAI_Turtle_State_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        BossAI_Turtle_State_Dead state_Dead = new BossAI_Turtle_State_Dead(gameObject);
        BTDead.AddChild(state_Dead);




        //������ �Ǻ� <������ �� �����>[�⺻ = 1������  ||  ü�� 50% '�̸�' = 2������]
        BTSquence Phase_1 = new BTSquence();

        BossAI_Turtle_Phase_1_Condition phaseOneCondition = new BossAI_Turtle_Phase_1_Condition(gameObject);
        Phase_1.AddChild(phaseOneCondition);




        BTSelector phase_1_ActionSelector = new BTSelector();
        Phase_1.AddChild(phase_1_ActionSelector);


        BossAI_Turtle_Idle idle = new BossAI_Turtle_Idle(gameObject);
        phase_1_ActionSelector.AddChild(idle);
        BossAI_Turtle_State_Attack_Rolling rollingAttack = new BossAI_Turtle_State_Attack_Rolling(gameObject);
        phase_1_ActionSelector.AddChild(rollingAttack);
        BossAI_Turtle_State_Attack_ThornTornado tornado = new BossAI_Turtle_State_Attack_ThornTornado(gameObject);
        phase_1_ActionSelector.AddChild(tornado);
        BossAI_Turtle_State_Attack_Missile missile = new BossAI_Turtle_State_Attack_Missile(gameObject);
        phase_1_ActionSelector.AddChild(missile);

        /*
        BTSelector Phase_2 = new BTSelector();

        BossAI_Turtle_Phase_2_Condition phase_2_Condition = new BossAI_Turtle_Phase_2_Condition(gameObject);
        Phase_2.AddChild(phase_2_Condition);

        BossAI_Turtle_State_Attack_ThornTornado Phase_2_tornado = new BossAI_Turtle_State_Attack_ThornTornado(gameObject);
        Phase_2.AddChild(Phase_2_tornado);
        */





        //�����ʹ� �켱���� ���� ������ ��ġ : ���� ���� -> Ư�� ���� -> �÷��̾� üũ(���� ����) -> �̵� ���� ������ ������ ��ġ 
        //���� ������ : Squence�� Selector�� �ڽ����� �߰�(�ڽ� ���� �߿���) 

        BTMainSelector.AddChild(BTDead);
        //BTMainSelector.AddChild(BTAbnormal);

        //����(������) ������
        BTMainSelector.AddChild(Phase_1);
        //BTMainSelector.AddChild(Phase_2);

        //�۾��� ���� Selector�� ��Ʈ ��忡 ���̱�
        TreeAIState.AddChild(BTMainSelector);
    }


    #endregion
    #region �̻���
    [PunRPC]
    public void Missile(float atk, float speed, float duration)//�Ѿ� ���� ������ ���������� ���ش���̶�� �����ϰ� ���� ���Ǽ���
    {
        MissileCountCheck();
        Bullet _bullet = Instantiate<Bullet>(MissilePrefab, makeMissileZone.transform.position, bossAim.transform.rotation);
        _bullet.MissileFire(2);
        _bullet.IsDamage = true;
        _bullet.ATK = atk;
        _bullet.BulletLifeTime = duration;
        _bullet.BulletSpeed = speed;
        _bullet.targets["Player"] = (int)BulletTarget.Player;
        _bullet.BulletOwner = photonView.ViewID;
    }
    public void MissileOn() //�̻��� �߻� �߻����� �ֱ����� �ڷ�ƾ ���� �κ�ũ�� ��ü �����ϸ��󿹻��
    {
        missileCount = 0; // 3������ == 3�� �ٽ�� �̹� ���� end ���Ǹ����� ���� ī��Ʈ ����
        StartCoroutine("firefirefire");
    }

    //�� �� �� �̻���
    public IEnumerator firefirefire()
    {
        Debug.Log($"�̻��� �߻�");
        SetNearestTarget();
        float atk = bossSO.atk * 2f;
        float speed = bossSO.bulletSpeed * 0.5f ;
        float bulletLifeTIme = bossSO.bulletLifeTime;

        photonView.RPC("Missile", RpcTarget.All, atk, speed, bulletLifeTIme);
        yield return new WaitForSeconds(0.5f);

        float atk2 = atk * 0.5f;
        float speed2 = speed * 2f;
        FurthestTarget();
        photonView.RPC("Missile", RpcTarget.All, atk2, speed2, bulletLifeTIme); // atk * 0.5 , speed * 2
        yield return new WaitForSeconds(0.5f);

        float atk3 = atk * 2f;
        float speed3 = speed * 0.5f;
        float bulletLifeTime3 = bulletLifeTIme*2;
        SetNearestTarget();
        photonView.RPC("Missile", RpcTarget.All, atk, speed, bulletLifeTIme); //atk * 2, speed * 0.5, bulletLifeTIme * 2

    }
    public void MissileCountCheck()
    {
        missileCount++;
        if (missileCount >= 3)
        {
            isEndMissile = true;
            missileCoolTime = bossSO.missileCoolTime;
        }
    }
    #endregion
    #region ���ù߻�
    [PunRPC]
    public void Thorn(float rot,int type)//���� �̻��ϰ� �޸� �� �߾ӿ��� �߻��
    {
        Quaternion angle = Quaternion.Euler(new Vector3(0, 0, rot));
        Bullet _bullet = Instantiate<Bullet>(thornPrefab, transform.position, angle);
        _bullet.IsDamage = true;
        _bullet.ATK = bossSO.atk;
        _bullet.BulletLifeTime = bossSO.bulletLifeTime;
        _bullet.BulletSpeed = bossSO.bulletSpeed;
        if (type == 1)
        {
            _bullet.BulletSpeed = 1f;
        }
        else 
        {
            _bullet.BulletSpeed = bossSO.bulletSpeed;
        }
        _bullet.targets["Player"] = (int)BulletTarget.Player;
        _bullet.BulletOwner = photonView.ViewID;
    }
    public void ThornTornado1()
    {
        //Debug.Log($"���ý�� ����");
        float n = 0;
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, n));
        int atktype = 0;
        if (rolling)
        {
            atktype = 1;
        }
        for (int i = 0; i < 8; ++i)
        {
            photonView.RPC("Thorn", RpcTarget.All, n, atktype);
            n += 45;
        }
        if (!rolling) 
        {
        Invoke("ThornTornado2", 1f);
        }
    }
    public void ThornTornado2()
    {
        float n = 22.5f;
        //Debug.Log($"���ý�� ����2");
        //Debug.Log($"�Ѹ� ���� {rolling}");
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, n));
        int atktype = 0;
        if (rolling)
        {
            atktype = 1;
        }
        for (int i = 0; i < 8; ++i)
        {
            photonView.RPC("Thorn", RpcTarget.All, n, atktype);
            n += 45;
        }
        if (!rolling)
        {
            isEndThornTornado = true;
            thornTornadoCoolTime = bossSO.thornTornadoCoolTime;
            Debug.Log($"���ý�� ����");
        }
    }
    #endregion
    #region ��������
    public void RollStart()
    {
        SetNearestTarget();
        Debug.Log($"������ ����");
        //��ī��Ʈ �������� ���߰� n����(���� ���پ ������ �ʱ� ����) ������� Ʈ���ſ��� if rolling && collier.layer==wall
        rollCount = 0;
        rolling = true;
        //Debug.Log($"������ ��ȭ üũ {rolling}");
        Vector2 me = transform.position;
        Vector2 u = currentTarget.position;
        direction = (u - me).normalized;

    }
    public void RollEnd() 
    {
        Debug.Log($"������ ����");
        rolling = false;
        rollingCooltime = bossSO.rollingCooltime;
        //������ ���� ����
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rolling && isPhase1 && collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (rollCount % 2 == 0)
            {
                ThornTornado1();
            }
            else
            {
                ThornTornado2();
            }
            rollCount++;
            if (rollCount >= bossSO.endRollCount)
            {
                Invoke("RollEnd", 0.2f);
            }
            Vector3 normal = collision.contacts[0].normal; // ��������
            direction = Vector3.Reflect(direction, normal).normalized; // �ݻ�
        }
        else if (!isPhase1 && collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer ==LayerMask.NameToLayer("Player")) 
        {
            Vector3 normal = collision.contacts[0].normal; // ��������
            Debug.Log($"���� ���⺤�� {direction}");
            direction = Vector3.Reflect(direction, normal).normalized; // �ݻ�
            Debug.Log($"ƨ�� ���e���� {direction}");
        }

        if (rolling && collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (player != null) 
            {

                player.photonView.RPC("GiveDamege",RpcTarget.All, bossSO.atk*2);
            }
        }
    }
    public void updateclone()//������Ʈ ������ �� �ٵ� ������Ʈ ������ �̻��ҰŰ��Ƽ� �̷��� �ص�
    {
        if (rolling)
        {
            _rigidbody2D.velocity = direction * bossSO.enemyMoveSpeed * Time.deltaTime;
            if (!isPhase1)
            {
                time += Time.deltaTime;
                if (time > thornTime) //0.2�ʸ���
                {
                    thornAngle += 2.5f;
                    photonView.RPC("Thorn", RpcTarget.All, thornAngle, 1);
                    if (thornAngle >= 360)
                    {
                        thornAngle = 0;
                    }
                }
            }
        }

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
            stream.SendNext(hostAimRotation);

        }
        else if (stream.IsReading)
        {
            // �����͸� ����
            hostPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            hostAimRotation = (Quaternion)stream.ReceiveNext();
        }

    }
}


