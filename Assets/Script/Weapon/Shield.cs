using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldSurvivalTime = 3;
    public float shieldHP;

    private void Start()
    {
        Invoke("Destroy", shieldSurvivalTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            shieldHP -= collision.gameObject.GetComponent<Bullet>().ATK;
            if (shieldHP < 0)
            {
                Destroy();
            }
            Destroy(collision.gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
