using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviour
{
    public Action HitEvent;

    [SerializeField] private PlayerSO playerStats;

    public Stats ATK;                 // ���ݷ�
    public Stats HP;                  // ü��
    public Stats Speed;               // �̵� �ӵ�
    public Stats AtkSpeed;            // ���� �ӵ�
    public Stats ReloadCoolTime;      // ����   �� Ÿ��
    public Stats SkillCoolTime;       // ��ų   �� Ÿ��
    public Stats RollCoolTime;        // ������ �� Ÿ��
    public Stats BulletSpread;        // ź����
    public Stats BulletLifeTime;      // �Ѿ� ��Ÿ�
    public Stats LaunchVolume;        // �ѹ��� �߻��� �߻緮
    public Stats Critical;            // ũ��Ƽ��
    public Stats AmmoMax;             // ��ź��

    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // ��������Ʈ
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // ��������Ʈ
    [HideInInspector] public Sprite BulletSprite; // ��������Ʈ

    [HideInInspector] public SpriteLibrary PlayerSpriteCase; // ��������Ʈ
    [HideInInspector] public SpriteLibrary WeaponSpriteCase; // ��������Ʈ


    public GameObject _PlayerSprite;
    public GameObject _WeaponSprite;


    private float curHP;
    [HideInInspector] public float CurHP   { get { return curHP;  } set { if (value > HP.total) { curHP = HP.total; } HitEvent?.Invoke(); } }               //����   ü��

    private float curAmmo;
    [HideInInspector] public float CurAmmo { get { return curAmmo;  } set { if (value > AmmoMax.total) curAmmo = AmmoMax.total; curAmmo = value; } } //����   ��ź
    [HideInInspector] public bool  CanFire;                                //�߻�   ��������
    [HideInInspector] public bool  CanReload;                              //����   ��������
    [HideInInspector] public bool  CanSkill;                               //��ų   ��������
    [HideInInspector] public bool  CanRoll;                                //������ ��������
    [HideInInspector] public bool  Invincibility;                          //����

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
        BulletLifeTime =  new Stats(playerStats.bulletLifeTime);
        LaunchVolume   =  new Stats(playerStats.launchVolume);
        Critical       =  new Stats(playerStats.critical);
        AmmoMax        =  new Stats(playerStats.ammoMax);

        PlayerSprite   =  playerStats.playerSprite;
        WeaponSprite   =  playerStats.weaponSprite;
        BulletSprite   =  playerStats.BulletSprite;
        CurHP          =  HP.total;
        CurAmmo        =  AmmoMax.total;

        CanFire        =  true;
        CanReload      =  true;
        CanSkill       =  true;
        CanRoll        =  true;
        Invincibility  =  false;

        PlayerSpriteCase = _PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpriteCase = _WeaponSprite.GetComponent<SpriteLibrary>();

        PlayerSpriteCase.spriteLibraryAsset = PlayerSprite;
        WeaponSpriteCase.spriteLibraryAsset = WeaponSprite;
    }

    public void CharacterChange(PlayerSO playerData)
    {
        playerStats = playerData;
        Awake();
    }
}
