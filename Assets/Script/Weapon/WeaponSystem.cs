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

    public int _viewID;

    private void Awake()
    {
        isDamage = true;
        bullet         = Resources.Load<GameObject>("Prefabs/Player/Bullet");
        pv             = GetComponent<PhotonView>();
        _controller    = GetComponent<TopDownCharacterController>();
        _viewID        = pv.ViewID;
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
            rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * _controller.playerStatHandler.BulletSpread.total, _controller.playerStatHandler.BulletSpread.total));// 중요함

            float _ATK = _controller.playerStatHandler.ATK.total;
            float _BLT = _controller.playerStatHandler.BulletLifeTime.total;
            int _target = (int)target;
            bool _isDamage = isDamage;


            pv.RPC("BS", RpcTarget.All, rot, _ATK, _BLT, _target, _isDamage, _viewID);
        }
    }

    [PunRPC]
    public void BS(Quaternion rot, float Atk, float bulletLifeTime,int _target, bool _isDamage, int _viewID)//BulletSpawn
    {
        Debug.Log("타겟");
        Debug.Log(_target);
        Debug.Log("데미지를 주는가?");
        Debug.Log(_isDamage);

        GameObject _object =  Instantiate(bullet, muzzleOfAGun.transform.position, rot);
        Bullet _bullet = _object.GetComponent<Bullet>();

        _bullet.ATK = Atk;
        _bullet.BulletLifeTime = bulletLifeTime;
        _bullet.target = (BulletTarget)_target;
        _bullet.IsDamage = _isDamage;
        _bullet.BulletOwner = _viewID;
        _object.GetComponent<SpriteRenderer>().sprite = _controller.playerStatHandler.BulletSprite;
    }
}
