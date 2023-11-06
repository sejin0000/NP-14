using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    private TopDownCharacterController _controller;
    private PhotonView pv;
    public Transform muzzleOfAGun;


    private void Awake()
    {
        pv             = GetComponent<PhotonView>();
        _controller    = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        _controller.OnAttackEvent += Shooting;
    }

    public void Shooting()
    {
        for (int i = 0; i < _controller.playerStatHandler.LaunchVolume.total; i++)
        {
            pv.RPC("BS",RpcTarget.All);
        }
    }

    [PunRPC]
    public void BS()//BulletSpawn
    {
        GameObject go;
        Quaternion rot = muzzleOfAGun.transform.rotation;
        rot.eulerAngles += new Vector3(0, 0, Random.Range(-1 * _controller.playerStatHandler.BulletSpread.total, _controller.playerStatHandler.BulletSpread.total));// Áß¿äÇÔ

        go = PhotonNetwork.Instantiate("Pefabs/Bullet", muzzleOfAGun.transform.position, rot);

        go.GetComponent<Bullet>().ATK = _controller.playerStatHandler.ATK.total;
        go.GetComponent<Bullet>().BulletLifeTime = _controller.playerStatHandler.BulletLifeTime.total;
        go.GetComponent<SpriteRenderer>().sprite = _controller.playerStatHandler.BulletSprite;
    }
}
