using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3206_1 : MonoBehaviour
{
    public float shieldSurvivalTime = 5;
    public float shieldHP = 50;
    public float shieldPower;
    private float time = 0;
    public Rigidbody2D rigidbody2;
    private void Start()
    {
        time = 0;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > shieldSurvivalTime)
        {
            Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet _bullet = collision.GetComponent<Bullet>();
        if (_bullet != null)
        {
            if (_bullet.targets.ContainsValue((int)BulletTarget.Player)) 
            {
                shieldHP -= collision.gameObject.GetComponent<Bullet>().ATK;
                Debug.Log($"½¯µåÃ¼·Â{shieldHP}");
                if (shieldHP < 0)
                {
                    Destroy();
                }

            }
            Destroy(collision.gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
