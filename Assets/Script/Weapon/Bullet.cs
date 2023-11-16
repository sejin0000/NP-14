using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTarget
{
    Player,
    Enemy,
    All// 이부분 팀킬먹으면 둘다쳐야해서 올 반영시켜야될듯 다른분들한테 말하기
}

public class Bullet : MonoBehaviour
{
    public float ATK;
    public float BulletLifeTime;
    public float BulletSpeed = 15;
    public bool IsDamage = true;
    public bool canAngle = true;
    public BulletTarget target;
    Vector2 _direction;
    float time = 0f;

    public int BulletOwner;
    void Start()
    {
        BulletLifeTime = Random.Range(BulletLifeTime * 0.15f, BulletLifeTime * 0.2f);
        Invoke("Destroy", BulletLifeTime);
        _direction = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_direction * BulletSpeed * Time.deltaTime);
        time += Time.deltaTime;
        if (time>= BulletLifeTime) 
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") && canAngle)
        {
            Vector3 income = _direction; // 입사벡터
            Vector3 normal = collision.contacts[0].normal; // 법선벡터
            _direction = Vector3.Reflect(income, normal).normalized; // 반사벡터
        }
    }
}
