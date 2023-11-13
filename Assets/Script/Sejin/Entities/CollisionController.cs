using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private PlayerStatHandler playerStat;


    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        Bullet _bullet = collision.gameObject.GetComponent<Bullet>();



        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && !playerStat.Invincibility && !playerStat.isDie && _bullet.target == BulletTarget.Player)
        {
            float damage = collision.gameObject.GetComponent<Bullet>().ATK;

            Debug.Log("콜리전 데미지를 주는가?");
            Debug.Log(_bullet.IsDamage);

            if (_bullet.IsDamage)
            {
                Debug.Log("데미지 받음 ");
                Debug.Log(playerStat.CurHP);
                playerStat.Damage(damage);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("체력 회복 ");
                playerStat.HPadd(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
