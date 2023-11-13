using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTarget
{
    Player,
    Enemy
}

public class Bullet : MonoBehaviour
{
    public float ATK;
    public float BulletLifeTime;
    public float BulletSpeed = 20;
    public bool IsDamage = true;
    public BulletTarget target;

    void Start()
    {
        BulletLifeTime = Random.Range(BulletLifeTime * 0.15f, BulletLifeTime * 0.2f);
        Invoke("Destroy", BulletLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * BulletSpeed * Time.deltaTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy();
        }
    }
}
