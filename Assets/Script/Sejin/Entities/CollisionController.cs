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

        Bullet _bullet = collision.gameObject.gameObject.GetComponent<Bullet>();



        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && !playerStat.Invincibility && !playerStat.isDie && _bullet.target == BulletTarget.Player)
        {
            float damage = collision.gameObject.GetComponent<Bullet>().ATK;

            Debug.Log(_bullet.IsDamage);

            if (_bullet.IsDamage)
            {
                Debug.Log(playerStat.CurHP);
                playerStat.Damage(damage);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log(playerStat.CurHP);
                playerStat.HPadd(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
