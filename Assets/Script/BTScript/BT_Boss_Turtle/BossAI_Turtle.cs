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
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class BossAI_Turtle : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    //박민혁이만든함수
    public Bullet MissilePrefab;
    public Bullet thornPrefab;
    private int missileCount;
    private int rollCount;
    public bool rolling = false;
    public bool isPhase1;
    public float rollingTime;

    [HideInInspector] public Vector3 direction;

    public float time;
    private float thornAngle = 0;


    public float currentHP;                  // 현재 체력 계산
    [HideInInspector] public float viewAngle;                  // 시야각 (기본120도)
    [HideInInspector] public float viewDistance;               // 시야 거리 (기본 10)

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public Boss_Turtle_SO bossSO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
    public SpriteRenderer spriteRenderer;
    private SpriteRenderer AimSpriteRenderer;
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


    public float SpeedCoefficient = 1f;      // 이동속도 계수


    //각 액션 패턴 쿨타임
    [HideInInspector] public float rollingCooltime;
    [HideInInspector] public float thornTornadoCoolTime;
    [HideInInspector] public float missileCoolTime;



    public bool isLive = true;
    public bool isAttaking = false;
    public bool isGroggy = false;
    public bool isTrackingFurthestTarget = false;
    public bool isEndThornTornado = false;
    public bool isEndMissile = false;
    public bool isFilp = false;
    //플레이어 정보

    [HideInInspector] public int lastAttackPlayer;



    [SerializeField]
    private Image image_Gauge;              //몬스터 UI : Status
    [SerializeField]
    private TextMeshProUGUI txt_Gauge;



    //동기화
    public PhotonView PV;
    private Quaternion hostAimRotation;
    private Vector3 hostPosition;
    private Vector3 hostLocalScale;
    private Vector3 hosAimLocalScale;

    private Vector2 hostKnckbackPosition;
    public float lerpSpeed = 10f; // 보간시 필요한 수치(조정 필요)




    //객체별 넉백거리
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        AimSpriteRenderer = bossAim.GetComponent<SpriteRenderer>();

        PV = GetComponent<PhotonView>();
        
        MissilePrefab = Resources.Load<Bullet>(Enemy_PrefabPathes.BOSS_TURTLE_MISSILE_PREFAB);
        thornPrefab = Resources.Load<Bullet>(Enemy_PrefabPathes.BOSS_TURTLE_THORN_PREFAB);

        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeAIState();
        currentHP = bossSO.hp;
        viewAngle = bossSO.viewAngle;
        viewDistance = bossSO.viewDistance;
        isLive = true;

        originColor = spriteRenderer.color;

        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?

        //rollingCooltime = bossSO.rollingCooltime;
        //thornTornadoCoolTime = bossSO.thornTornadoCoolTime;
        //missileCoolTime = bossSO.missileCoolTime;
        rollingCooltime = 1;
        thornTornadoCoolTime = 0;
        missileCoolTime = 0;

        knockbackDistance = 0f;


        rolling = false;
        isPhase1 = true;


        GaugeUpdate();

        //TODO 생성할 때, 모든 플레이어 Transform 정보를 담는다.TestGameManagerWooMin
        if (TestGameManager.Instance != null)
        {
            //생성할 때, 모든 플레이어 Transform 정보를 담는다.
            foreach (var _value in TestGameManager.Instance.playerInfoDictionary.Values)
            {
                PlayersTransform.Add(_value);
            }
        }
        else if (TestGameManagerWooMin.Instance != null)
        {
            //생성할 때, 모든 플레이어 Transform 정보를 담는다.
            foreach (var _value in TestGameManagerWooMin.Instance.playerInfoDictionary.Values)
            {
                PlayersTransform.Add(_value);
            }
        }
        else
        {
            //생성할 때, 모든 플레이어 Transform 정보를 담는다.
            foreach (var _value in GameManager.Instance.playerInfoDictionary.Values)
            {
                PlayersTransform.Add(_value);
            }
        }



        //생성 시 랜덤 타겟 지정
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget];

    }
    void Update()
    {
        //AI트리의 노드 상태를 매 프레임 마다 얻어옴
        if (!PhotonNetwork.IsMasterClient)
        {
            //$추가됨 : 동기화된 머리 위치에 대한 보간 처리
            transform.position = Vector3.Lerp(transform.position, hostPosition, Time.deltaTime * lerpSpeed);
            transform.localScale = hostLocalScale;
            bossAim.localScale = hosAimLocalScale;
            bossAim.transform.rotation = Quaternion.Slerp(bossAim.transform.rotation, hostAimRotation, Time.deltaTime * lerpSpeed);
            return;
        }

        hostPosition = transform.position;
        hostLocalScale = transform.localScale;

        hosAimLocalScale = bossAim.transform.localScale;
        hostAimRotation = bossAim.transform.rotation;


        if (rolling)
        {
            rollingTime+= Time.deltaTime;
            if (rollingTime >= 5f) 
            {
                Debug.Log("끼임 발생으로 타켓팅 초기화");
                FurthestTarget();
                rollingTime = 0f;
                Vector2 me = transform.position;
                Vector2 u = currentTarget.position;
                direction = (u - me).normalized;
            }
            transform.Translate(direction * bossSO.enemyMoveSpeed * Time.deltaTime);
            //_rigidbody2D.velocity = direction * bossSO.enemyMoveSpeed * Time.deltaTime;
            if (!isPhase1)
            {
                time += Time.deltaTime;
                if (time > bossSO.thornTime) //0.2초마다
                {
                    thornAngle += bossSO.thrronAngle;
                    photonView.RPC("Thorn", RpcTarget.All, thornAngle, 1);
                    if (thornAngle >= 360)
                    {
                        thornAngle = 0;
                    }
                    time = 0;
                }
            }
        }



        TreeAIState.Tick();

        Vector3 directiontarget = (currentTarget.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(directiontarget.y, directiontarget.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        bossAim.rotation = rotation;
        FilpSet();

        if (!isLive)
            return;

        //롤링


        /*
        //목적지와 내 거리가 일정거리 이하거나 / nav가 멈춘 상태(그냥 정지) 가 아닌경우
        if (!IsNavAbled())
        {
            SetAnim("isWalk", false);
            SetAnim("isUpWalk", false);
            return;
        }

        UpdateAnimation();
        */
    }



    #region Enemy 피격, 사망, 넉백, 공격 관련
    //★맞음 & 죽음
    private void GaugeUpdate()
    {
        image_Gauge.fillAmount = (float)currentHP / bossSO.hp;
        txt_Gauge.text = currentHP.ToString("F0") + " / " + bossSO.hp;
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

        //TODO : ★사운드
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

        //TODO : ★사운드
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


    //상태이상
    [PunRPC]
    public void Groggy()
    {
        //행동 제한(이거 총알 맞는 부분에 넣으셈)
        isGroggy = true;

        //동기화 해야할 부분
        //기절에 관한animSet이나 기타 파티클, 효과 등등...
    }
    #endregion

    #region 시야각(타겟 서치) 관련
    private Vector2 BoundaryAngle(float angle)
    {
        // 현재 오브젝트의 회전값을 고려하여 방향 벡터를 계산

        // 현재 오브젝트의 회전값 + 지정 각도값 => 이 값을 라디안으로 변환
        float radAngle = (transform.eulerAngles.z + angle) * Mathf.Deg2Rad;

        // 벡터를 radAngle값을 x,y 방향으로 계산하여 2D 벡터로 반환
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }
    #endregion


    #region 타겟(Player) 관련 
    private void OnTargetChaged(Transform _target)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (_target == null)
                photonView.RPC("SendTargetNull", RpcTarget.Others);
            else
            {
                int viewID = _target.gameObject.GetPhotonView().ViewID; //변하는 viewID
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


    //가장 가까운 타겟 서치
    private void SetNearestTarget()
    {

        float minDistance = float.MaxValue;


        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (PlayersTransform[i] == null)
            {
                continue;
            }

            float distanceToAllTarget = Vector2.Distance(transform.position, PlayersTransform[i].transform.position);

            if (distanceToAllTarget < minDistance)
            {
                minDistance = distanceToAllTarget;
                currentTarget = PlayersTransform[i];
            }
        }
    }


    // 가장 먼 타겟 서치
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
                spriteRenderer.color = Color.red;
                break;
            case (int)BossStateColor.ColorYellow:
                spriteRenderer.color = Color.yellow;
                break;
            case (int)BossStateColor.ColorBlue:
                spriteRenderer.color = Color.blue;
                break;
            case (int)BossStateColor.ColorBlack:
                spriteRenderer.color = Color.black;
                break;
            case (int)BossStateColor.ColorOrigin:
                spriteRenderer.color = originColor;
                break;
            case (int)BossStateColor.ColorMagenta:
                spriteRenderer.color = Color.magenta;
                break;
        }
    }

    [PunRPC]
    public void SetStateColor(Color _color)
    {
        spriteRenderer.color = _color;
    }
    #endregion

    #region 애니메이션 관련


    public void FilpSet()
    {
        if (currentTarget.position.x < transform.position.x)
        {
            isFilp = true; 
            transform.localScale = new Vector3(-1, 1, 1);
            bossAim.transform.localScale = new Vector3(-1, -1, 1);
        }
        else if (currentTarget.position.x > transform.position.x)
        {
            isFilp = false;
            transform.localScale = new Vector3(1, 1, 1);
            bossAim.transform.localScale = new Vector3(1, 1, 1);
        }
        else
            return;
    }


    private void SetAnim(string animName, bool set)
    {
        if (PV.IsMine == false)
            return;

        //이전 상태
        bool prev = anim.GetBool(animName);

        if (prev == set)
            return;

        anim.SetBool(animName, set);
        PV.RPC(nameof(SyncAnimation), RpcTarget.All, animName, set);
    }

    [PunRPC]
    public void SyncAnimation(string animName, bool set)
    {
        anim.SetBool(animName, set);
    }
    #endregion

    #region BehaviourTree 관련 
    void CreateTreeAIState()
    {
        //초기화&루트 노드로 설정
        TreeAIState = new BTRoot();

        //BTSelector와 BTSquence 생성 : 트리 구조 정의
        BTSelector BTMainSelector = new BTSelector();



        //Enemy 생존 체크
        //컨디션 체크 -> 사망 시 필요한 액션들(오브젝트 제거....)
        BTSquence BTDead = new BTSquence();
        BossAI_Turtle_State_Dead_DeadCondition deadCondition = new BossAI_Turtle_State_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        BossAI_Turtle_State_Dead state_Dead = new BossAI_Turtle_State_Dead(gameObject);
        BTDead.AddChild(state_Dead);




        //페이즈 판별 <셀렉터 겸 컨디션>[기본 = 1페이즈  ||  체력 50% '미만' = 2페이즈]
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

        
        BTSquence Phase_2 = new BTSquence();

        BossAI_Turtle_Phase_2_Condition phase_2_Condition = new BossAI_Turtle_Phase_2_Condition(gameObject);
        Phase_2.AddChild(phase_2_Condition);
        BossAI_Turtle_State_Attack_InfinityRolling infinityTornado = new BossAI_Turtle_State_Attack_InfinityRolling(gameObject);
        Phase_2.AddChild(infinityTornado);
        





        //셀렉터는 우선순위 높은 순서로 배치 : 생존 여부 -> 특수 패턴 -> 플레이어 체크(공격 여부) -> 이동 여부 순서로 셀렉터 배치 
        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함) 

        BTMainSelector.AddChild(BTDead);
        //BTMainSelector.AddChild(BTAbnormal);

        //메인(페이즈) 셀렉터
        BTMainSelector.AddChild(Phase_1);
        BTMainSelector.AddChild(Phase_2);

        //작업이 끝난 Selector를 루트 노드에 붙이기
        TreeAIState.AddChild(BTMainSelector);
    }


    #endregion
    #region 미사일
    [PunRPC]
    public void Missile(float atk, float speed, float duration)//총알 생성 프리팹 보스에임이 조준대상이라고 생각하고 있음 뇌피셜임
    {
        MissileCountCheck();

        Bullet _bullet = Instantiate<Bullet>(MissilePrefab, makeMissileZone.transform.position, bossAim.transform.rotation);
        if (isFilp)
        {
            _bullet.transform.localScale = new Vector3(1.5f, -1.5f, 1);
        }

        _bullet.MissileFire(2);
        _bullet.IsDamage = true;
        _bullet.ATK = atk;
        _bullet.BulletLifeTime = duration;
        _bullet.BulletSpeed = speed;
        _bullet.targets["Player"] = (int)BulletTarget.Player;
        _bullet.BulletOwner = photonView.ViewID;
    }
    public void MissileOn() //미사일 발사 발사텀을 주기위해 코루틴 선택 인보크로 대체 가능하리라예상됨
    {
        missileCount = 0; // 3발을쏨 == 3발 다쏘면 이번 패턴 end 조건만족을 위해 카운트 리셋
        StartCoroutine("firefirefire");
    }

    //가 멀 가 미사일
    public IEnumerator firefirefire()
    {
        Debug.Log($"미사일 발사");
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
    #region 가시발사
    [PunRPC]
    public void Thorn(float rot,int type)//가시 미사일과 달리 몸 중앙에서 발사됨
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
        //Debug.Log($"가시쏘기 입장");
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
        //Debug.Log($"가시쏘기 입장2");
        //Debug.Log($"롤링 상태 {rolling}");
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
        }
    }
    #endregion
    #region 구르기모드
    [PunRPC]
    public void RollStart()
    {
        SetAnim("Rolling", true);
        bossAim.gameObject.SetActive(false);
        SetNearestTarget();
        //롤카운트 기준으로 멈추고 n초후(벽에 딱붙어서 멈추지 않기 위함) 멈출것임 트리거에서 if rolling && collier.layer==wall
        rollCount = 0;
        rolling = true;
        rollingTime = 0;
        //Debug.Log($"구르기 변화 체크 {rolling}");
        Vector2 me = transform.position;
        Vector2 u = currentTarget.position;
        direction = (u - me).normalized;

    }
    /*
    public void RollEnd() 
    {
        SetAnim("Rolling", false);
        bossAim.gameObject.SetActive(true);
        Debug.Log($"구르기 퇴장");
        //adas
        rolling = false;
        rollingCooltime = bossSO.rollingCooltime;
        //구르기 패턴 종료
    }
    */
    [PunRPC]
    public void LaterRollEnd()
    {
        StartCoroutine(RollEnd());
    }

    IEnumerator RollEnd()
    {


        SetAnim("Rolling", false);
        bossAim.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        
        rolling = false;
        rollingCooltime = bossSO.rollingCooltime;
        //구르기 패턴 종료
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //호스트에서만 충돌 처리됨
        if (!PhotonNetwork.IsMasterClient)
            return;

        Bullet playerBullet = collision.gameObject.GetComponent<Bullet>();


        if (collision.gameObject.tag == "Bullet" && playerBullet.targets.ContainsValue((int)BulletTarget.Enemy) && playerBullet.IsDamage)
        {
            float atk = collision.transform.GetComponent<Bullet>().ATK;
            int ViewID = playerBullet.BulletOwner;
            //Debug.Log($"뷰아이디 : {ViewID}");
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire)
            {
                Debuff.Instance.GiveFire(this.gameObject, atk, ViewID);
            }
            if (playerBullet.burn)
            {
                GameObject firezone = PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition, Quaternion.identity);
                firezone.GetComponent<A0122_1>().Init(playerBullet.BulletOwner, atk);
            }
            //모든 플레이어에게 현재 적의 체력 동기화
            PV.RPC("DecreaseHP", RpcTarget.All, atk);



            //여기다 불렛 모시깽이 얻기
            lastAttackPlayer = playerBullet.BulletOwner;

            // 뷰ID를 사용하여 포톤 플레이어 찾기&해당 플레이어로 타겟 변경
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
        if (rolling && (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            rollingTime = 0;
            if (isPhase1)
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
                    PV.RPC("LaterRollEnd", RpcTarget.All);
                }
            }
            PlayerStatHandler player = collision.gameObject.GetComponent<PlayerStatHandler>();
            if (player != null)
            {
                player.photonView.RPC("GiveDamege", RpcTarget.All, bossSO.atk * 2);
            }
            Vector3 normal = collision.contacts[0].normal; // 법선벡터
            direction = Vector3.Reflect(direction, normal).normalized; // 반사
        }
    }
    #endregion



    //동기화 관련 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송
            stream.SendNext(hostPosition);
            stream.SendNext(hostLocalScale);
            stream.SendNext(hosAimLocalScale);
            stream.SendNext(hostAimRotation);

        }
        else if (stream.IsReading)
        {
            // 데이터를 수신
            hostPosition = (Vector3)stream.ReceiveNext();
            hostLocalScale = (Vector3)stream.ReceiveNext();
            hosAimLocalScale = (Vector3)stream.ReceiveNext();
            hostAimRotation = (Quaternion)stream.ReceiveNext();
        }

    }
}


