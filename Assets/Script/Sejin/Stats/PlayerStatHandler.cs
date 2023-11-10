using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviour
{
    public event Action<float> HitEvent2;
    public event Action HitEvent;
    public event Action OnDieEvent;
    public event Action OnRegenEvent;
    public event Action OnChangeAmmorEvent;


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

    public bool isDie;

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
            if (value > HP.total)
            {
                curHP = HP.total;
            }
            else
            {
                curHP = value;
            }
        }
    }               //현재   체력

    private float curAmmo;
    [HideInInspector] public float CurAmmo { get { return curAmmo; } set { if (value > AmmoMax.total) curAmmo = AmmoMax.total; curAmmo = value; OnChangeAmmorEvent?.Invoke(); } } //현재   잔탄
    [HideInInspector] public bool CanFire;                                //발사   가능한지
    [HideInInspector] public bool CanReload;                              //장전   가능한지
    [HideInInspector] public bool CanSkill;                               //스킬   가능한지
    [HideInInspector] public bool CanRoll;                                //구르기 가능한지
    [HideInInspector] public bool Invincibility;                          //무적

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

        PlayerSprite = playerStats.playerSprite;
        WeaponSprite = playerStats.weaponSprite;
        BulletSprite = playerStats.BulletSprite;
        CurHP = HP.total;
        CurAmmo = AmmoMax.total;

        CanFire = true;
        CanReload = true;
        CanSkill = true;
        CanRoll = true;
        Invincibility = false;

        PlayerSpriteCase = _PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpriteCase = _WeaponSprite.GetComponent<SpriteLibrary>();

        PlayerSpriteCase.spriteLibraryAsset = PlayerSprite;
        WeaponSpriteCase.spriteLibraryAsset = WeaponSprite;
        defense = 1;
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
        if(CurHP - damage <= 0)
        {
            isDie = true;
            OnDieEvent?.Invoke();
        }
        damage = damage * defense;
        CurHP -= damage;
        HitEvent?.Invoke();
        HitEvent2?.Invoke(damage);//이게 값이 필요한경우와 필요 없는경우가 있는데 한개로 할수가 있는지 모르겠음 일단 이렇게함
        Debug.Log("[PlayerStatHandler] " + "Damage Done");
    }

    public void Regen(float HP)
    {
        OnRegenEvent?.Invoke();
        CurHP = HP;
        isDie = false;
    }
}