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

//[Root 노드] => 왜 액션과 다르게 상속 안받음?
//==>특정 AI 동작과 상태에 맞게 유연하게 조정하기 위해서
//대신 BTRoot 객체를 생성하고, 그 아래에 복합 노드와 액션 노드를 추가해서 트리를 구성함
//EnemyAI : AI의 상태와 동작의 구조를 정의하고, 시작함


//Enemy에 필요한 컴포넌트들 + 기타 요소들 여기에 다 추가
public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // 현재 체력 계산
    public float viewAngle;                  // 시야각 (기본120도)
    public float viewDistance;               // 시야 거리 (기본 10)

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public EnemySO enemySO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D collider2D;
    public Animator anim;

    private Collider2D target;
    public Collider2D Target { get { return target; } 
                               set { if (target != value) { OnTargetChaged(value); target = value; } } }//추적 타겟[Palyer]
    //public Collider2D target;
    public NavMeshAgent nav;
    public Vector3 navTargetPoint;              //nav 목적지


    public GameObject enemyAim;
    public Bullet enemyBulletPrefab;


    public LayerMask targetMask;             // 타겟 레이어(Player)

    public float SpeedCoefficient = 1f;      // 이동속도 계수
   
    public bool isLive;
    public bool isChase;
    public bool isAttaking;


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


    //넉백
    private bool isKnockback = false;
    private Vector2 knockbackStartPosition;
    private Vector2 knockbackTargetPosition;
    private float knockbackStartTime;
    public float knockbackDuration = 0.2f;

    void Awake()
    {

        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponent<CircleCollider2D>();
        PV = GetComponent<PhotonView>();

        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeAIState();
        currentHP = enemySO.hp;
        viewAngle = enemySO.viewAngle;
        viewDistance = enemySO.viewDistance;
        isLive = true;

        nav.updateRotation = false;
        nav.updateUpAxis = false;

        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?

        nowEnemyPosition = this.gameObject.transform.position;


        nav.speed = enemySO.enemyMoveSpeed;
        navTargetPoint = transform.position;


        //호스트만 nav활성화 하도록 설정
        if (!PhotonNetwork.IsMasterClient)
            nav.enabled = false;
        else
            nav.enabled = true;
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
            NomalView();


        // 넉백 중인 경우
        if (isKnockback)
        {
            // 넉백 시간 비율 계산
            float knockbackRatio = (Time.time - knockbackStartTime) / knockbackDuration;

            // Lerp를 사용하여 현재 위치를 부드럽게 이동
            transform.position = Vector2.Lerp(knockbackStartPosition, knockbackTargetPosition, knockbackRatio);

            // 넉백이 끝났는지 확인
            if (knockbackRatio >= 0.3f)
            {
                isKnockback = false;
            }
        }


        if (!IsNavAbled() || nav.remainingDistance < 0.2f)
        {
            anim.SetBool("isWalk", false);
            anim.SetBool("isUpWalk", false);
            return;
        }          

        if (navTargetPoint.y > transform.position.y)
        {
            anim.SetBool("isUpWalk", true);
            anim.SetBool("isWalk", false);
        }
        else
        {
            anim.SetBool("isWalk", true);
            anim.SetBool("isUpWalk", false);
        }
    }


    //#####Enemy 이동 속도변경 관련######
    public void ChangeSpeed(float statSpeed)
    {
        nav.speed = statSpeed * SpeedCoefficient;
    }


    //#####Enemy 피격, 사망, 넉백 관련######

    //★맞음 & 죽음
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //호스트에서만 충돌 처리됨
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
            //모든 플레이어에게 현재 적의 체력 동기화
            PV.RPC("DecreaseHP", RpcTarget.All, atk);


            //넉백
            Vector2 directionToBullet = (collision.transform.position - transform.position).normalized;

            // 넉백을 위한 거리 조절
            float knockbackDistance = 2.0f;

            // 넉백 시작 위치와 목표 위치 계산
            knockbackStartPosition = transform.position;
            knockbackTargetPosition = knockbackStartPosition - directionToBullet * knockbackDistance;

            // 넉백 시작 시간 저장
            knockbackStartTime = Time.time;

            // 넉백 플래그 설정
            isKnockback = true;
            if (!playerBullet.Penetrate) 
            {
                Destroy(collision.gameObject);
            }
        }
    }

    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / enemySO.hp; //체력
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

        }
            DieCheck(playerid);
    }
    public void DieCheck(int playerid) 
    {

            isLive = false;
    }

    [PunRPC]
    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    //#####공격 관련######
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


    //#####시야각(타겟 서치) 관련######
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
        if (Target == null)
        {           
            isAttaking = false;
            isChase = false;
            return;
        }


        Vector2 directionToTarget = (Target.transform.position - transform.position).normalized;


        Vector2 rightBoundary = Quaternion.Euler(0, 0,-viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.black);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.black);

        FindPlayer(rightBoundary, leftBoundary);
    }

    private void FindPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (PhotonNetwork.IsMasterClient && Target == null)
        {

            Target = Physics2D.OverlapCircle(transform.position, viewDistance, targetMask);
            Debug.Log($"타겟 수집{Target}");
        }           

        //플레이어를 관리하는 객체에게 타겟의 위치를 요청하고, 내가 원하는범위안의 플레이어들의 리스트를 요청

        if (Target == null)
            return;


        if (Target.tag == "Player")
        {

            //시야각 방향의 직선 Direction
            Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;

            //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);

            //Enemy와 Player 사이의 방향
            Vector2 directionToPlayer = (Target.transform.position - transform.position).normalized;

            //플레이어 시야 중앙~타겟위치 사이의 각도
            float angle = Vector3.Angle(directionToPlayer, middleDirection);

            if (angle < viewAngle * 0.5f)
            {
                isChase = true;

                Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
            }
        }
    }

    //#####타겟 관련######
    private void OnTargetChaged(Collider2D _target)
    {
        //마스터 클라이언트가 몬스터를 소환하고, 해당 몬스터들이
        if(PhotonNetwork.IsMasterClient)
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
        Target = targetPV.gameObject.GetComponent<Collider2D>();
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



    //#####플레이어 애니메이션 관련######
    
    /*
    private void SetAnim(string animName, bool set)
    {
        if (PV.IsMine == false)
            return;

        anim.SetBool(animName, set);
        PV.RPC(nameof(SyncAnimation), RpcTarget.All, animName, set);
    }

    public void SyncAnimation(string animName, bool set)
    {
        anim.SetBool(animName, set);
    }
    */


    //#####NAV관련######
    public void DestinationSet()
    {
        if (!isAttaking || isLive)
        {
            nav.SetDestination(navTargetPoint);
        }

        if (navTargetPoint.x <= transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
            
        else
        {
            spriteRenderer.flipX = false;
        }
            
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
            nav.isStopped = false; // 활성화
            return true;
        }           
    }


    //#####BT######
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
    

    //#####동기화######
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송
            stream.SendNext(hostPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(spriteRenderer.flipX); // 이게 맞나?
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
 
}
