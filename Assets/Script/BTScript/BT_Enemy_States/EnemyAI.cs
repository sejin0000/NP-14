using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using myBehaviourTree;
using static UnityEditor.PlayerSettings;
using UnityEditor.Rendering.LookDev;
using Photon.Pun;
using UnityEngine.UI;

//[Root 노드] => 왜 액션과 다르게 상속 안받음?
//==>특정 AI 동작과 상태에 맞게 유연하게 조정하기 위해서
//대신 BTRoot 객체를 생성하고, 그 아래에 복합 노드와 액션 노드를 추가해서 트리를 구성함
//EnemyAI : AI의 상태와 동작의 구조를 정의하고, 시작함


//Enemy에 필요한 컴포넌트들 + 기타 요소들 여기에 다 추가
public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // 현재 체력 계산
     

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public EnemySO enemySO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D collider2D;
    public Animator anim;  
    public GameObject target;                //추적 타겟[Palyer]
    public Collider2D targetColl;
    public NavMeshAgent nav;

    public GameObject enemyAim;
    public GameObject enemyBullet;

    public float viewAngle;                  // 시야각 (기본120도)
    public float viewDistance;               // 시야 거리 (기본 10)
    public LayerMask targetMask;             // 타겟 레이어(Player)

    public bool isLive;
    public bool isChase;
    public bool isAttaking;

    
    [SerializeField]
    private Image images_Gauge;              //몬스터 UI : Status

    
    public PhotonView PV;                    //동기화


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponentInChildren<CircleCollider2D>();
        PV = GetComponent<PhotonView>();

        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeATState();
        currentHP = enemySO.hp;
        isLive = true;

        nav.updateRotation = false;
        nav.updateUpAxis = false;

        //★싱글 테스트 시 if else 주석처리 할것
        
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
        //AI트리의 노드 상태를 매 프레임 마다 얻어옴
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


    //★
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //호스트에서만 충돌 처리됨
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (collision.gameObject.tag == "Bullet")
        {
            isChase = true;

            //모든 플레이어에게 현재 적의 체력 동기화
            PV.RPC("DecreaseHP", RpcTarget.AllBuffered, collision.transform.GetComponent<Bullet>().ATK);

            Debug.Log("현재 체력 :" + currentHP);
            //TODO게이지 이미지에 hp수치 적용
        }
    }



    private void GaugeUpdate()
    {
        images_Gauge.fillAmount = (float)currentHP / enemySO.hp; //체력
    }

        //★
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
        // 현재 오브젝트의 회전값을 고려하여 방향 벡터를 계산

        // 현재 오브젝트의 회전값 + 지정 각도값 => 이 값을 라디안으로 변환
        float radAngle = (transform.eulerAngles.z + angle) * Mathf.Deg2Rad;

        // 벡터를 radAngle값을 x,y 방향으로 계산하여 2D 벡터로 반환
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle));
    }


    //플레이어 탐지★ 여기서 추적시&공격시 시야각도 지정하자
    private void NomalView()
    {
        Vector2 rightBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector2 leftBoundary = BoundaryAngle(viewAngle * 0.5f);

        // 스프라이트 랜더러가 flipX 상태이면 레이 방향을 반대로 설정
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

            //시야각 방향의 직선 Direction
            Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;

            //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);

            //Enemy와 Player 사이의 방향
            Vector2 directionToPlayer = (targetColl.transform.position - transform.position).normalized;

            //플레이어 시야 중앙~타겟위치 사이의 각도
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
            nav.isStopped = false; // 활성화
    }


    //행동 트리 실제 생성
    void CreateTreeATState()
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
        EnemyState_Attack state_Attack = new EnemyState_Attack(gameObject);
        BTChase.AddChild(state_Attack);


        


        //순찰(시퀀스 : 하나라도 실패하면 실패반환)
        //할거 없으면 이동
        BTSquence BTPatrol = new BTSquence();

        EnemyState_Patrol state_Patrol = new EnemyState_Patrol(gameObject);
        BTPatrol.AddChild(state_Patrol);


        //사망(상시 체크)

        //셀렉터는 우선순위 높은 순서로 배치 : 생존 여부 -> 특수 패턴 -> 플레이어 체크(공격 여부) -> 이동 여부 순서로 셀렉터 배치 
        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함) 

        BTMainSelector.AddChild(BTDead);
        BTMainSelector.AddChild(BTChase);
        BTMainSelector.AddChild(BTPatrol);

        //작업이 끝난 Selector를 루트 노드에 붙이기
        TreeAIState.AddChild(BTMainSelector);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            Debug.Log("위치 데이터 전송");
        }
        else if (stream.IsReading)
        {
            // 데이터를 수신
            transform.position = (Vector2)stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
