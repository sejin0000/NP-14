using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
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


    [SerializeField] private PlayerSO playerStats;

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


    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // 스프라이트
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // 스프라이트
    [HideInInspector] public Sprite BulletSprite; // 스프라이트

    [HideInInspector] public SpriteLibrary PlayerSpriteCase; // 스프라이트
    [HideInInspector] public SpriteLibrary WeaponSpriteCase; // 스프라이트


    public GameObject _PlayerSprite;
    public GameObject _WeaponSprite;

    public bool isNoramlMove;
    public bool isCanSkill;
    public bool isCanAtk;
    public bool isDie;
    public int RegenHP;
    public int MaxRegenCoin;
    private int curRegenCoin;
    private int kill;
    [HideInInspector] public bool CanSpeedBuff;
    [HideInInspector] public bool CanLowSteam;
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
                else
                {
                    curHP = value;
                }
                OnChangeCurHPEvent?.Invoke();
                Debug.Log($"HPHPHPHPHPHHP  {CurHP}");
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
    [HideInInspector] public bool Invincibility;                          //무적

    public bool useSkill;
    public bool UseRoll;

    int viewID;
    [HideInInspector] public bool IsChargeAttack;

    private void Awake()
    {
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

        isNoramlMove = true;
        isCanSkill=true;
        isCanAtk = true;
        evasionPersent = 0;

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
    }
    private void OnEnable()
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
    }

    private void Start()
    {
        if (MainGameManager.Instance != null)
        {
            MainGameManager.Instance.OnGameStartedEvent += RefillCoin;
            viewID = photonView.ViewID;
            OnChangeCurHPEvent += SendSyncHP;
        }

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

    public void Damage(float damage)
    {
        DamegeTemp = damage;
        GetDamege?.Invoke(DamegeTemp);
        int a = UnityEngine.Random.Range(0, 100);
        if (evasionPersent <= a)
        {
            if (CurHP - DamegeTemp <= 0)
            {
                if (CurRegenCoin > 0)
                {
                    CurRegenCoin -= 1;
                    Regen(HP.total);
                    return;
                }

                isDie = true;
                OnDieEvent?.Invoke();
                this.gameObject.layer = 0;
            }
            DamegeTemp = DamegeTemp * defense;
            CurHP -= DamegeTemp;
            HitEvent?.Invoke();
            HitEvent2?.Invoke(DamegeTemp);//이게 값이 필요한경우와 필요 없는경우가 있는데 한개로 할수가 있는지 모르겠음 일단 이렇게함
            Debug.Log("[PlayerStatHandler] " + "Damage Done");
        }
        else 
        {
            Debug.Log("피함 피함 피함");
        }

    }

    public void HPadd(float addhp)
    {
        CurHP += addhp;
    }

    public void Regen(float HP)
    {
        CurHP = HP;
        OnRegenEvent?.Invoke();
        OnRegenCalculateEvent?.Invoke(RegenHP);
        PlayerInputController tempInputControl = this.gameObject.GetComponent<PlayerInputController>();
        tempInputControl.ResetSetting();
        isDie = false;
    }

    public void RefillCoin()
    {
        CurRegenCoin = MaxRegenCoin;
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
        Debug.Log($" { viewID} : HP : {_CurHP}");
        PhotonView _PV;
        _PV = PhotonView.Find(viewID);

        _PV.gameObject.GetComponent <PlayerStatHandler>().CurHP = _CurHP;

        if(_PV.gameObject.GetComponent<PlayerStatHandler>().CurHP <= 0 )
        {
            isDie = true;
            OnDieEvent?.Invoke();
            this.gameObject.layer = 0;
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
            curHP = calHP;
        }
    }

}