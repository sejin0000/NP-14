using myBehaviourTree;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

enum BossStateColor
{
    ColorRed,
    ColorYellow,
    ColorBlue,
    ColorBlack,
    ColorOrigin,
    ColorMagenta,
}

enum PatternArea
{
    RightArea,
    LeftArea,
    TwoSideArea,
    AllArea,
    TargetCircleArea,
    TriangleArea,
}
public class BossAI_Dragon : MonoBehaviourPunCallbacks, IPunObservable
{
    private BTRoot TreeAIState;

    public float currentHP;                  // 현재 체력 계산
    public float viewAngle;                  // 시야각 (기본120도)
    public float viewDistance;               // 시야 거리 (기본 10)

    //컴포넌트 및 기타 외부요소(일부 할당은 하위 노드에서 진행)
    public EnemySO bossSO;                  // Enemy 정보 [모든 Action Node에 owner로 획득시킴]
    public SpriteRenderer[] spriteRenderers;
    public Animator anim;

    public GameObject Show_AttackArea;
    public Collider2D[] AreaList;

    private Transform target;
    public Transform currentTarget
    {
        get { return target; }
        set { if (target != value) { OnTargetChaged(value); target = value; } }
    }
    //public Collider2D target;
    public List<Transform> PlayersTransform;

    //공격 범위 안에 들어온 플레이어 리스트
    public List<PlayerStatHandler> inToAreaPlayers = new List<PlayerStatHandler>();


    public Bullet enemyBulletPrefab;
    public Transform bossHead;
    public Transform bossAim;
    public Color originColor;

    public LayerMask targetMask;             // 타겟 레이어(Player)

    public float SpeedCoefficient = 1f;      // 이동속도 계수


    public bool isLive;
    public bool isAttaking;
    public bool isGroggy;

    //플레이어 정보

    public int lastAttackPlayer;

    //게임매니저에서(어디든) 관리하는 플레이어들 정보를 요청해서 사용

    //가장많은 피해를 준 플레이어 타겟-> 불렛(쏜사람 정보) 맞은놈만 알면됨 ->플레이 공격력->



    [SerializeField]
    private Image images_Gauge;              //몬스터 UI : Status



    //동기화
    public PhotonView PV;
    private Vector3 hostPosition;
    private Quaternion hostRotation;
    public float lerpSpeed = 10f; // 보간시 필요한 수치(조정 필요)




    //객체별 넉백거리
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        AreaList = Show_AttackArea.GetComponentsInChildren<Collider2D>(true); //비활성화된 오브젝트도 탐색<>(true)

        //게임 오브젝트 활성화 시, 행동 트리 생성
        CreateTreeAIState();
        currentHP = bossSO.hp;
        viewAngle = bossSO.viewAngle;
        viewDistance = bossSO.viewDistance;
        isLive = true;

        originColor = spriteRenderers[0].color;

        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?


        knockbackDistance = 0f;


