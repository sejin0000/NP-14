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
        if(collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet") && !playerStat.Invincibility)
        {
            float damage = collision.gameObject.GetComponent<Bullet>().ATK;
            playerStat.Damage(damage);
        }
    }
}
