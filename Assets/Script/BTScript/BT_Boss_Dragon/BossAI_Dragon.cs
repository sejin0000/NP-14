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

    public float currentHP;                  // ���� ü�� ���
    public float viewAngle;                  // �þ߰� (�⺻120��)
    public float viewDistance;               // �þ� �Ÿ� (�⺻ 10)

    //������Ʈ �� ��Ÿ �ܺο��(�Ϻ� �Ҵ��� ���� ��忡�� ����)
    public EnemySO bossSO;                  // Enemy ���� [��� Action Node�� owner�� ȹ���Ŵ]
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

    //���� ���� �ȿ� ���� �÷��̾� ����Ʈ
    public List<PlayerStatHandler> inToAreaPlayers = new List<PlayerStatHandler>();


    public Bullet enemyBulletPrefab;
    public Transform bossHead;
    public Transform bossAim;
    public Collider2D[] bossHitBox;
    public Color originColor;

    public LayerMask targetMask;             // Ÿ�� ���̾�(Player)

    public float SpeedCoefficient = 1f;      // �̵��ӵ� ���
    public float breathAttackDelay;
    public float currentTime;

    public bool isLive = true;
    public bool isAttaking = false;
    public bool isGroggy = false;
    public bool isBreathInProgress = false; // �극�� ���࿩�� �Ǻ�, �ߺ����� ����
    public bool isRunningBreath = false;    // �극�� �߻���
    public bool isTrackingFurthestTarget = false;
    //�÷��̾� ����

    public int lastAttackPlayer;
    public int currentNomalAttackSquence; // �븻���� ������ ���� ����(0~3) : 3��

    //���ӸŴ�������(����) �����ϴ� �÷��̾�� ������ ��û�ؼ� ���

    //���帹�� ���ظ� �� �÷��̾� Ÿ��-> �ҷ�(���� ����) ������ �˸�� ->�÷��� ���ݷ�->



    [SerializeField]
    private Image image_Gauge;              //���� UI : Status
    [SerializeField]
    private TextMeshProUGUI txt_Gauge;


    //����ȭ
    public PhotonView PV;
    private Vector3 hostAimPosition;
    private Quaternion hostRotation;
    private Vector2 hostKnckbackPosition;
    public float lerpSpeed = 10f; // ������ �ʿ��� ��ġ(���� �ʿ�)




    //��ü�� �˹�Ÿ�
    public float knockbackDistance;


    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        PV = GetComponent<PhotonView>();
        AreaList = Show_AttackArea.GetComponentsInChildren<Collider2D>(true); //��Ȱ��ȭ�� ������Ʈ�� Ž��<>(true)

        //���� ������Ʈ Ȱ��ȭ ��, �ൿ Ʈ�� ����
        CreateTreeAIState();
        currentHP = bossSO.hp;
        viewAngle = bossSO.viewAngle;
        viewDistance = bossSO.viewDistance;
        breathAttackDelay = bossSO.breathAttackDelay;
        currentTime = breathAttackDelay;
        isLive = true;

        originColor = spriteRenderers[0].color;

        //�ڽ̱� �׽�Ʈ �� if else �ּ�ó�� �Ұ�
        //�Ѵ� �÷��̾ ȣ��Ʈ�� �Ǻ�?


        knockbackDistance = 0f;


        //TODO ������ ��, ��� �÷��̾� Transform ������ ��´�.TestGameManagerWooMin
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

        currentNomalAttackSquence = Random.Range(0, 3);

        //���� �� ���� Ÿ�� ����
        int randomTarget = Random.Range(0, PlayersTransform.Count);

        currentTarget = PlayersTransform[randomTarget]; 
    }

    void Update()
    {

        if (!PhotonNetwork.IsMasterClient)
        {
            //$�߰��� : ����ȭ�� �Ӹ� ��ġ�� ���� ���� ó��
            bossHead.transform.rotation = Quaternion.Slerp(bossHead.transform.rotation, hostRotation, Time.deltaTime * lerpSpeed);

            AreaList[6].transform.position = hostAimPosition;
            AreaList[6].transform.rotation = Quaternion.Slerp(bossHead.transform.rotation, hostRotation, Time.deltaTime * lerpSpeed);
            return;
        }
        //AIƮ���� ��� ���¸� �� ������ ���� ����


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

    [PunRPC]
    public void Fire()
    {
        int numBullets = 5; // ��ä�� ���� �Ѿ� ���� �����ϼ���
        float AttackAngle = 120f; // ��ä���� ������ �����ϼ���

        float startAngle = -AttackAngle / 2f; // ��ä���� ���� ���� -60 == -60 ~ 120 �� 180���� ������ Ŀ���Ѵ�.

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

    #region ���� �׼�[�극��]

    public void Breath()
    {

        //���� 1 �Ϸ�        
        //�극�� ���� ������ ���� ����
        //�극�� ���׻����� �������� �� �Ӹ��� y - n ��ŭ�� ������ ��ġ�� Ȱ��ȭ ��Ŵ v
        //�극�� ���׻����� �����̼� ���� �����ϰ� �����ش�. v (������Ʈ���� ���� ������)
        //���� ���� �÷��̾�� ���׻��� ���ο��� �˾Ƽ� ó���� v
        //�극�� ���׻����� ���� �� ���ö� ������� �ʴ´�. v
        

        //�극�� ��ƼŬ�� aim�� �����ǿ� Ȱ��ȭ(Ȥ�� ����) - ��ƼŬ��  �ƿ� �Ӹ��� �޾Ƽ� ���� �����̼� �� ������ �ʿ������ ���� v
        //�극���� �ݸ��� ó���� on �ؼ� Ʈ���Ű� �ƴ� �ݶ��̴��� �΋H�� ���, ƨ�ܳ������� ó���Ѵ�. v
        //�극�� ��ƼŬ ��ü�� �÷��̾�� �΋H���� �ƹ��� ��ȣ�ۿ뵵 ���� �ʴ´�. v 

        //���� 2
        //���� ������ �� ������ ��� �극�� ��ƼŬ�� �����Ѵ�. (2.0f �� �ڿ� ��� ������)


        //�극�� ���� �� : �ణ�� �ð�(0.1f ~0.2f) ����, ���� ����� ���� ����Ʈ�� �ִ� �÷��̾ ������θ� ����ĳ��Ʈ�� �߻��Ѵ�.
        //����ĳ��Ʈ�� ���� hit ����� wall�� ��� �����Ѵ�.
        //���� hit ����� ����Ʈ ���� �ִ� �÷��̾��� ��쿡��, �ش� �÷��̾ ������� ���ؿ� �˹��� �ֵ����Ѵ�.
        //4~5�� �� ��ƼŬ�� ���� ���� ���ÿ� ��Ȱ��ȭ ��Ų��.

        //�ణ�� �ð� �����̹Ƿ� 


    }

    [PunRPC]
    public void StartBreathCoroutine()
    {
        StartCoroutine(StartBreath());
    }


    public IEnumerator StartBreath()
    {
        //�극�� �ߺ����� ����
        if (isBreathInProgress)
            yield break;

        isBreathInProgress = true;
        AreaList[6].gameObject.SetActive(true);
        SetAnim("isBreathAttack", true);
        //���� ���� �������°� ��� �� �극�� ��ƼŬ�� �÷���
        yield return new WaitForSeconds(2f);
        BreathParticleObject.Play();


        //����� �극�� ������Ʈ�� bool
        isRunningBreath = true;

        //�극�� ���� �ð� ���� (2+4f)
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
            Debug.Log("������Ʈ �극�� ����");

            //����� ����Ʈ�� ����� �ϳ��� �ִٸ�?
            //����Ʈ �� ��� ��󿡰� ����ĳ��Ʈ �߻�
            //������Ʈ�ϰ� ���� �����Ƿ� ������ ���� �� ���ɼ��� ŭ �� �κ��� ���� ������Ʈ �����°� ���� �ƿ� �����װڴ�

            for (int i = 0; i < inToAreaPlayers.Count; i++)
            {
                Debug.Log("�ݺ��� ����");
                // �÷��̾��� ��ġ
                PlayerStatHandler player = inToAreaPlayers[i];
                Vector3 playerPosition = player.transform.position;
                // �÷��̾�<->�������� ���� ���ϱ�
                Vector3 directionToPlayer = (playerPosition - bossAim.position).normalized;
                Debug.DrawLine(bossAim.position, player.transform.position, Color.white);

                // ���� �߻�
                RaycastHit2D hit = Physics2D.Raycast(bossAim.position, directionToPlayer, Mathf.Infinity, breathTargetLayer);

                if (hit.transform.tag != "Wall" || inToAreaPlayers.Count != 0) // hit ����� �ְų�, ���� �ƴ� ��쿡�� �۵�
                {
                    if (hit.transform.tag == "Player")
                    {
                        //���� �ֱ�� ���ظ� �ֱ�

                        // �÷��̾�� ���� ������ ���� ����


                        // �˹�Ÿ�
                        float knockbackDistance = 1f;


                        // ���� ����
                        player.DirectDamage(bossSO.atk, PV.ViewID);
                        // ���� �˹�
                        player.photonView.RPC("StartKnockback", RpcTarget.All, directionToPlayer, knockbackDistance);
                        //StartCoroutine(player.Knockback(directionToPlayer, knockbackDistance));
                    
                        Debug.Log($"2������ �ȵ� : {inToAreaPlayers.Count} �������� �̸�ŭ ���� :{bossSO.atk}");
                        Debug.Log($"�÷��̾� ���� ü���� : {player.CurHP}");
                    }
                }
            }

            currentTime = breathAttackDelay;
        }
       
    }
    //���� ������ [�� �޼��� ���ÿ� ������]
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
        //���� ����->�� �������� ��������Ʈ ���ο� �ִ� �÷��̾�� ������
        //���� ����->Ÿ�� �÷��̾ �����ϴ� �� �������� ������ ���� -> �� �������� �ش� ��ġ�� ���� ������Ʈ ����
        switch(patternNum)
        {
            case 0:
                AreaList[0].gameObject.SetActive(true); //���� ���ݿ��� (���� ��)
                SetAnim("isLeftAttack", true); //Ʈ���ŷ� �Ϸ��� ������ ������ ������ ���� ����ٰ� �ؼ� bool�� animSet
                LocalSetAnimFalse("isLeftAttack");
                //�� �κп� �÷��̾� ����&�˹� �����ִ� �ڷ�ƾ ����
                LocalTryAttackAreaTargets(0);

                //���� ����
                LocalLaterInActiveAttackArea(0);
                break;
            case 1:
                AreaList[1].gameObject.SetActive(true); ; //���� ���ݿ��� (���� ��)
                SetAnim("isRightAttack", true);
                LocalSetAnimFalse("isRightAttack");
                LocalTryAttackAreaTargets(1);


                LocalLaterInActiveAttackArea(1);
                break;
            case 2:
                AreaList[0].gameObject.SetActive(true); ; // ���� ���� ���� ���� �̰� ��ĳ��?             
                AreaList[1].gameObject.SetActive(true); ;
                SetAnim("isTwoArmAttack", true);
                LocalSetAnimFalse("isTwoArmAttack");

                // �� Area�� ���� ó��
                LocalTryAttackAreaTargets(2);

                LocalLaterInActiveAttackArea(2);  // 2 : �� �� ����
                break;
            case 3:
                AreaList[2].gameObject.SetActive(true); ; // ��� ���� ����
                LocalTryAttackAreaTargets(2);


                LocalLaterInActiveAttackArea(3);
                break;
            case 4:
                AreaList[3].gameObject.SetActive(true); ; // Ÿ�� �÷��̾ ���� ����
                LocalChaseArea();
                LocalTryAttackAreaTargets(3);

                LocalLaterInActiveAttackArea(4);
                break;
            case 5:
                // ��� �÷��̾ ���� ����(3,4,5)
                break;
        }
    }

    public void InActiveAttackArea(int patternNum)
    {
        switch (patternNum)
        {
            case 0:
                AreaList[0].gameObject.SetActive(false); //���� ��
                break;
            case 1:
                AreaList[1].gameObject.SetActive(false); //���� ��
                break;
            case 2:
                AreaList[0].gameObject.SetActive(false); // ���� ���� ���� ����
                AreaList[1].gameObject.SetActive(false);                
                break;
            case 3:
                AreaList[2].gameObject.SetActive(false); // ��� ���� ����
                break;
            case 4:
                AreaList[3].gameObject.SetActive(false); // Ÿ�� �÷��̾ ���� ����
                isTrackingFurthestTarget = false;
                break;
            case 5:
                ;//��� �÷��̾ �����ϴ� ����(3,4,5)
                break;
        }
    }


    //���� ���� UI ����� �ڷ�ƾ(���⼭ ���� ���̾� Ÿ�� ���� => �Ϲ� �������� ��ü)
    public IEnumerator LaterInActiveAttackArea(int patternNum)
    {
        yield return new WaitForSeconds(3f);       
        InActiveAttackArea(patternNum);
    }

    public void LocalLaterInActiveAttackArea(int patternNum)
    {
        StartCoroutine(LaterInActiveAttackArea(patternNum));
    }

    //�ִϸ��̼� ����� �ڷ�ƾ(���⼭ ���� ���̾� �Ϲ� ���� => Ÿ�� �������� ��ü)
    IEnumerator SetAnimFalse(string boolName)
    {
        yield return new WaitForSeconds(0.5f);

        SetAnim(boolName, false);
    }

    public void LocalSetAnimFalse(string boolName)
    {
        StartCoroutine(SetAnimFalse(boolName));
    }


    //���� ���� ���� Ÿ�̹� (���ݸ� �� �޼��� 3���� ��������)
    IEnumerator TryAttackAreaTargets(int areaIndex, float attackCoefficient = 1)
    {
        yield return new WaitForSeconds(2.0f);
        AttackTargetsInArea(areaIndex);
    }
    public void LocalTryAttackAreaTargets(int areaIndex, float attackCoefficient = 1)
    {
        StartCoroutine(TryAttackAreaTargets(areaIndex));
    }


    //��¥ ��¥ ����&�˹���
    public void AttackTargetsInArea(int areaIndex, float attackCoefficient = 1f)
    {
        if (inToAreaPlayers == null || areaIndex < 0 || areaIndex > AreaList.Length)
            return;

        Transform attackAreaTransform = AreaList[areaIndex].transform;

        for (int i = 0; i < inToAreaPlayers.Count; i++)
        {

            PlayerStatHandler player = inToAreaPlayers[i];

            // �÷��̾�� ���� ������ ���� ����
            Vector3 playerPosition = player.transform.position;
            Vector3 areaPosition = attackAreaTransform.position;

            if (areaIndex == 2)
                areaPosition = new Vector3(0, 5, 0);

            Vector3 directionToPlayer = (playerPosition - areaPosition).normalized;


            // �˹�Ÿ�
            float knockbackDistance = 1.5f;

            // ���� �˹�

            StartCoroutine(player.Knockback(directionToPlayer, knockbackDistance));

            // ���� ����
            player.DirectDamage(bossSO.atk * attackCoefficient, PV.ViewID);
        }
    }

    //���� �ð� ���Ŀ� ���ߵ���(���� �ӵ��� 0���� ����)
    IEnumerator ChaseArea()
    {
        if (AreaList[3].gameObject.activeSelf)
        {
            // ���� ���� ��ġ ����
            Vector3 areaStartPosition = currentTarget.position;

            AreaList[3].transform.position = areaStartPosition;

            // ���� �ð� ���� õõ�� Ÿ���� ����
            float elapsedTime = 0f;
            float chaseDuration = 3.5f; // ���� �ð�

            while (elapsedTime < chaseDuration)
            {
                // Ÿ�� ������ õõ�� �̵�
                AreaList[3].transform.position = Vector3.Lerp(areaStartPosition, currentTarget.position, elapsedTime / chaseDuration);

                elapsedTime += Time.deltaTime;
                yield return null; // �� ������ ���
            }
        }

    }

    public void LocalChaseArea()
    {
        StartCoroutine(ChaseArea());
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

    private void SetNearestTarget()
    {

        float minDistance = float.MaxValue;


        for (int i = 0; i < PlayersTransform.Count; i++)
        {
            Debug.Log($"Ÿ�� �ο� üũ {i}");
            if (PlayersTransform[i] == null)
            {
                Debug.Log($"Ÿ���� �������� �ʽ��ϴ�. {i}");
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
        BossAI_State_Dead_DeadCondition deadCondition = new BossAI_State_Dead_DeadCondition(gameObject);
        BTDead.AddChild(deadCondition);
        BossAI_State_Dead state_Dead = new BossAI_State_Dead(gameObject);
        BTDead.AddChild(state_Dead);




        //������ �Ǻ� <������ �� �����>[�⺻ = 1������  ||  ü�� 50% '�̸�' = 2������]
        BTSquence Phase_1 = new BTSquence();
        
        //BossAI_Phase_1_Condition phaseOneCondition = new BossAI_Phase_1_Condition(gameObject);
        //Phase_1.AddChild(phaseOneCondition);
        

        //�׼� ������

        BTSelector ActionSelector = new BTSelector();
        Phase_1.AddChild(ActionSelector);


        //BossAI_State_SpecialAttack specialAttack = new BossAI_State_SpecialAttack(gameObject);
        //ActionSelector.AddChild(specialAttack);

        //ù �븻���� �������� ����ǿ��� �븻�׼� �������� ����� ���� ���� ���ֱ�

        //1������
        BTSelector nomalAttack_Selector = new BTSelector();
        ActionSelector.AddChild(nomalAttack_Selector);


        //�븻 ���� ������1 ��
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


        //���� ����
        BossAI_SetRandomNum setRandomNum_1 = new BossAI_SetRandomNum(gameObject);
        nomalAttack_Squence_1.AddChild(setRandomNum_1);


        //�븻 ���� ������2��
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


        //���� ����
        //BossAI_SetRandomNum setRandomNum_2 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_2.AddChild(setRandomNum_2);
        nomalAttack_Squence_2.AddChild(setRandomNum_1);



        //�븻 ���� ������3 ��
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

        //���� ����
        //BossAI_SetRandomNum setRandomNum_3 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_3.AddChild(setRandomNum_3);
        nomalAttack_Squence_3.AddChild(setRandomNum_1);

        /*
        BTSelector Phase_2 = new BTSelector();

        BossAI_Phase_2_Condition phase_2_Condition = new BossAI_Phase_2_Condition(gameObject);
        Phase_2.AddChild(phase_2_Condition);


        //�׼� ������

        BTSelector phase_2_ActionSelector = new BTSelector();
        Phase_2.AddChild(ActionSelector);


        BossAI_State_Phase_2_SpecialAttack phase_2_specialAttack = new BossAI_State_Phase_2_SpecialAttack(gameObject);
        phase_2_ActionSelector.AddChild(phase_2_specialAttack);

        //ù �븻���� �������� ����ǿ��� �븻�׼� �������� ����� ���� ���� ���ֱ�

        //2������
        BTSelector phase_2_nomalAttack_Selector = new BTSelector();
        phase_2_ActionSelector.AddChild(phase_2_nomalAttack_Selector);


        //�븻 ���� ������1 ��
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


        //���� ���� (���߿� 2������ �븻 ���� �������� �� �߰��ȴٸ� �б��ϰ� �� ��ũ��Ʈ�� �����)
        nomalAttack_Squence_1.AddChild(setRandomNum_1);


        //�븻 ���� ������2��
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


        //���� ����
        //BossAI_SetRandomNum setRandomNum_2 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_2.AddChild(setRandomNum_2);
        nomalAttack_Squence_2.AddChild(setRandomNum_1);



        //�븻 ���� ������3 ��
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

        //���� ����
        //BossAI_SetRandomNum setRandomNum_3 = new BossAI_SetRandomNum(gameObject);
        //nomalAttack_Squence_3.AddChild(setRandomNum_3);
        nomalAttack_Squence_3.AddChild(setRandomNum_1);

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



    //����ȭ ���� 
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // �����͸� ����
            stream.SendNext(hostAimPosition);
            //stream.SendNext(navTargetPoint);
            stream.SendNext(hostRotation);

        }
        else if (stream.IsReading)
        {
            // �����͸� ����
            hostAimPosition = (Vector3)stream.ReceiveNext();
            //navTargetPoint = (Vector3)stream.ReceiveNext();
            hostRotation = (Quaternion)stream.ReceiveNext();
        }

    }

    #endregion
}
