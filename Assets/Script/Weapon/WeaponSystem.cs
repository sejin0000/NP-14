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
    public BulletTarget target;
    public bool isDamage;

    private void Awake()
    {
        isDamage = true;
        bullet         = Resources.Load<GameObject>("Prefabs/Player/Bullet");
        pv             = GetComponent<PhotonView>();
        _controller    = GetComponent<TopDownCharacterController>();
        target = BulletTarget.Enemy;
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
            rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * _controller.playerStatHandler.BulletSpread.total, _controller.playerStatHandler.BulletSpread.total));// Áß¿äÇÔ

            pv.RPC("BS", RpcTarget.All, rot, _controller.playerStatHandler.ATK.total, _controller.playerStatHandler.BulletLifeTime.total,(int)target, isDamage);
        }
    }

    [PunRPC]
    public void BS(Quaternion rot, float Atk, float bulletLifeTime,int _target, bool _isDamage)//BulletSpawn
    {
        GameObject _object =  Instantiate(bullet, muzzleOfAGun.transform.position, rot);
        Bullet _bullet = _object.GetComponent<Bullet>();

        _bullet.ATK = Atk;
        _bullet.BulletLifeTime = bulletLifeTime;
        _bullet.target = (BulletTarget)_target;
        _bullet.IsDamage = _isDamage;
        _object.GetComponent<SpriteRenderer>().sprite = _controller.playerStatHandler.BulletSprite;
    }
}