        //TODO 생성할 때, 모든 플레이어 Transform 정보를 담는다.
        foreach (var _value in TestGameManagerWooMin.Instance.playerInfoDictionary.Values)
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
            //$추가됨 : 동기화된 머리 위치에 대한 보간 처리
            transform.position = Vector3.Lerp(transform.position, hostPosition, Time.deltaTime * lerpSpeed);
            bossHead.transform.rotation = Quaternion.Slerp(bossHead.transform.rotation, hostRotation, Time.deltaTime * lerpSpeed);
            return;
        }

        
        for (int i = 0; i < inToAreaPlayers.Count; i++)
        {
            Debug.Log($"리스트 안에 들어온 플레이어{inToAreaPlayers[i]}");           
        }
        Debug.Log($"리스트 개수{inToAreaPlayers.Count} 개");
        


        hostPosition = transform.position;
        hostRotation = bossHead.transform.rotation;

        //AI트리의 노드 상태를 매 프레임 마다 얻어옴
        TreeAIState.Tick();

        if (!isLive)
            return;


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
        PV.RPC("SetStateColor", RpcTarget.All, Color.red);
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
        var _bullet = Instantiate(enemyBulletPrefab, bossAim.transform.position, bossAim.transform.rotation);

        _bullet.IsDamage = true;
        _bullet.ATK = bossSO.atk;
        _bullet.BulletLifeTime = bossSO.bulletLifeTime;
        _bullet.BulletSpeed = bossSO.bulletSpeed;
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


    //특수 패턴 시, 대상 플레이어를 바라보도록 설정
    private void ChaseView()
    {
        if (currentTarget == null)
        {
            isAttaking = false;
            return;
        }

        //머리와 타겟의 방향
        Vector2 directionToTarget = (currentTarget.position - bossHead.transform.position).normalized;

        //브레스 범위
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -viewAngle * 0.5f) * directionToTarget;
        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * directionToTarget;

        Debug.DrawRay(transform.position, rightBoundary * viewDistance, Color.red);
        Debug.DrawRay(transform.position, leftBoundary * viewDistance, Color.red);

        LookPlayer(rightBoundary, leftBoundary);
    }

    //특수 패턴 시, 대상 플레이어를 바라보도록 설정
    private void LookPlayer(Vector2 _rightBoundary, Vector2 _leftBoundary)
    {
        if (!PhotonNetwork.IsMasterClient || currentTarget != null)
            return;
        //viewDistance > Vector2.Distance(playerTransform.position, transform.position


        //Debug.DrawRay(transform.position, middleDirection * viewDistance, Color.green);


        //시야각 방향의 직선 Direction
        Vector2 middleDirection = (_rightBoundary + _leftBoundary).normalized;
        //Enemy와 Player 사이의 방향
        Vector2 directionToPlayer = (currentTarget.position - transform.position).normalized;

        float angle = Vector3.Angle(directionToPlayer, middleDirection);
        if (angle < viewAngle * 0.5f)
        {
            Debug.DrawRay(transform.position, directionToPlayer * viewDistance, Color.red);
        }

    }
    #endregion

    #region 실제 액션[브레스]

    public void Breath()
    {
    }

    //양팔 공격은 [두 메서드 동시에 실행함]
    public void RightArmAttack()
    {

    }
    public void LeftArmAttack() 
    {

    }

    [PunRPC]
    public void ActiveAttackArea(int patternNum)
    {
        //고정 범위->다 차오르면 스프라이트 내부에 있는 플레이어에게 데미지
        //원형 범위->타겟 플레이어를 추적하다 다 차오르기 직전에 정지 -> 다 차오르면 해당 위치에 낙석 오브젝트 생성
        switch(patternNum)
        {
            case 0:
                AreaList[0].gameObject.SetActive(true); //우측 공격영역 (좌측 팔)
                SetAnim("isLeftAttack", true); //트리거로 하려고 했으나 팀원이 에러가 가끔 생긴다고 해서 bool로 animSet
                StartCoroutine(SetAnimFalse("isLeftAttack"));
                //이 부분에 플레이어 피해&넉백 판정주는 코루틴 실행
                StartCoroutine(TryAttackAreaTargets(0));

                //공격 이후
                StartCoroutine(LaterInActiveAttackArea(0));
                break;
            case 1:
                AreaList[1].gameObject.SetActive(true); ; //좌측 공격영역 (우측 팔)
                SetAnim("isRightAttack", true);
                StartCoroutine(SetAnimFalse("isRightAttack"));
                StartCoroutine(TryAttackAreaTargets(1));


                StartCoroutine(LaterInActiveAttackArea(1));
                break;
            case 2:
                AreaList[0].gameObject.SetActive(true); ; // 좌측 우측 동시 실행             
                AreaList[1].gameObject.SetActive(true); ;
                SetAnim("isTwoArmAttack", true);
                StartCoroutine(SetAnimFalse("isTwoArmAttack"));

                // 각 Area에 대해 처리
                StartCoroutine(TryAttackAreaTargets(2));

                StartCoroutine(LaterInActiveAttackArea(2)); // 2 : 둘 다 꺼짐
                break;
            case 3:
                AreaList[2].gameObject.SetActive(true); ; // 모든 범위 실행
                StartCoroutine(TryAttackAreaTargets(2));


                StartCoroutine(LaterInActiveAttackArea(3));               
                break;
            case 4:
                AreaList[3].gameObject.SetActive(true); ; // 타겟 플레이어에 원형 실행
                StartCoroutine(ChaseArea());
                StartCoroutine(TryAttackAreaTargets(3));               

                StartCoroutine(LaterInActiveAttackArea(4));             
                break;
            case 5:
                // 모든 플레이어에 원형 실행(3,4,5)
                break;
            case 6:
                AreaList[6].gameObject.SetActive(true); ; // 브레스 범위 표시(방법이 상이함)
                StartCoroutine(TryAttackAreaTargets(4));

                StartCoroutine(LaterInActiveAttackArea(6));                
                break; 
        }
    }

    public void InActiveAttackArea(int patternNum)
    {
        switch (patternNum)
        {
            case 0:
                AreaList[0].gameObject.SetActive(false); //좌측 팔
                break;
            case 1:
                AreaList[1].gameObject.SetActive(false); //우측 팔
                break;
            case 2:
                AreaList[0].gameObject.SetActive(false); // 좌측 우측 동시 실행
                AreaList[1].gameObject.SetActive(false);                
                break;
            case 3:
                AreaList[2].gameObject.SetActive(false); // 모든 범위 실행
                break;
            case 4:
                AreaList[3].gameObject.SetActive(false); // 타겟 플레이어에 원형 실행
                break;
            case 5:
                ;//모든 플레이어를 추적하는 원형(3,4,5)
                break;
            case 6:
                AreaList[6].gameObject.SetActive(false); // 브레스 범위 표시(방법이 상이함)
                break;
        }
    }


    //공격 범위 UI 종료용 코루틴(여기서 예고 레이어 타격 영역 => 일반 영역으로 교체)
    public IEnumerator LaterInActiveAttackArea(int patternNum)
    {
        yield return new WaitForSeconds(3f);       
        InActiveAttackArea(patternNum);
    }

    //애니메이션 종료용 코루틴(여기서 예고 레이어 일반 영역 => 타격 영역으로 교체)
    IEnumerator SetAnimFalse(string boolName)
    {
        yield return new WaitForSeconds(0.5f);

        SetAnim(boolName, false);
    }


    //실제 공격 판정 타이밍
    IEnumerator TryAttackAreaTargets(int areaIndex)
    {
        yield return new WaitForSeconds(2.0f);
        AttackTargetsInArea(areaIndex);
    }

    //진짜 진짜 공격&넉백임
    public void AttackTargetsInArea(int areaIndex)
    {
        if (inToAreaPlayers == null || areaIndex < 0 || areaIndex >= AreaList.Length)
            return;

        Transform attackAreaTransform = AreaList[areaIndex].transform;

        for (int i = 0; i < inToAreaPlayers.Count; i++)
        {

            PlayerStatHandler player = inToAreaPlayers[i];

            // 플레이어와 공격 영역의 방향 벡터
            Vector3 playerPosition = player.transform.position;
            Vector3 areaPosition = attackAreaTransform.position;

            if (areaIndex == 2)
                areaPosition = new Vector3(0, 5, 0);

            Vector3 directionToPlayer = (playerPosition - areaPosition).normalized;


            // 넉백거리
            float knockbackDistance = 1.5f;

            // 실제 넉백

            StartCoroutine(player.Knockback(directionToPlayer, knockbackDistance));

            // 실제 피해
            player.GiveDamege(bossSO.atk);


        }
    }

    //일정 시간 이후에 멈추도록(추적 속도를 0으로 변경)
    IEnumerator ChaseArea()
    {
        if (AreaList[3].gameObject.activeSelf)
        {
            // 추적 시작 위치 저장
            Vector3 areaStartPosition = currentTarget.position;

            AreaList[3].transform.position = areaStartPosition;

            // 일정 시간 동안 천천히 타겟을 쫓음
            float elapsedTime = 0f;
            float chaseDuration = 3.5f; // 추적 시간

            while (elapsedTime < chaseDuration)
            {
                // 타겟 쪽으로 천천히 이동
                AreaList[3].transform.position = Vector3.Lerp(areaStartPosition, currentTarget.position, elapsedTime / chaseDuration);

                elapsedTime += Time.deltaTime;
                yield return null; // 한 프레임 대기
            }
        }

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

    private void SetColor(int colorNum)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            switch (colorNum)
            {
                case (int)BossStateColor.ColorRed:
                    spriteRenderers[i].color = Color.red;
                    break;
                case (int)BossStateColor.ColorYellow:
                    spriteRenderers[i].color = Color.yellow;
                    break;
                case (int)BossStateColor.ColorBlue:
                    spriteRenderers[i].color = Color.blue;
                    break;
                case (int)BossStateColor.ColorBlack:
                    spriteRenderers[i].color = Color.black;
                    break;
                case (int)BossStateColor.ColorOrigin:
                    spriteRenderers[i].color = originColor;
                    break;
                case (int)BossStateColor.ColorMagenta:
                    spriteRenderers[i].color = Color.magenta;
                    break;
            }
        }
    }

    [PunRPC]
    public void SetStateColor(Color _color)
    {
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = _color;
        }
    }
    #endregion

    #region 애니메이션 관련

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


        /*
        //Enemy 생존 체크
        //컨디션 체크 -> 사망 시 필요한 액션들(오브젝트 제거....)
        BTSquence BTDead = new BTSquence();
        EnemyState_Dead_DeadCondition deadCondition = new EnemyState_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        EnemyState_Dead state_Dead = new EnemyState_Dead(gameObject);
        BTDead.AddChild(state_Dead);
        */


        /*
        //상태이상 체크 [스턴....]
        BTSquence BTAbnormal = new BTSquence();
        EnemyState_GroggyCondition groggyConditon = new EnemyState_GroggyCondition(gameObject);
        BTAbnormal.AddChild(groggyConditon);
        */


        //페이즈 판별 <셀렉터 겸 컨디션>[기본 = 1페이즈  ||  체력 50% '미만' = 2페이즈]
        BTSelector Phase_One = new BTSelector();
        //페이즈 컨디션


        BossAI_State_SpecialAttack specialAttack = new BossAI_State_SpecialAttack(gameObject);
        //패턴 컨디션 -> 컨디션
        Phase_One.AddChild(specialAttack);

        //여기에서 노말액션 시퀀스에 사용할 랜덤 난수  쏴주기
        BTSelector nomalAttack_Selector = new BTSelector();
        //패턴 컨디션 -> 컨디션
        Phase_One.AddChild(nomalAttack_Selector);


        BTSquence nomalAttack_Squence_1 = new BTSquence();
        //노말 패턴 시퀀스의 컨디션 1
        //실제 노말 패턴 1
        //실제 노말 패턴 2
        //예시
        //BossAI_State_NomalAttackSequence_1 nomalAttack_Sequence_1 = new BossAI_State_NomalAttackSequence_1(gameObject);
        //nomalAttack_Squence_1.AddChild(액션노드 변수명);
        BTSquence nomalAttack_Squence_2 = new BTSquence();
        //노말 패턴 시퀀스의 컨디션 2
        //실제 노말 패턴 1
        //실제 노말 패턴 2
        //실제 노말 패턴 3
        //예시
        //BossAI_State_NomalAttackSequence_2 nomalAttack_Sequence_2 = new BossAI_State_NomalAttackSequence_2(gameObject);
        //nomalAttack_Squence_2.AddChild(액션노드 변수명);
        BTSquence nomalAttack_Squence_3 = new BTSquence();
        //노말 패턴 시퀀스의 컨디션 3
        //실제 노말 패턴 1
        //실제 노말 패턴 2
        //실제 노말 패턴 3
        //실제 노말 패턴 4
        //예시
        //BossAI_State_NomalAttackSequence3 nomalAttack_Sequence_3 = new BossAI_State_NomalAttackSequence_2(gameObject);
        //nomalAttack_Squence_3.AddChild(액션노드 변수명);

        nomalAttack_Selector.AddChild(nomalAttack_Squence_1);
        nomalAttack_Selector.AddChild(nomalAttack_Squence_2);
        nomalAttack_Selector.AddChild(nomalAttack_Squence_3);

        // 예를 들어:
        // EnemyState_Action1 action1 = new EnemyState_Action1(gameObject);
        // EnemyState_Action2 action2 = new EnemyState_Action2(gameObject);
        // specialAttackSequence.AddChild(action1);
        // specialAttackSequence.AddChild(action2);


        BTSelector Phase_Two = new BTSelector();
        








        //셀렉터는 우선순위 높은 순서로 배치 : 생존 여부 -> 특수 패턴 -> 플레이어 체크(공격 여부) -> 이동 여부 순서로 셀렉터 배치 
        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함) 

        //BTMainSelector.AddChild(BTDead);
        //BTMainSelector.AddChild(BTAbnormal);

        //메인(페이즈) 셀렉터
        BTMainSelector.AddChild(Phase_One);
        BTMainSelector.AddChild(Phase_Two);

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
            stream.SendNext(hostRotation);

        }
        else if (stream.IsReading)
        {
            // 데이터를 수신
            hostPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            hostRotation = (Quaternion)stream.ReceiveNext();
        }

    }

    #endregion
}
