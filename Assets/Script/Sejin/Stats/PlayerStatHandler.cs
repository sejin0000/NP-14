using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviourPun
{
    public event Action<float> HitEvent2;
    public event Action HitEvent;
    public event Action OnDieEvent;
    public event Action OnRegenEvent;
    public event Action<int> OnRegenCalculateEvent;
    public event Action OnChangeAmmorEvent;
    public event Action OnChangeCurHPEvent;
    public event Action MoveStartEvent;
    public event Action MoveEndEvent;
    public event Action EnemyHitEvent;
    public event Action KillCatchEvent;
    public event Action<float> GetDamege;
    public event Action<float, int> OnDamageReflectEvent;


    [SerializeField] private PlayerSO playerStats;

    PlayerAnimatorController anime;


    public Stats ATK;                 // 공격력
    public Stats HP;                  // 체력
    public Stats Speed;               // 이동 속도
    public Stats AtkSpeed;            // 공격 속도
    public Stats ReloadCoolTime;      // 장전   쿨 타임
    public Stats SkillCoolTime;       // 스킬   쿨 타임
    public Stats RollCoolTime;        // 구르기 쿨 타임
    public Stats BulletSpread;        // 탄퍼짐
    public Stats BulletLifeTime;      // 총알 사거리
    public Stats LaunchVolume;        // 한번의 발사의 발사량
    public Stats Critical;            // 크리티컬
    public Stats AmmoMax;             // 장탄수
    public float defense;

    public Sprite indicatorSprite;
    public AudioClip atkClip;
    public AudioClip reloadStartClip;
    public AudioClip reloadFinishClip;

    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // 스프라이트
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // 스프라이트
    [HideInInspector] public Sprite BulletSprite; // 스프라이트

    [HideInInspector] public SpriteLibrary PlayerSpriteCase; // 스프라이트
    [HideInInspector] public SpriteLibrary WeaponSpriteCase; // 스프라이트


    public GameObject _PlayerSprite;
    public GameObject _WeaponSprite;
    public PlayerDebuffControl _DebuffControl;

    public bool isNoramlMove;
    public bool isCanSkill;
    public bool isCanAtk;
    public bool isDie;
    public bool isRegen;
    public int RegenHP;
    public int MaxRegenCoin;
    private int curRegenCoin;
    private int kill;
    [HideInInspector] public bool CanSpeedBuff;
    [HideInInspector] public bool CanLowSteam;
    [HideInInspector] public bool CanAtkBuff;
    public int CurRegenCoin
    {
        get { return curRegenCoin; }
        set
        {
            if (curRegenCoin != value)
            {
                curRegenCoin = value;
            }
            if (value == 0)
            {
                OnRegenCalculateEvent += RegenHPCalculator;
            }
        }
    }
    public int MaxSkillStack;
    public int CurSkillStack;

    public int MaxRollStack;
    public int CurRollStack;

    public int evasionPersent;
    public float DamegeTemp;


    private float curHP;
    [HideInInspector]
    public float CurHP
    {
        get
        {
            return curHP;
        }
        set
        {
            if (curHP != value)
            {
                if (value > HP.total)
                {
                    curHP = HP.total;
                }
                else if (value < 0)
                {
                    curHP = 0;
                }
                else
                {
                    curHP = value;
                }
                OnChangeCurHPEvent?.Invoke();
            }
        }
    }

    [SerializeField] private float curAmmo;
    //[HideInInspector]
    public float CurAmmo //현재 잔탄
    {
        get
        {
            return curAmmo;
        }
        set
        {
            if (value > AmmoMax.total)
                curAmmo = AmmoMax.total;
            curAmmo = value;
            OnChangeAmmorEvent?.Invoke();
        }
    }
    [HideInInspector] public bool CanFire;                                //발사   가능한지
    [HideInInspector] public bool CanReload;                              //장전   가능한지
    [HideInInspector] public bool CanSkill;                               //스킬   가능한지
    [HideInInspector] public bool CanRoll;                                //구르기 가능한지
    public bool Invincibility;                          //무적

    public bool useSkill;
    public bool UseRoll;
    public bool ImGhost;
    public bool IsInShield;
    int viewID;
    [HideInInspector] public bool IsChargeAttack;
    [HideInInspector] public bool CanReflect;

    public float ReflectCoeff;
    public float InShieldHP;

    [HideInInspector] public string[] PlayerStatNameArray;
    [HideInInspector] public Stats[] PlayerStatArray;

    private void Awake()
    {
        anime = GetComponent<PlayerAnimatorController>();
        ATK = new Stats(playerStats.atk);
        HP = new Stats(playerStats.hp);
        Speed = new Stats(playerStats.unitSpeed);
        AtkSpeed = new Stats(playerStats.atkSpeed);
        ReloadCoolTime = new Stats(playerStats.reloadCoolTime);
        SkillCoolTime = new Stats(playerStats.skillCoolTime);
        RollCoolTime = new Stats(playerStats.rollCoolTime);
        BulletSpread = new Stats(playerStats.bulletSpread);
        BulletLifeTime = new Stats(playerStats.bulletLifeTime);
        LaunchVolume = new Stats(playerStats.launchVolume);
        Critical = new Stats(playerStats.critical);
        AmmoMax = new Stats(playerStats.ammoMax);
        MaxRegenCoin = 0;
        CurRegenCoin = MaxRegenCoin;

        PlayerSprite = playerStats.playerSprite;
        WeaponSprite = playerStats.weaponSprite;
        BulletSprite = playerStats.BulletSprite;
        CurHP = HP.total;
        CurAmmo = AmmoMax.total;

        CanFire = true;
        CanReload = true;
        CanSkill = true;
        CanRoll = true;
        UseRoll = true;
        Invincibility = false;

        CanSpeedBuff = true;
        CanLowSteam = true;
        CanAtkBuff = true;

        isNoramlMove = true;
        isCanSkill = true;
        isCanAtk = true;
        evasionPersent = 0;
        isRegen = false;
        ImGhost = false;

        kill = 0;
        MaxSkillStack = 1;
        CurSkillStack = MaxSkillStack;
        MaxRollStack = 1;
        CurRollStack = MaxRollStack;

        PlayerSpriteCase = _PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpriteCase = _WeaponSprite.GetComponent<SpriteLibrary>();

        PlayerSpriteCase.spriteLibraryAsset = PlayerSprite;
        WeaponSpriteCase.spriteLibraryAsset = WeaponSprite;
        defense = 1;

        IsChargeAttack = false;

        _DebuffControl = GetComponent<PlayerDebuffControl>();
        indicatorSprite = playerStats.indicatorSprite;
        atkClip = playerStats.atkClip;
        reloadStartClip = playerStats.reloadClip[0];
        reloadFinishClip = playerStats.reloadClip[1];

        PlayerStatArray = new Stats[11];
        PlayerStatNameArray = new string[11]
        {
            "체력",
            "공격력",
            "이동속도",
            "공격속도",
            "장전 쿨타임",
            "스킬 쿨타임",
            "대쉬 쿨타임",
            "탄퍼짐",
            "사거리",
            "크리티컬",
            "장탄수",
        };
    }
    private void PunRpcStageBuffReset()
    {
        photonView.RPC("stageBuffReset", RpcTarget.All);
    }
    [PunRPC]
    private void stageBuffReset()
    {
        if (!CanSpeedBuff)
        {
            Speed.added -= 3f;
            CanSpeedBuff = true;
        }
        if (!CanLowSteam)
        {
            CanSpeedBuff = true;
            AtkSpeed.added -= 0.5f;
            Speed.added -= 0.5f;
        }
        if (!CanAtkBuff)
        {
            ATK.coefficient -= 0.1f;
            CanAtkBuff = true;
        }
    }

    private void Start()
    {
        //if (MainGameManager.Instance != null)
        //{
        //    MainGameManager.Instance.OnGameStartedEvent += RefillCoin;
        //    viewID = photonView.ViewID;
        //    OnChangeCurHPEvent += SendSyncHP;
        //}

        if (TestGameManager.Instance != null)
        {
            viewID = photonView.ViewID;
            OnChangeCurHPEvent += SendSyncHP;
        }
        if (GameManager.Instance != null)
        {
            viewID = photonView.ViewID;
            OnChangeCurHPEvent += SendSyncHP;
            StageStartSet();
        }

    }
    public void StageStartSet()
    {
        GameManager.Instance.OnStageStartEvent += RefillCoin;
        GameManager.Instance.OnStageStartEvent += startHp;
        GameManager.Instance.OnBossStageStartEvent += RefillCoin;
        GameManager.Instance.OnBossStageStartEvent += startHp;
        GameManager.Instance.OnStageStartEvent += PunRpcStageBuffReset;
        GameManager.Instance.OnBossStageStartEvent += PunRpcStageBuffReset;
        viewID = photonView.ViewID;
    }
    public override string ToString()
    {
        return curHP.ToString() + "/" + HP.total.ToString();
    }

    public void CharacterChange(PlayerSO playerData)
    {
        playerStats = playerData;
        Awake();
        Debug.Log("[PlayerStatHandler]" + this.ToString());
        Debug.Log("[PlayerStatHandler] " + "CharacterChange Done");
    }
    [PunRPC]
    public void GiveDamege(float damage)
    {
        Damage(damage);
    }

    [PunRPC]
    public void DirectDamage(float damage, int targetID)
    {
        if (IsInShield)
        {
            damage -= InShieldHP;
        }
        if (CanReflect)
        {
            CallReflectEvent(damage, targetID);
            damage *= (1 - ReflectCoeff);
        }
        Damage(damage);
    }

    public void Damage(float damage)
    {
        if (!isDie)
        {
            DamegeTemp = damage;
            GetDamege?.Invoke(DamegeTemp);
            int a = UnityEngine.Random.Range(0, 100);
            if (evasionPersent <= a)
            {
                DamegeTemp = DamegeTemp * defense;
                CurHP -= DamegeTemp;
                HitEvent?.Invoke();
                HitEvent2?.Invoke(DamegeTemp);//이게 값이 필요한경우와 필요 없는경우가 있는데 한개로 할수가 있는지 모르겠음 일단 이렇게함

                if (CurHP - DamegeTemp <= 0)
                {
                    CurHP -= DamegeTemp;
                    isDie = true;
                    OnDieEvent?.Invoke();

                    if (CurRegenCoin > 0)
                    {
                        CurRegenCoin -= 1;
                        isDie = false;
                        Debug.Log($"부활 : {CurRegenCoin}");
                        Regen(HP.total);
                        return;
                    }

                    TestGameManager.Instance?.DiedAfter();
                    GameManager.Instance?.PlayerDie();
                    photonView.RPC("LayerSet", RpcTarget.All);
                }

            }
        }

    }
    [PunRPC]
    public void LayerSet() 
    {
        this.gameObject.layer = 12;
    }

    public void HPadd(float addhp)
    {
        CurHP += addhp;
    }

    public void Regen(float HP)
    {
        HPadd(HP);
        OnRegenEvent?.Invoke();
        OnRegenCalculateEvent?.Invoke(RegenHP);
        if (photonView.IsMine) 
        {
            PlayerInputController tempInputControl = this.gameObject.GetComponent<PlayerInputController>();
            tempInputControl.ResetSetting();
        }
        isDie = false;
        isRegen = true;
        _DebuffControl.Init(PlayerDebuffControl.buffName.TwoMoon, 5f);
        photonView.RPC("SendRegenBool", RpcTarget.All, viewID);
    }

    [PunRPC]
    public void SendRegenBool(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);       
        pv.GetComponent<PlayerStatHandler>().isRegen = true;

        Invoke("InvokeSetRegenBool", 5f);
    }

    private void InvokeSetRegenBool()
    {
        SetRegenBool(viewID);
    }

    [PunRPC]
    public void SetRegenBool(int viewID)
    {
        PhotonView pv = PhotonView.Find(viewID);
        if (pv.IsMine)
        {
            isRegen = false;
        }
        else
        {
            pv.GetComponent<PlayerStatHandler>().isRegen = false;            
        }
        // 부활 파티클이 꺼져야 하는 시점
    }

    public void RefillCoin()
    {
        CurRegenCoin = MaxRegenCoin;
    }
    public void startHp()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("PunRpcStartHp",RpcTarget.All);
        }    
    }
    [PunRPC]
    public void PunRpcStartHp() 
    {
        if (photonView.IsMine)
        {
            PlayerInputController tempInputControl = this.gameObject.GetComponent<PlayerInputController>();
            tempInputControl.ResetSetting();
            tempInputControl.InputOn();
        }
        CurHP = HP.total;
        this.gameObject.layer = 8;
        if (ImGhost)
        {
            this.gameObject.layer = 13;
        }
        if (isDie == true)
        {
            isDie = false;
            anime._animation.SetTrigger("IsRegen");
        }
    }

    public void SendSyncHP()
    {
        if(photonView.IsMine)
        {
            photonView.RPC("SetSyncHP",RpcTarget.OthersBuffered,viewID,CurHP);
        }
    }

    [PunRPC]
    public void SetSyncHP(int viewID,float _CurHP )
    {
        PhotonView _PV;
        _PV = PhotonView.Find(viewID);
        PlayerStatHandler _PvPlayer = _PV.gameObject.GetComponent<PlayerStatHandler>();
        _PvPlayer.CurHP = _CurHP;

        if(_PvPlayer.CurHP <= 0 )
        {
            isDie = true;
            OnDieEvent?.Invoke();
            this.gameObject.layer = 12;
        }
    }
    public void MoveStartCall() 
    {
        MoveStartEvent?.Invoke();
    }
    public void MoveEndCall()
    {
        MoveEndEvent?.Invoke();
    }

    public void EnemyHitCall() 
    {
        EnemyHitEvent?.Invoke();
    }
    [PunRPC]
    public void KillEvent()
    {
        if (photonView.IsMine) 
        {
            kill++;
            KillCatchEvent?.Invoke();
        }
    }
    public void RegenHPCalculator(int calHP = 0)
    {
        if (calHP == 0)
        {
            return;
        }
        else
        {
            Debug.Log($"A206 발악 실행 : {calHP}");
            HPadd((calHP - HP.total));
        }
    }

    public void CallReflectEvent(float damage, int targetID)
    {
        if (CanReflect)
        {
            OnDamageReflectEvent?.Invoke(damage, targetID);
        }
    }

    [PunRPC]
    public void thankyouLife(int pvid)
    {
        PhotonView photonView = PhotonView.Find(pvid);
        WeaponSystem weapon = photonView.gameObject.GetComponent<WeaponSystem>();
        weapon.canresurrection = false;
    }

    [PunRPC]
    public void StartKnockback(Vector3 direction, float distance)
    {
        StartCoroutine(Knockback(direction, distance));
    }

    //보스 패턴용 넉백 추가함 - 우민규
    public IEnumerator Knockback(Vector3 direction, float distance)
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * distance;

        float elapsedTime = 0f;

        while (elapsedTime < 0.1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.1f);
            elapsedTime += Time.deltaTime;
            yield return null; // 1프레임 대기
        }

        // 최종 위치에 고정
        transform.position = targetPosition;
    }
    [PunRPC]
    public void ImLive()
    {
        Regen(HP.total);
        this.gameObject.layer = 8;
    }

    public void SetStatusArray()
    {
        PlayerStatArray = new Stats[11]
        {
            HP,
            ATK,
            Speed,
            AtkSpeed,
            ReloadCoolTime,
            SkillCoolTime,
            RollCoolTime,
            BulletSpread,
            BulletLifeTime,
            Critical,
            AmmoMax
        };

    }
}