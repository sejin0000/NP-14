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

//[Root 노드] => 왜 액션과 다르게 상속 안받음?
//==>특정 AI 동작과 상태에 맞게 유연하게 조정하기 위해서
//대신 BTRoot 객체를 생성하고, 그 아래에 복합 노드와 액션 노드를 추가해서 트리를 구성함
//EnemyAI : AI의 상태와 동작의 구조를 정의하고, 시작함

//Enemy에 필요한 컴포넌트들 + 기타 요소들 여기에 다 추가

public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public bool CanFire;
    public bool CanWater;
    public bool CanIce;

    public float currentHP;                  // 현재 체력 계산
    public float maxHP;                     

    private float viewAngle;                  // 시야각 (기본120도)
    private float viewDistance;               // 시야 거리 (기본 10)

    public int roomNum;                    // 방의 정보(클리어 조건을 위해 사용됨 -세진-)

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public EnemySO enemySO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
    public float appliedATK;
    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public Color originColor;

    private Transform target;
    public Transform Target { get { return target; } 
                               set { if (target != value) { OnTargetChaged(value); target = value; } } }//추적 타겟[Palyer]
    //public Collider2D target;
    public NavMeshAgent nav;
    public Vector3 navTargetPoint;              //nav 목적지
    public List<Transform> PlayersTransform;

    public GameObject enemyAim;
    public Bullet enemyBulletPrefab;


    public LayerMask targetMask;             // 타겟 레이어(Player)

    public float SpeedCoefficient = 1f;      // 이동속도 계수


    public bool isLive;
    public bool isChase;
    public bool isAttaking;
    public bool isGroggy;

    //플레이어 정보

    public int lastAttackPlayer;

    //게임매니저에서(어디든) 관리하는 플레이어들 정보를 요청해서 사용

    //가장많은 피해를 준 플레이어 타겟-> 불렛(쏜사람 정보) 맞은놈만 알면됨 ->플레이 공격력->

    Vector2 nowEnemyPosition;
    Quaternion nowEnemyRotation;
    [SerializeField]
    private Image images_Gauge;              //몬스터 UI : Status
    private SpriteLibrary spriteLibrary;     //스프라이트 라이브러리(껍데기 변환용)


    //동기화
    public PhotonView PV;
    private Vector3 hostPosition;
    public float lerpSpeed = 10f; // 보간시 필요한 수치(조정 필요)


    //넉백
    public bool isKnockback = false;

    //넉백 시작 시점 & 넉백을 주는 타겟
    public Vector2 knockbackStartPosition;
    public Vector2 knockbackTargetPosition;

    //넉백 시작시간 & 넉백 지속 시간
    public float knockbackStartTime;
    public float knockbackDuration = 0.3f;

    public float ViewDistanceThreshold = 0.2f;
    public float KnockbackLimitTime = 0.2f;



    //객체별 넉백거리
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


        appliedATK = enemySO.atk;
        maxHP = enemySO.hp;

        currentHP = maxHP;

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
        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeAIState();

        viewAngle = enemySO.viewAngle;
        viewDistance = enemySO.viewDistance;
        isLive = true;

        nav.updateRotation = false;
        nav.updateUpAxis = false;

        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?


        nowEnemyPosition = this.gameObject.transform.position;
        knockbackDistance = 0f;

        nav.speed = enemySO.enemyMoveSpeed;
        navTargetPoint = transform.position;


        //호스트만 nav활성화 하도록 설정
        if (!PhotonNetwork.IsMasterClient)
            nav.enabled = false;
        else
            nav.enabled = true;

        if(TestGameManager.Instance != null)
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
    }
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            //$추가됨 : 동기화된 위치에 대한 보간 처리
            transform.position = Vector3.Lerp(transform.position, hostPosition, Time.deltaTime * lerpSpeed);
            return;
        }

        hostPosition = transform.position;

        //AI트리의 노드 상태를 매 프레임 마다 얻어옴
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
            

        // 넉백 중인 경우
        if (isKnockback)
        {
            HandleKnockback();
        }



        //목적지와 내 거리가 일정거리 이하거나 / nav가 멈춘 상태(그냥 정지) 가 아닌경우
        if (!IsNavAbled() || nav.remainingDistance < 0.2f)
        {          
            SetAnim("isWalk", false);
            SetAnim("isUpWalk", false);
            return;
        }

        UpdateAnimation();
    }


    //Enemy 이동 속도변경 관련
    public void ChangeSpeed(float statSpeed)
    {
        nav.speed = statSpeed * SpeedCoefficient;
    }

    #region Enemy 피격, 사망, 넉백, 공격 관련
    //★맞음 & 죽음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //호스트에서만 충돌 처리됨
        if (!PhotonNetwork.IsMasterClient)
            return;

        Bullet playerBullet = collision.gameObject.GetComponent<Bullet>();


        if (collision.gameObject.tag == "Bullet" && playerBullet.targets.ContainsValue((int)BulletTarget.Enemy) && playerBullet.IsDamage)
        {
            float atk = collision.transform.GetComponent<Bullet>().ATK;
            isChase = true;
            int ViewID = playerBullet.BulletOwner;
            //Debug.Log($"뷰아이디 : {ViewID}");
            PhotonView PlayerPv = PhotonView.Find(ViewID);
            PlayerStatHandler player = PlayerPv.gameObject.GetComponent<PlayerStatHandler>();
            player.EnemyHitCall();


            if (playerBullet.fire) 
            {
                Debuff.Instance.GiveFire(this.gameObject, atk,ViewID);
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
                    Debug.Log("얼음체크");
                    Debuff.Instance.GiveIce(this.gameObject);
                }
            }
            if (playerBullet.burn)
            {
                GameObject firezone =PhotonNetwork.Instantiate("AugmentList/A0122", transform.localPosition,quaternion.identity);
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
            //모든 플레이어에게 현재 적의 체력 동기화
            PV.RPC("DecreaseHP", RpcTarget.All, atk);

            float BulletknockbackDistance = 2.0f;


            //여기다 불렛 모시깽이 얻기
            lastAttackPlayer = playerBullet.BulletOwner;

            // 뷰ID를 사용하여 포톤 플레이어 찾기&해당 플레이어로 타겟 변경
            PhotonView photonView = PhotonView.Find(playerBullet.BulletOwner);
            if (photonView != null)
            {
                Transform playerTransform = photonView.transform;

                Target = playerTransform;
            }

            //넉백(충돌 대상과&Enemy 방향 정규화)
            Vector2 directionToBullet = (collision.transform.position - transform.position).normalized;

            // 넉백 시작 위치와 목표 위치 계산
            knockbackStartPosition = transform.position;
            knockbackTargetPosition = knockbackStartPosition - directionToBullet * BulletknockbackDistance;

            // 넉백 시작 시간 저장
            knockbackStartTime = Time.time;

            // 업데이트 넉백 실행
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
            && collision.gameObject.GetComponent<A3104>() == null )
            return;

        Transform PlayersTransform = collision.gameObject.transform;

        //넉백(충돌 대상과&Enemy 방향 정규화)
        Vector2 directionToPlayer = (collision.transform.position - transform.position).normalized * knockbackDistance;

        // 넉백 시작 위치와 목표 위치 계산
        knockbackStartPosition = transform.position;
        knockbackTargetPosition = knockbackStartPosition - directionToPlayer;

        // 넉백 시작 시간 저장
        knockbackStartTime = Time.time;

        // 업데이트 넉백 실행
        isKnockback = true;

        // 계수 조정
        float damageCoeff = 0;

        if (collision.gameObject.GetComponent<A0126>() != null)
        {
            damageCoeff += collision.gameObject.GetComponent<A0126>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
        if (collision.gameObject.GetComponent<A3104>() != null)
        {
            damageCoeff += collision.gameObject.GetComponent<A3104>().DamageCoeff;
            int viewID = collision.gameObject.GetPhotonView().ViewID;
            PV.RPC("DecreaseHPByObject", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * damageCoeff, viewID);
        }
        knockbackDistance = 0f;
    }
    private void HandleKnockback()
    {
        //넉백 지속시간
        float knockbackRatio = (Time.time - knockbackStartTime) / knockbackDuration;
        transform.position = Vector2.Lerp(knockbackStartPosition, knockbackTargetPosition, knockbackRatio);

        if (!PhotonNetwork.IsMasterClient)
        {
            //$추가됨 : 동기화된 위치에 대한 보간 처리
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
        images_Gauge.fillAmount = (float)currentHP / maxHP; //체력
    }

    [PunRPC]
    public void IncreaseHP(float damage)
    {
        currentHP += damage;

        if (currentHP > maxHP)
            currentHP = maxHP;

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


    //정확히
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
        _bullet.ATK = appliedATK;
        _bullet.BulletLifeTime = enemySO.bulletLifeTime;
        _bullet.BulletSpeed = enemySO.bulletSpeed;
        _bullet.targets["Player"] = (int)BulletTarget.Player;
        _bullet.BulletOwner = photonView.ViewID;

        /*
        //수정 : gameObject 에서 Bullet으로 ->변수 형태와 용도를 통일함
        Bullet _bullet = Instantiate<Bullet>(enemyBulletPrefab, enemyAim.transform.position, enemyAim.transform.rotation);



        _bullet.IsDamage = true;
        _bullet.ATK = enemySO.atk;
        _bullet.BulletLifeTime = enemySO.bulletLifeTime;
        _bullet.BulletSpeed = enemySO.bulletSpeed;
        _bullet.target = BulletTarget.Player;
        */

        //수정 : gameObject 에서 Bullet으로 ->변수 형태와 용도를 통일함      
    }

    //상태이상
    [PunRPC]
    public void Groggy()
    {
        //행동 제한(이거 총알 맞는 부분에 넣으셈)
        isGroggy = true;
        nav.isStopped = false;

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



    private void NomalView()
    {
        Vector2 rightBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector2 leftBoundary = BoundaryAngle(viewAngle * 0.5f);

        // 스프라이트 랜더러 flipX 상태에 따라 레이 방향을 반대로 설정
        if (spriteRenderer.flipX)
        {
            rightBoundary = -rightBoundary;
            leftBoundary = -leftBoundary;
        }

        //Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.yellow);
       // Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.yellow);

        FindPlayer(rightBoundary, leftBoundary);
    }


    //추적, 공격시 플레이어를 바라보는 시야각으로 전환
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

       // Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.black);
       // Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.black);

        FindPlayer(rightBoundary, leftBoundary);
    }


    private void FindPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (!PhotonNetwork.IsMasterClient || Target != null || isChase)
            return;
        //viewDistance > Vector2.Distance(playerTransform.position, transform.position


        //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);


        //받아온 모든 플레이어 트랜스폼을 받아온다.
        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (viewDistance >= Vector2.Distance(PlayersTransform[i].position, transform.position) &&
                PlayersTransform[i].gameObject.layer == LayerMask.NameToLayer("Player"))
            {                
                //시야각 방향의 직선 Direction
                Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;
                //Enemy와 Player 사이의 방향
                Vector2 directionToPlayer = (PlayersTransform[i].position - transform.position).normalized;

                float angle = Vector3.Angle(directionToPlayer, middleDirection);
                if (angle < viewAngle * 0.5f)
                {
                    isChase = true;
                    Target = PlayersTransform[i];  // 시야각 안에 있는 플레이어로 currentTargetPlayer 설정
                    //Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
                    Debug.Log($"타겟 수집{Target}");
                    break;
                }
                else
                    Target = null;
            }

        }
    }
    #endregion

    #region 타겟(Player) 관련 
    private void OnTargetChaged(Transform _target)
    {
        //마스터 클라이언트가 몬스터를 소환하고, 해당 몬스터들이

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
        Target = targetPV.transform;
    }

    [PunRPC]
    private void SendTargetNull()
    {
        Target = null;
    }

    //1.이넘으로 칼라 세팅을 하고 해당 이넘에 대한 인트 보내기
    //2.컬러 4색 요소를 다 보내기
    [PunRPC]
    private void SetStateColor(int colorNum, int viewID)
    {
        //TODO
        PV = PhotonView.Find(viewID);
        PV.GetComponent<EnemyAI>().SetColor(colorNum);
    }

    private void SetColor(int colorNum)
    {
        switch(colorNum)
        {
            case (int)EnemyStateColor.ColorRed:
                spriteRenderer.color = Color.red;
                //Debug.Log($"지금 스프라이트 색상{spriteRenderer.color}");
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

    #region player애니메이션 관련    

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
        //Debug.Log($"{animName}이 {set} 상태로 호출됨");
        anim.SetBool(animName, set);
    }

    #endregion

    #region NavAgent 관련   
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
            nav.isStopped = false; // 활성화
            return true;
        }           
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
        EnemyState_Dead_DeadCondition deadCondition = new EnemyState_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        EnemyState_Dead state_Dead = new EnemyState_Dead(gameObject);
        BTDead.AddChild(state_Dead);

        //상태이상 체크 [스턴....]
        BTSquence BTAbnormal = new BTSquence();
        EnemyState_GroggyCondition groggyConditon = new EnemyState_GroggyCondition(gameObject);
        BTAbnormal.AddChild(groggyConditon);



        //추적+공격
        //컨디션 체크 -> 플레이어 추적 & 플레이어가 공격 범위 내 -> 공격(성공 반환 후 최초로)
        BTSquence BTChase = new BTSquence();

        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase state_Chase = new EnemyState_Chase(gameObject);
        BTChase.AddChild (state_Chase);

        EnemyState_Attack_AttackCondition attackCondition = new EnemyState_Attack_AttackCondition(gameObject);
        BTChase.AddChild(attackCondition);
        EnemyState_Attack state_Attack = new EnemyState_Attack(gameObject);
        BTChase.AddChild(state_Attack);





        //순찰(시퀀스 : 하나라도 실패하면 실패반환)
        //할거 없으면 이동
        BTSquence BTPatrol = new BTSquence();

        EnemyState_Patrol state_Patrol = new EnemyState_Patrol(gameObject);
        BTPatrol.AddChild(state_Patrol);



        //셀렉터는 우선순위 높은 순서로 배치 : 생존 여부 -> 특수 패턴 -> 플레이어 체크(공격 여부) -> 이동 여부 순서로 셀렉터 배치 
        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함) 

        BTMainSelector.AddChild(BTDead);
        BTMainSelector.AddChild(BTAbnormal);
        BTMainSelector.AddChild(BTChase);
        BTMainSelector.AddChild(BTPatrol);

        //작업이 끝난 Selector를 루트 노드에 붙이기
        TreeAIState.AddChild(BTMainSelector);
    }


    private void MoveToHostPosition()
    {
        // 현재 위치에서 네트워크로부터 받은 목표 위치로 부드럽게 이동
        transform.position = Vector3.Lerp(transform.position, navTargetPoint, Time.deltaTime * lerpSpeed);
    }
    #endregion

    //동기화 관련 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송
            stream.SendNext(hostPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(spriteRenderer.flipX);
            stream.SendNext(enemyAim.transform.rotation);

        }
        else if (stream.IsReading)
        {
            // 데이터를 수신
            hostPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            spriteRenderer.flipX = (bool)stream.ReceiveNext();
            enemyAim.transform.rotation = (Quaternion)stream.ReceiveNext();
        }   
    }

    public override void OnDisable()
    {
        base.OnDisable();
        //Debug.Log("몬스터 죽음요");
        if (!GameManager.Instance.isStageEnd)
        {
            PV.RPC("DeadSync", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    public void DeadSync()
    {
        if (GameManager.Instance == null)
            return;
        GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].roomInMoster--;
        if (GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].roomInMoster == 0)
        {
            GameManager.Instance.MG.roomNodeInfo.allRoomList[roomNum].thisRoomClear = true;
            //GameManager.Instance.CallRoomEndEvent();
            GameManager.Instance.PV.RPC("CallRoomEndEvent", RpcTarget.MasterClient);
        }
    }
}
