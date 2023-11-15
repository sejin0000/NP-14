using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.U2D.Animation;

public class PlayerStatHandler : MonoBehaviourPun
{
    public event Action<float> HitEvent2;
    public event Action HitEvent;
    public event Action OnDieEvent;
    public event Action OnRegenEvent;
    public event Action OnChangeAmmorEvent;
    public event Action OnChangeCurHPEvent;


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
    public float defense;


    [HideInInspector] public SpriteLibraryAsset PlayerSprite; // ��������Ʈ
    [HideInInspector] public SpriteLibraryAsset WeaponSprite; // ��������Ʈ
    [HideInInspector] public Sprite BulletSprite; // ��������Ʈ

    [HideInInspector] public SpriteLibrary PlayerSpriteCase; // ��������Ʈ
    [HideInInspector] public SpriteLibrary WeaponSpriteCase; // ��������Ʈ


    public GameObject _PlayerSprite;
    public GameObject _WeaponSprite;

    public bool isNoramlMove;
    public bool isDie;
    public int MaxRegenCoin;
    public int CurRegenCoin;

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
    }               //����   ü��

    private float curAmmo;
    [HideInInspector] public float CurAmmo { get { return curAmmo; } set { if (value > AmmoMax.total) curAmmo = AmmoMax.total; curAmmo = value; OnChangeAmmorEvent?.Invoke(); } } //����   ��ź
    [HideInInspector] public bool CanFire;                                //�߻�   ��������
    [HideInInspector] public bool CanReload;                              //����   ��������
    [HideInInspector] public bool CanSkill;                               //��ų   ��������
    [HideInInspector] public bool CanRoll;                                //������ ��������
    [HideInInspector] public bool Invincibility;                          //����

    public bool useSkill;

    int viewID;


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
        Invincibility = false;

        isNoramlMove = true;

        PlayerSpriteCase = _PlayerSprite.GetComponent<SpriteLibrary>();
        WeaponSpriteCase = _WeaponSprite.GetComponent<SpriteLibrary>();

        PlayerSpriteCase.spriteLibraryAsset = PlayerSprite;
        WeaponSpriteCase.spriteLibraryAsset = WeaponSprite;
        defense = 1;
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
        
        if(CurHP - damage <= 0)
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

        damage = damage * defense;
        CurHP -= damage;
        HitEvent?.Invoke();
        HitEvent2?.Invoke(damage);//�̰� ���� �ʿ��Ѱ��� �ʿ� ���°�찡 �ִµ� �Ѱ��� �Ҽ��� �ִ��� �𸣰��� �ϴ� �̷�����
        Debug.Log("[PlayerStatHandler] " + "Damage Done");
    }

    public void HPadd(float addhp)
    {
        CurHP += addhp;
    }

    public void Regen(float HP)
    {
        CurHP = HP;
        Debug.Log("��Ȱ�Ͽ����ϴ�. ��Ȱ ��ƼŬ �߰��ؾ���");
        OnRegenEvent?.Invoke();
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
}