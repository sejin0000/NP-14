using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviour
{


    [SerializeField] private PlayerSO playerStats;

    public Stats ATK;                 // ���ݷ�
    public Stats HP;                  // ü��
    public Stats Speed;               // �̵� �ӵ�
    public Stats AtkSpeed;            // ���� �ӵ�
    public Stats ReloadCoolTime;      // ����   �� Ÿ��
    public Stats SkillCoolTime;       // ��ų   �� Ÿ��
    public Stats RollCoolTime;        // ������ �� Ÿ��
    public Stats BulletSpread;        // ź����
    public Stats Critical;            // ũ��Ƽ��
    public Stats AmmoMax;             // ��ź��

    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // ��������Ʈ
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // ��������Ʈ

    [HideInInspector] public float CurHP   { get { return CurHP;  } set { if (value > HP.total) CurHP = HP.total;  } }               //����   ü��
    [HideInInspector] public float CurAmmo { get { return CurAmmo;  } set { if (value > AmmoMax.total) CurAmmo = AmmoMax.total;  } } //����   ��ź
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
