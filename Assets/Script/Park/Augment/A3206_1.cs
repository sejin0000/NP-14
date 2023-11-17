using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3206_1 : MonoBehaviour
{
    public float shieldSurvivalTime = 5;
    public float shieldHP = 50;
    public float shieldPower;
    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > shieldSurvivalTime)
        {
            Destroy();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
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
