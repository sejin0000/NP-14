using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ATK;
    public float BulletLifeTime;
    public float BulletSpeed = 20;


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

    void Destroy()
    {
        Destroy(gameObject);
    }
}
