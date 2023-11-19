using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletTarget
{
    Player,
    Enemy,
    All// �̺κ� ��ų������ �Ѵ��ľ��ؼ� �� �ݿ����Ѿߵɵ� �ٸ��е����� ���ϱ�
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
            Debug.Log($"���������� ���� {ATK}");
        }
        if (sniping) 
        {
            ATK += ATK * 0.1f * Time.deltaTime;
            Debug.Log($"���������� ���� {ATK}");
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�����׽�Ʈ�ݶ��̴�");
        Debug.Log($"�ޱ��׽�Ʈ {canAngle}");
        if (canAngle)
        {
            Debug.Log("111111111111");
            Vector3 income = _direction.normalized; // �Ի纤��
            Vector3 normal = collision.contacts[0].normal; // ��������
            Vector3 async = Vector3.Reflect(income, normal); // �ݻ纤��
            _direction = async.normalized;

        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�����׽�ƮƮ����");
    }
}
