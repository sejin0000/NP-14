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

    public float currentHP;                  // 현재 체력 계산
    public float viewAngle;                  // 시야각 (기본120도)
    public float viewDistance;               // 시야 거리 (기본 10)

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public EnemySO enemySO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
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

    public GameObject enemyAim;
    public Bullet enemyBulletPrefab;


    public LayerMask targetMask;             // 타겟 레이어(Player)

    public float SpeedCoefficient = 1f;      // 이동속도 계수


    public bool isLive;
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



    //동기화
    public PhotonView PV;
    private Vector3 hostPosition;
    public float lerpSpeed = 10f; // 보간시 필요한 수치(조정 필요)




    //객체별 넉백거리
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();

        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeAIState();
        currentHP = enemySO.hp;
        viewAngle = enemySO.viewAngle;
        viewDistance = enemySO.viewDistance;
        isLive = true;


        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?


        nowEnemyPosition = this.gameObject.transform.position;
        knockbackDistance = 0f;



        //생성할 때, 모든 플레이어 Transform 정보를 담는다.
        foreach (var _value in TestGameManager.Instance.playerInfoDictionary.Values)
        {
            PlayersTransform.Add(_value);
        }

        //생성 시 랜덤 타겟 지정
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget]; 
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


        if (isAttaking)
        {
            //PV.RPC("Filp", RpcTarget.All);
            ChaseView();
        }
        else
        {
            NomalView();
        }



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
        if ((knockbackDistance == 0 || collision.gameObject.tag != "player")
            && collision.gameObject.GetComponent<A0126>() == null)
            return;

        Transform PlayersTransform = collision.gameObject.transform;


        //TODO : 계수 수정 - 0.15f
        PV.RPC("DecreaseHP", RpcTarget.All, collision.transform.GetComponent<PlayerStatHandler>().HP.total * 0.15f);
    }
    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / enemySO.hp;
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
        SetStateColor();
        currentHP -= damage;
        GaugeUpdate();
        if (currentHP <= 0)
        {
            //플레이어의 뷰 아이디 여깄어요
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
        _bullet.targets["Player"] = (int)BulletTarget.Player;

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

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.yellow);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.yellow);

        FindPlayer(rightBoundary, leftBoundary);
    }


    //추적, 공격시 플레이어를 바라보는 시야각으로 전환
    private void ChaseView()
    {
        if (currentTarget == null)
        {
            isAttaking = false;
            return;
        }


        Vector2 directionToTarget = (currentTarget.position - transform.position).normalized;


        Vector2 rightBoundary = Quaternion.Euler(0, 0, -viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.black);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.black);

        FindPlayer(rightBoundary, leftBoundary);
    }


    private void FindPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (!PhotonNetwork.IsMasterClient || currentTarget != null)
            return;
        //viewDistance > Vector2.Distance(playerTransform.position, transform.position


        //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);


        //받아온 모든 플레이어 트랜스폼을 받아온다.
        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            if (viewDistance >= Vector2.Distance(PlayersTransform[i].position, transform.position))
            {
                //시야각 방향의 직선 Direction
                Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;
                //Enemy와 Player 사이의 방향
                Vector2 directionToPlayer = (PlayersTransform[i].position - transform.position).normalized;

                float angle = Vector3.Angle(directionToPlayer, middleDirection);
                if (angle < viewAngle * 0.5f)
                {
                    currentTarget = PlayersTransform[i]; // currentTargetPlayer
                    Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);

                    break;
                }
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

    #region player애니메이션 관련    

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
        Debug.Log($"{animName}이 {set} 상태로 호출됨");
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
        EnemyState_Dead_DeadCondition deadCondition = new EnemyState_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        EnemyState_Dead state_Dead = new EnemyState_Dead(gameObject);
        BTDead.AddChild(state_Dead);

        //상태이상 체크 [스턴....]
        BTSquence BTAbnormal = new BTSquence();
        EnemyState_GroggyCondition groggyConditon = new EnemyState_GroggyCondition(gameObject);
        BTAbnormal.AddChild(groggyConditon);

        BTMainSelector.AddChild(BTAbnormal);

        //추적+공격
        //컨디션 체크 -> 플레이어 추적 & 플레이어가 공격 범위 내 -> 공격(성공 반환 후 최초로)
        BTSquence BTChase = new BTSquence();

        EnemyState_Chase_ChaseCondition chaseCondition = new EnemyState_Chase_ChaseCondition(gameObject);
        BTChase.AddChild(chaseCondition);
        EnemyState_Chase state_Chase = new EnemyState_Chase(gameObject);
        BTChase.AddChild(state_Chase);

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
    #endregion
}
