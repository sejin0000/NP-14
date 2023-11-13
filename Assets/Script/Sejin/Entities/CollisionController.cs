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

            if (_bullet.IsDamage)
            {
                playerStat.Damage(damage);
                Destroy(collision.gameObject);
            }
            else
            {
                playerStat.CurHP += damage;
            }
        }
    }
}
