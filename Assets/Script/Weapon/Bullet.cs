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
    public bool IsDamage = false;
    public bool canAngle = false;
    public bool fire;
    public bool water;
    public bool ice;
    public bool burn;
    public bool gravity;
    public bool Penetrate;
    public Dictionary<string, int> targets;
    //targets.Contains(BulletTarget.Enemy)
    Vector2 _direction;
    float time = 0f;


    public bool locator;
    public bool sniping;

    public int BulletOwner;

    private void Awake()
    {
        targets = new Dictionary<string, int>(); 
    }
    void Start()
    {        
        BulletLifeTime = Random.Range(BulletLifeTime * 0.15f, BulletLifeTime * 0.2f);
        //Invoke("Destroy", BulletLifeTime);
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
        if (locator) 
        {
            ATK -= ATK*0.1f * Time.deltaTime;
            Debug.Log($"강해지는중 현재 {ATK}");
        }
        if (sniping) 
        {
            ATK += ATK * 0.1f * Time.deltaTime;
            Debug.Log($"약해지는중 현재 {ATK}");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("접촉테스트콜라이더");
        Debug.Log($"앵글테스트 {canAngle}");
        if (canAngle)
        {
            Debug.Log("111111111111");
            Vector3 income = _direction.normalized; // 입사벡터
            Vector3 normal = collision.contacts[0].normal; // 법선벡터
            Vector3 async = Vector3.Reflect(income, normal); // 반사벡터
            _direction = async.normalized;

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("접촉테스트트리거");
    }
}
