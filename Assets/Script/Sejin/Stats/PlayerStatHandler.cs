using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviour
{


    [SerializeField] private PlayerSO playerStats;

    public Stats ATK;                 // 공격력
    public Stats HP;                  // 체력
    public Stats Speed;               // 이동 속도
    public Stats AtkSpeed;            // 공격 속도
    public Stats ReloadCoolTime;      // 장전   쿨 타임
    public Stats SkillCoolTime;       // 스킬   쿨 타임
    public Stats RollCoolTime;        // 구르기 쿨 타임
    public Stats BulletSpread;        // 탄퍼짐
    public Stats Critical;            // 크리티컬
    public Stats AmmoMax;             // 장탄수

    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // 스프라이트
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // 스프라이트

    [HideInInspector] public float CurHP   { get { return CurHP;  } set { if (value > HP.total) CurHP = HP.total;  } }               //현재   체력
    [HideInInspector] public float CurAmmo { get { return CurAmmo;  } set { if (value > AmmoMax.total) CurAmmo = AmmoMax.total;  } } //현재   잔탄
    [HideInInspector] public bool  CanReload;                              //장전   가능한지
    [HideInInspector] public bool  CanSkill;                               //스킬   가능한지
    [HideInInspector] public bool  CanRoll;                                //구르기 가능한지
    [HideInInspector] public bool  Invincibility;                          //무적

    private void Awake()
    {
        ATK            =  new Stats(playerStats.atk);
        HP             =  new Stats(playerStats.hp);
        Speed          =  new Stats(playerStats.unitSpeed);
        AtkSpeed       =  new Stats(playerStats.atkSpeed);
        ReloadCoolTime =  new Stats(playerStats.reloadCoolTime);
        SkillCoolTime  =  new Stats(playerStats.skillCoolTime);
        RollCoolTime   =  new Stats(playerStats.rollCoolTime);
        BulletSpread   =  new Stats(playerStats.bulletSpread);
        Critical       =  new Stats(playerStats.critical);
        AmmoMax        =  new Stats(playerStats.ammoMax);
        PlayerSprite   =  playerStats.playerSprite;
        WeaponSprite   =  playerStats.weaponSprite;
        CurHP          =  HP.total;
        CanReload      =  true;
        CanSkill       =  true;
        CanRoll        =  true;
        Invincibility  =  false;
    }

    public void CharacterChange(PlayerSO playerData)
    {
        playerStats = playerData;
        Awake();
    }
}
