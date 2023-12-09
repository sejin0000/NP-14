using JetBrains.Annotations;
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
    BreathArea,
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
    public ParticleSystem BreathParticleObject;

    public GameObject Show_AttackArea;
    public Collider2D[] AreaList;

    private Transform target;
    public Transform currentTarget
    {
        get { return target; }
        set { if (target != value) { OnTargetChaged(value); target = value; } }
    }
    public LayerMask breathTargetLayer;

    //public Collider2D target;
    public List<Transform> PlayersTransform;

    //공격 범위 안에 들어온 플레이어 리스트
    public List<PlayerStatHandler> inToAreaPlayers = new List<PlayerStatHandler>();


    public Bullet enemyBulletPrefab;
    public Transform bossHead;
    public Transform bossAim;
    public Collider2D[] bossHitBox;
    public Color originColor;

    public LayerMask targetMask;             // 타겟 레이어(Player)

    public float SpeedCoefficient = 1f;      // 이동속도 계수
    public float breathAttackDelay;
    public float currentTime;

    public bool isLive = true;
    public bool isAttaking = false;
    public bool isGroggy = false;
    public bool isBreathInProgress = false; // 브레스 실행여부 판별, 중복실행 방지
    public bool isRunningBreath = false;    // 브레스 발사중
    public bool isTrackingFurthestTarget = false;
    //플레이어 정보

    public int lastAttackPlayer;
    public int currentNomalAttackSquence; // 노말어택 시퀀스 랜덤 난수(0~3) : 3종

    //게임매니저에서(어디든) 관리하는 플레이어들 정보를 요청해서 사용

    //가장많은 피해를 준 플레이어 타겟-> 불렛(쏜사람 정보) 맞은놈만 알면됨 ->플레이 공격력->



    [SerializeField]
    private Image image_Gauge;              //몬스터 UI : Status
    [SerializeField]
    private TextMeshProUGUI txt_Gauge;


    //동기화
    public PhotonView PV;
    private Vector3 hostAimPosition;
    private Quaternion hostRotation;
    private Vector2 hostKnckbackPosition;
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
        breathAttackDelay = bossSO.breathAttackDelay;
        currentTime = breathAttackDelay;
        isLive = true;

        originColor = spriteRenderers[0].color;

        //★싱글 테스트 시 if else 주석처리 할것
        //쫓는 플레이어도 호스트가 판별?


        knockbackDistance = 0f;


        //TODO 생성할 때, 모든 플레이어 Transform 정보를 담는다.TestGameManagerWooMin
        if (TestGameManager.Instance != null)
        {
            //생성할 때, 모든 플레이어 Transform 정보를 담는다.
            foreach (var _value in TestGameManager.Instance.playerInfoDictionary.Values)
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

        currentNomalAttackSquence = Random.Range(0, 3);

        //생성 시 랜덤 타겟 지정
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget]; 
    }

    void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            //$추가됨 : 동기화된 머리 위치에 대한 보간 처리
            bossHead.transform.rotation = Quaternion.Slerp(bossHead.transform.rotation, hostRotation, Time.deltaTime * lerpSpeed);

            AreaList[6].transform.position = hostAimPosition;
            AreaList[6].transform.rotation = Quaternion.Slerp(bossHead.transform.rotation, hostRotation, Time.deltaTime * lerpSpeed);
            return;
        }
        //AI트리의 노드 상태를 매 프레임 마다 얻어옴


        TreeAIState.Tick();

        if (!isLive)
            return;


        if (!isTrackingFurthestTarget)
            SetNearestTarget();


        SetHead();

        hostAimPosition = bossAim.transform.position;
        hostRotation = bossHead.transform.rotation;
        



        if (isBreathInProgress)
        {
            AreaList[6].transform.position = bossAim.transform.position;
            AreaList[6].transform.rotation = bossHead.transform.rotation;

            if (!isRunningBreath || inToAreaPlayers.Count == 0)
                return;

            UpdateBreath();
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
    private void GaugeUpdate()
    {
        image_Gauge.fillAmount = (float)currentHP / bossSO.hp;
        txt_Gauge.text = currentHP + " / " + bossSO.hp; // 현재 체력 / 최대 체력 표시
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

    [PunRPC]
    public void Fire()
    {
        int numBullets = 5; // 부채꼴 내의 총알 수를 조절하세요
        float AttackAngle = 120f; // 부채꼴의 각도를 조절하세요

        float startAngle = -AttackAngle / 2f; // 부채꼴의 시작 각도 -60 == -60 ~ 120 즉 180도의 범위를 커버한다.

        for (int i = 0; i < numBullets; i++)
        {
            //-60 + 0 * (120/4) = 0 || -60 + 1 * (120/4)
            float angle = startAngle + i * (AttackAngle / (numBullets - 1));
            Quaternion bulletRotation = bossHead.transform.rotation * Quaternion.Euler(0f, 0f, angle - 90f);

            var _bullet = Instantiate(enemyBulletPrefab, bossAim.transform.position, bulletRotation);

            _bullet.IsDamage = true;
            _bullet.ATK = bossSO.atk;
            _bullet.BulletLifeTime = bossSO.bulletLifeTime;
            _bullet.BulletSpeed = bossSO.bulletSpeed;
            _bullet.targets["Player"] = (int)BulletTarget.Player;
        }
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

    #region 실제 액션[브레스]

    public void Breath()
    {

        //과정 1 완료        
        //브레스 상태 변수를 통해 제어
        //브레스 워닝사인의 포지션을 용 머리에 y - n 만큼의 포지션 위치에 활성화 시킴 v
        //브레스 워닝사인의 로테이션 값도 동일하게 돌려준다. v (업데이트에서 보간 진행중)
        //범위 내의 플레이어는 워닝사인 내부에서 알아서 처리함 v
        //브레스 워닝사인의 경우는 다 차올라도 사라지지 않는다. v
        

        //브레스 파티클을 aim의 포지션에 활성화(혹은 생성) - 파티클을  아예 머리에 달아서 굳이 로테이션 값 조절이 필요없도록 하자 v
        //브레스는 콜리전 처리를 on 해서 트리거가 아닌 콜라이더에 부딫힐 경우, 튕겨나가도록 처리한다. v
        //브레스 파티클 자체는 플레이어와 부딫혀도 아무런 상호작용도 하지 않는다. v 

        //과정 2
        //워닝 사인이 다 차오른 경우 브레스 파티클을 실행한다. (2.0f 초 뒤에 모두 차오름)


        //브레스 진행 중 : 약간의 시간(0.1f ~0.2f) 이후, 어택 에어리어 내부 리스트에 있는 플레이어를 대상으로만 레이캐스트를 발사한다.
        //레이캐스트는 최초 hit 대상이 wall인 경우 리턴한다.
        //최초 hit 대상이 리스트 내에 있는 플레이어인 경우에는, 해당 플레이어를 대상으로 피해와 넉백을 주도록한다.
        //4~5초 후 파티클과 어택 에어리어를 동시에 비활성화 시킨다.

        //약간의 시간 이후이므로 


    }

    [PunRPC]
    public void StartBreathCoroutine()
    {
        StartCoroutine(StartBreath());
    }


    public IEnumerator StartBreath()
    {
        //브레스 중복실행 방지
        if (isBreathInProgress)
            yield break;

        isBreathInProgress = true;
        AreaList[6].gameObject.SetActive(true);
        SetAnim("isBreathAttack", true);
        //워닝 사인 차오르는거 대기 후 브레스 파티클을 플레이
        yield return new WaitForSeconds(2f);
        BreathParticleObject.Play();


        //여기다 브레스 업데이트용 bool
        isRunningBreath = true;

        //브레스 종료 시간 도달 (2+4f)
        yield return new WaitForSeconds(4f);
        isBreathInProgress = false;
        AreaList[6].gameObject.SetActive(false);
        SetAnim("isBreathAttack", false);
        BreathParticleObject.Stop();
        isRunningBreath = false;

        currentTime = breathAttackDelay;
    }

    public void UpdateBreath()
    {
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            Debug.Log("업데이트 브레스 들어옴");

            //에어리어 리스트에 사람이 하나라도 있다면?
            //리스트 내 모든 대상에게 레이캐스트 발사
            //업데이트하고 있지 않으므로 위에서 리턴 날 가능성이 큼 이 부분은 역시 업데이트 조지는게 맞음 아오 졸려죽겠다

            for (int i = 0; i < inToAreaPlayers.Count; i++)
            {
                Debug.Log("반복문 들어옴");
                // 플레이어의 위치
                PlayerStatHandler player = inToAreaPlayers[i];
                Vector3 playerPosition = player.transform.position;
                // 플레이어<->보스에임 방향 구하기
                Vector3 directionToPlayer = (playerPosition - bossAim.position).normalized;
                Debug.DrawLine(bossAim.position, player.transform.position, Color.white);

                // 레이 발사
                RaycastHit2D hit = Physics2D.Raycast(bossAim.position, directionToPlayer, Mathf.Infinity, breathTargetLayer);

                if (hit.transform.tag != "Wall" || inToAreaPlayers.Count != 0) // hit 대상이 있거나, 벽이 아닌 경우에만 작동
                {
                    if (hit.transform.tag == "Player")
                    {
                        //일정 주기로 피해를 주기

                        // 플레이어와 공격 영역의 방향 벡터


                        // 넉백거리
                        float knockbackDistance = 1f;


                        // 실제 피해
                        player.DirectDamage(bossSO.atk, PV.ViewID);
                        // 실제 넉백
                        player.photonView.RPC("StartKnockback", RpcTarget.All, directionToPlayer, knockbackDistance);
                        //StartCoroutine(player.Knockback(directionToPlayer, knockbackDistance));
                    
                        Debug.Log($"2나오면 안됨 : {inToAreaPlayers.Count} 데미지는 이만큼 받음 :{bossSO.atk}");
                        Debug.Log($"플레이어 현재 체력은 : {player.CurHP}");
                    }
                }
            }

            currentTime = breathAttackDelay;
        }
       
    }
    //양팔 공격은 [두 메서드 동시에 실행함]
    public void RightArmAttack()
    {

    }
    public void LeftArmAttack() 
    {

    }
    public void TwoHandAttack()
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
                LocalSetAnimFalse("isLeftAttack");
                //이 부분에 플레이어 피해&넉백 판정주는 코루틴 실행
                LocalTryAttackAreaTargets(0);

                //공격 이후
                LocalLaterInActiveAttackArea(0);
                break;
            case 1:
                AreaList[1].gameObject.SetActive(true); ; //좌측 공격영역 (우측 팔)
                SetAnim("isRightAttack", true);
                LocalSetAnimFalse("isRightAttack");
                LocalTryAttackAreaTargets(1);


                LocalLaterInActiveAttackArea(1);
                break;
            case 2:
                AreaList[0].gameObject.SetActive(true); ; // 좌측 우측 동시 실행 이거 어캐함?             
                AreaList[1].gameObject.SetActive(true); ;
                SetAnim("isTwoArmAttack", true);
                LocalSetAnimFalse("isTwoArmAttack");

                // 각 Area에 대해 처리
                LocalTryAttackAreaTargets(2);

                LocalLaterInActiveAttackArea(2);  // 2 : 둘 다 꺼짐
                break;
            case 3:
                AreaList[2].gameObject.SetActive(true); ; // 모든 범위 실행
                LocalTryAttackAreaTargets(2);


                LocalLaterInActiveAttackArea(3);
                break;
            case 4:
                AreaList[3].gameObject.SetActive(true); ; // 타겟 플레이어에 원형 실행
                LocalChaseArea();
                LocalTryAttackAreaTargets(3);

                LocalLaterInActiveAttackArea(4);
                break;
            case 5:
                // 모든 플레이어에 원형 실행(3,4,5)
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
                isTrackingFurthestTarget = false;
                break;
            case 5:
                ;//모든 플레이어를 추적하는 원형(3,4,5)
                break;
        }
    }


    //공격 범위 UI 종료용 코루틴(여기서 예고 레이어 타격 영역 => 일반 영역으로 교체)
    public IEnumerator LaterInActiveAttackArea(int patternNum)
    {
        yield return new WaitForSeconds(3f);       
        InActiveAttackArea(patternNum);
    }

    public void LocalLaterInActiveAttackArea(int patternNum)
    {
        StartCoroutine(LaterInActiveAttackArea(patternNum));
    }

    //애니메이션 종료용 코루틴(여기서 예고 레이어 일반 영역 => 타격 영역으로 교체)
    IEnumerator SetAnimFalse(string boolName)
    {
        yield return new WaitForSeconds(0.5f);

        SetAnim(boolName, false);
    }

    public void LocalSetAnimFalse(string boolName)
    {
        StartCoroutine(SetAnimFalse(boolName));
    }


    //실제 공격 판정 타이밍 (공격만 왜 메서드 3개냐 ㅋㅋㅋㅋ)
    IEnumerator TryAttackAreaTargets(int areaIndex, float attackCoefficient = 1)
    {
        yield return new WaitForSeconds(2.0f);
        AttackTargetsInArea(areaIndex);
    }
    public void LocalTryAttackAreaTargets(int areaIndex, float attackCoefficient = 1)
    {
        StartCoroutine(TryAttackAreaTargets(areaIndex));
    }


    //진짜 진짜 공격&넉백임
    public void AttackTargetsInArea(int areaIndex, float attackCoefficient = 1f)
    {
        if (inToAreaPlayers == null || areaIndex < 0 || areaIndex > AreaList.Length)
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
            player.DirectDamage(bossSO.atk * attackCoefficient, PV.ViewID);
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

    public void LocalChaseArea()
    {
        StartCoroutine(ChaseArea());
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

    public void SetHead() // 피해량, 플레이어 위치 받아옴
    {
        //플레이어를 바라보도록 설정

        //anim.SetTrigger("Attack"); // 공격 애니메이션

        //대상과 머리의 방향을 구한 뒤 해당 방향으로 Rotat
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
            //여기다 지진패턴[양 팔을 들어서 내려놓기] 넣어서 꼼수 대응
            return;
        }

        // 원하는 회전 범위 지정
        float minRotation = -25f;
        float maxRotation = 25f;
        //270~61 == 회전하면 안됨
        rotZ = Mathf.Clamp(rotZ, minRotation, maxRotation);


        // 현재 회전 각도
        Quaternion currentRotation = bossHead.transform.rotation;

        // 목표 회전 각도
        Quaternion targetRotation = Quaternion.Euler(0, 0, rotZ);

        // 회전 보간
        float interpolationFactor = 0.005f; // 보간 계수
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossHead.transform.rotation = interpolatedRotation;
    }

    private void ReturnOriginRotate()
    {
        // 목표 회전 각도
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        // 현재 회전 각도
        Quaternion currentRotation = bossHead.transform.rotation;

        // 회전 보간
        float interpolationFactor = 0.005f; // 보간 계수
        Quaternion interpolatedRotation = Quaternion.Slerp(currentRotation, targetRotation, interpolationFactor);


        bossHead.transform.rotation = interpolatedRotation;
    }

    private void SetNearestTarget()
    {

        float minDistance = float.MaxValue;


        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            Debug.Log($"타겟 인원 체크 {i}");
            if (PlayersTransform[i] == null)
            {
                Debug.Log($"타겟이 존재하지 않습니다. {i}");
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


        
        //Enemy 생존 체크
        //컨디션 체크 -> 사망 시 필요한 액션들(오브젝트 제거....)
        BTSquence BTDead = new BTSquence();
        BossAI_State_Dead_DeadCondition deadCondition = new BossAI_State_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        BossAI_State_Dead state_Dead = new BossAI_State_Dead(gameObject);
        BTDead.AddChild(state_Dead);




        //페이즈 판별 <셀렉터 겸 컨디션>[기본 = 1페이즈  ||  체력 50% '미만' = 2페이즈]
        BTSquence Phase_1 = new BTSquence();
        
        //BossAI_Phase_1_Condition phaseOneCondition = new BossAI_Phase_1_Condition(gameObject);
        //Phase_1.AddChild(phaseOneCondition);
        

        //액션 셀렉터

        BTSelector ActionSelector = new BTSelector();
        Phase_1.AddChild(ActionSelector);


        //BossAI_State_SpecialAttack specialAttack = new BossAI_State_SpecialAttack(gameObject);
        //ActionSelector.AddChild(specialAttack);

        //첫 노말패턴 시퀀스의 컨디션에서 노말액션 시퀀스에 사용할 랜덤 난수 쏴주기

        //1페이즈
        BTSelector nomalAttack_Selector = new BTSelector();
        ActionSelector.AddChild(nomalAttack_Selector);


        //노말 패턴 시퀀스1 번
        BTSquence nomalAttack_Squence_1 = new BTSquence();
        nomalAttack_Selector.AddChild(nomalAttack_Squence_1);

        BossAI_State_Choice_1_Condition nomalChoice_1 = new BossAI_State_Choice_1_Condition(gameObject);
        nomalAttack_Squence_1.AddChild(nomalChoice_1);



        BossAI_State_ChaseAttack chaseAttack1_1 = new BossAI_State_ChaseAttack(gameObject);
        nomalAttack_Squence_1.AddChild(chaseAttack1_1);
        BossAI_State_TwoWayAttack twoWayAttack_1 = new BossAI_State_TwoWayAttack(gameObject);
        nomalAttack_Squence_1.AddChild(twoWayAttack_1);
        BossAI_State_RightAttack rightAttack_1 = new BossAI_State_RightAttack(gameObject);
        nomalAttack_Squence_1.AddChild(rightAttack_1);
        BossAL_State_LeftAttack leftAttack_1 = new BossAL_State_LeftAttack(gameObject);
        nomalAttack_Squence_1.AddChild(leftAttack_1);


        //난수 세팅
        BossAI_SetRandomNum setRandomNum_1 = new BossAI_SetRandomNum(gameObject);
        nomalAttack_Squence_1.AddChild(setRandomNum_1);


        //노말 패턴 시퀀스2번
        BTSquence nomalAttack_Squence_2 = new BTSquence();
        nomalAttack_Selector.AddChild(nomalAttack_Squence_2);

        BossAI_State_Choice_2_Condition nomalChoice_2 = new BossAI_State_Choice_2_Condition(gameObject);
        nomalAttack_Squence_2.AddChild(nomalChoice_2);

        BossAI_State_Breath Breath_2 = new BossAI_State_Breath(gameObject);
        nomalAttack_Squence_2.AddChild(Breath_2);
        BossAI_State_TwoWayAttack twoWayAttack_2 = new BossAI_State_TwoWayAttack(gameObject);
        nomalAttack_Squence_2.AddChild(twoWayAttack_2);
        BossAL_State_LeftAttack leftAttack_2 = new BossAL_State_LeftAttack(gameObject);
        nomalAttack_Squence_2.AddChild(leftAttack_2);
        BossAL_State_LeftAttack leftAttack_2_1 = new BossAL_State_LeftAttack(gameObject);
        nomalAttack_Squence_2.AddChild(leftAttack_2_1);


        //난수 세팅
        //BossAI_SetRandomNum setRandomNum_2 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_2.AddChild(setRandomNum_2);
        nomalAttack_Squence_2.AddChild(setRandomNum_1);



        //노말 패턴 시퀀스3 번
        BTSquence nomalAttack_Squence_3 = new BTSquence();
        nomalAttack_Selector.AddChild(nomalAttack_Squence_3);

        BossAI_State_Choice_3_Condition nomalChoice_3 = new BossAI_State_Choice_3_Condition(gameObject);
        nomalAttack_Squence_3.AddChild(nomalChoice_3);


        BossAL_State_LeftAttack leftAttack_3 = new BossAL_State_LeftAttack(gameObject);
        nomalAttack_Squence_3.AddChild(leftAttack_3);
        BossAI_State_TwoWayAttack twoWayAttack_3 = new BossAI_State_TwoWayAttack(gameObject);
        nomalAttack_Squence_3.AddChild(twoWayAttack_3);
        BossAI_State_ChaseAttack chaseAttack_3 = new BossAI_State_ChaseAttack(gameObject);
        nomalAttack_Squence_3.AddChild(chaseAttack_3);
        BossAI_State_Breath Breath_3 = new BossAI_State_Breath(gameObject);
        nomalAttack_Squence_3.AddChild(Breath_3);

        //난수 세팅
        //BossAI_SetRandomNum setRandomNum_3 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_3.AddChild(setRandomNum_3);
        nomalAttack_Squence_3.AddChild(setRandomNum_1);

        /*
        BTSelector Phase_2 = new BTSelector();

        BossAI_Phase_2_Condition phase_2_Condition = new BossAI_Phase_2_Condition(gameObject);
        Phase_2.AddChild(phase_2_Condition);


        //액션 셀렉터

        BTSelector phase_2_ActionSelector = new BTSelector();
        Phase_2.AddChild(ActionSelector);


        BossAI_State_Phase_2_SpecialAttack phase_2_specialAttack = new BossAI_State_Phase_2_SpecialAttack(gameObject);
        phase_2_ActionSelector.AddChild(phase_2_specialAttack);

        //첫 노말패턴 시퀀스의 컨디션에서 노말액션 시퀀스에 사용할 랜덤 난수 쏴주기

        //2페이즈
        BTSelector phase_2_nomalAttack_Selector = new BTSelector();
        phase_2_ActionSelector.AddChild(phase_2_nomalAttack_Selector);


        //노말 패턴 시퀀스1 번
        BTSquence Phase_2_nomalAttack_Squence_1 = new BTSquence();
        phase_2_nomalAttack_Selector.AddChild(Phase_2_nomalAttack_Squence_1);

        BossAI_State_Choice_1_Condition phase_2_nomalChoice_1 = new BossAI_State_Choice_1_Condition(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_nomalChoice_1);



        BossAI_State_ChaseAttack phase_2_chaseAttack1_1 = new BossAI_State_ChaseAttack(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_chaseAttack1_1);
        BossAI_State_TwoWayAttack phase_2_twoWayAttack_1 = new BossAI_State_TwoWayAttack(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_twoWayAttack_1);
        BossAI_State_RightAttack phase_2_rightAttack_1 = new BossAI_State_RightAttack(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_rightAttack_1);
        BossAI_State_Breath phase_2_Breath_1 = new BossAI_State_Breath(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_Breath_1);
        BossAL_State_LeftAttack phase_2_leftAttack_1 = new BossAL_State_LeftAttack(gameObject);
        Phase_2_nomalAttack_Squence_1.AddChild(phase_2_leftAttack_1);


        //난수 세팅 (나중에 2페이즈 노말 패턴 시퀀스가 더 추가된다면 분기하고 새 스크립트를 만든다)
        nomalAttack_Squence_1.AddChild(setRandomNum_1);


        //노말 패턴 시퀀스2번
        BTSquence phase_2_nomalAttack_Squence_2 = new BTSquence();
        phase_2_nomalAttack_Selector.AddChild(phase_2_nomalAttack_Squence_2);

        BossAI_State_Choice_2_Condition phase_2_nomalChoice_2 = new BossAI_State_Choice_2_Condition(gameObject);
        phase_2_nomalAttack_Squence_2.AddChild(phase_2_nomalChoice_2);


        BossAI_State_TwoWayAttack phase_2_twoWayAttack_2 = new BossAI_State_TwoWayAttack(gameObject);
        phase_2_nomalAttack_Squence_2.AddChild(phase_2_twoWayAttack_2);
        BossAL_State_LeftAttack phase_2_leftAttack_2 = new BossAL_State_LeftAttack(gameObject);
        phase_2_nomalAttack_Squence_2.AddChild(phase_2_leftAttack_2);
        BossAI_State_Breath phase_2_Breath_2 = new BossAI_State_Breath(gameObject);
        phase_2_nomalAttack_Squence_2.AddChild(phase_2_Breath_2);
        BossAL_State_LeftAttack phase_2_leftAttack_2_1 = new BossAL_State_LeftAttack(gameObject);
        phase_2_nomalAttack_Squence_2.AddChild(phase_2_leftAttack_2_1);


        //난수 세팅
        //BossAI_SetRandomNum setRandomNum_2 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_2.AddChild(setRandomNum_2);
        nomalAttack_Squence_2.AddChild(setRandomNum_1);



        //노말 패턴 시퀀스3 번
        BTSquence phase_2_nomalAttack_Squence_3 = new BTSquence();
        phase_2_nomalAttack_Selector.AddChild(phase_2_nomalAttack_Squence_3);

        BossAI_State_Choice_3_Condition phase_2_nomalChoice_3 = new BossAI_State_Choice_3_Condition(gameObject);
        phase_2_nomalAttack_Squence_3.AddChild(phase_2_nomalChoice_3);


        BossAL_State_LeftAttack phase_2_leftAttack_3 = new BossAL_State_LeftAttack(gameObject);
        phase_2_nomalAttack_Squence_3.AddChild(phase_2_leftAttack_3);
        BossAI_State_TwoWayAttack phase_2_twoWayAttack_3 = new BossAI_State_TwoWayAttack(gameObject);
        phase_2_nomalAttack_Squence_3.AddChild(phase_2_twoWayAttack_3);
        BossAI_State_ChaseAttack phase_2_chaseAttack_3 = new BossAI_State_ChaseAttack(gameObject);
        phase_2_nomalAttack_Squence_3.AddChild(phase_2_chaseAttack_3);
        BossAI_State_Breath phase_2_Breath_3 = new BossAI_State_Breath(gameObject);
        phase_2_nomalAttack_Squence_3.AddChild(phase_2_Breath_3);

        //난수 세팅
        //BossAI_SetRandomNum setRandomNum_3 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_3.AddChild(setRandomNum_3);
        nomalAttack_Squence_3.AddChild(setRandomNum_1);

        */





        //셀렉터는 우선순위 높은 순서로 배치 : 생존 여부 -> 특수 패턴 -> 플레이어 체크(공격 여부) -> 이동 여부 순서로 셀렉터 배치 
        //메인 셀렉터 : Squence를 Selector의 자식으로 추가(자식 순서 중요함) 

        BTMainSelector.AddChild(BTDead);
        //BTMainSelector.AddChild(BTAbnormal);

        //메인(페이즈) 셀렉터
        BTMainSelector.AddChild(Phase_1);
        //BTMainSelector.AddChild(Phase_2);

        //작업이 끝난 Selector를 루트 노드에 붙이기
        TreeAIState.AddChild(BTMainSelector);
    }



    //동기화 관련 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 데이터를 전송
            stream.SendNext(hostAimPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(hostRotation);

        }
        else if (stream.IsReading)
        {
            // 데이터를 수신
            hostAimPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            hostRotation = (Quaternion)stream.ReceiveNext();
        }

    }

    #endregion
}
