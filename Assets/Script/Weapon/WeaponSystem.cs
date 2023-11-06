using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private PhotonView pv;

    public GameObject bullet;
    public Transform muzzleOfAGun;

    private Stats launchVolume;
    private Stats bulletSpread;
    private Stats bulletLifeTime;
    private Stats atk;


    private void Awake()
    {
        pv             = GetComponent<PhotonView>();
        _controller    = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {

        launchVolume   = _controller.playerStatHandler.LaunchVolume;
        bulletSpread   = _controller.playerStatHandler.BulletSpread;
        bulletLifeTime = _controller.playerStatHandler.BulletLifeTime;
        atk            = _controller.playerStatHandler.ATK;

        _controller.OnAttackEvent += Shooting;
    }

    public void Shooting()
    {
        for (int i = 0; i < launchVolume.total; i++)
        {
            pv.RPC("BS",RpcTarget.All);
        }
    }

    [PunRPC]
    public void BS()//BulletSpawn
    {
        GameObject go;
        Quaternion rot = muzzleOfAGun.transform.rotation;
        rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * bulletSpread.total, bulletSpread.total));// Áß¿äÇÔ

        go = PhotonNetwork.Instantiate("Pefabs/Bullet", muzzleOfAGun.transform.position, rot);

        go.GetComponent<Bullet>().ATK = atk.total;
        go.GetComponent<Bullet>().BulletLifeTime = bulletLifeTime.total;
        go.GetComponent<SpriteRenderer>().sprite = _controller.playerStatHandler.BulletSprite;
    }
}
