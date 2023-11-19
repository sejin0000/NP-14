using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private PhotonView pv;
    public Transform muzzleOfAGun;
    private GameObject bullet;
    public Dictionary<string, int> targets;
    public bool isDamage;
    public bool sizeUp;
    public bool sizeBody;
    public bool locator;
    public bool sniping;
    public bool canAngle;

    public bool fire;
    public bool water;
    public bool ice;
    public bool burn;
    public bool gravity;
    public bool Penetrate;

    public int _viewID;
    // 추가
    public int bulletNum;
    private CoolTimeController _cool;

    private void Awake()
    {
        isDamage = true;
        bullet         = Resources.Load<GameObject>("Prefabs/Player/Bullet");
        pv             = GetComponent<PhotonView>();
        _controller    = GetComponent<TopDownCharacterController>();
        _viewID        = pv.ViewID;
        //target = BulletTarget.Enemy;
        targets = new Dictionary<string, int>();
        targets["Enemy"] = (int)BulletTarget.Enemy;
        // 추가
        sizeUp = false;
        sizeBody = false;
        locator = false;
        sniping = false;
        canAngle = false;
        fire = false;
        water = false;
        ice = false;
        burn = false;
        gravity = false;
        Penetrate = false;
    _cool = GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        _controller.OnAttackEvent += Shooting;
    }

    public void Shooting()
    {
        for (int i = 0; i < _controller.playerStatHandler.LaunchVolume.total; i++)
        {
            Quaternion rot = muzzleOfAGun.transform.rotation;
            rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * _controller.playerStatHandler.BulletSpread.total, _controller.playerStatHandler.BulletSpread.total));// 중요함

            float _ATK = _controller.playerStatHandler.ATK.total;
            float _BLT = _controller.playerStatHandler.BulletLifeTime.total;
            var _targets = targets;
            bool _isDamage = isDamage;


            pv.RPC("BS", RpcTarget.All, rot, _ATK, _BLT, _targets, _isDamage, _viewID);
            _controller.playerStatHandler.CurAmmo--;
        }
    }

    public void Charging()
    {
        int bullets = _cool.bulletNum;
        if (bullets == 0)
            return;
        for (int i = 0; i < bullets; i++) 
        {
            Quaternion rot = muzzleOfAGun.transform.rotation;
            rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * _controller.playerStatHandler.BulletSpread.total, _controller.playerStatHandler.BulletSpread.total));// 중요함

            float _ATK = _controller.playerStatHandler.ATK.total;
            float _BLT = _controller.playerStatHandler.BulletLifeTime.total;
            var _targets = targets;
            bool _isDamage = isDamage;

            pv.RPC("BS", RpcTarget.All, rot, _ATK, _BLT, _targets, _isDamage, _viewID);
            //_controller.playerStatHandler.CurAmmo--;
        }
        _cool.bulletNum = 0;
    }

    [PunRPC]
    public void BS(Quaternion rot, float Atk, float bulletLifeTime,Dictionary<string, int> _targets, bool _isDamage, int _viewID)//BulletSpawn
    {
        Debug.Log("타겟");
        foreach (var target in _targets)
        {
            Debug.Log(target);
        }
        Debug.Log("데미지를 주는가?");
        Debug.Log(_isDamage);
        float size=1f;

        if (sizeBody) 
        {
            size = transform.localScale.x;
        }
        if (sizeUp) 
        {
            size *= 1.3f;
        }

        GameObject _object =  Instantiate(bullet, muzzleOfAGun.transform.position, rot);
        Bullet _bullet = _object.GetComponent<Bullet>();

        
        _object.transform.localScale = new Vector2(size, size);
        if (locator)
        {
            _bullet.locator = true;
            Atk = Atk * 0.3f;
        }
        if (sniping)
        {
            _bullet.sniping = true;
            Atk -= Atk * 0.3f;
        }
        _bullet.ATK = Atk;
        _bullet.BulletLifeTime = bulletLifeTime;
        _bullet.targets = _targets;
        _bullet.IsDamage = _isDamage;
        _bullet.BulletOwner = _viewID;
        _bullet.canAngle = canAngle;
        _object.GetComponent<SpriteRenderer>().sprite = _controller.playerStatHandler.BulletSprite;
        _bullet.fire = fire;
        _bullet.water = water;
        _bullet.ice = ice;
        _bullet.burn = burn;
        _bullet.gravity = gravity;
        _bullet.Penetrate = Penetrate;
    }
}
