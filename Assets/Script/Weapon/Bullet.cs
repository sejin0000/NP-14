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

    private HumanAttackintelligentmissile missile;

    public Dictionary<string, int> targets;



    //targets.Contains(BulletTarget.Enemy)
    public Vector2 _direction;
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
    public void MissileFire() 
    {
        missile = GetComponentInChildren<HumanAttackintelligentmissile>();
        missile.ready = true;
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
            ATK -= ATK*3f * Time.deltaTime;
            Debug.Log($"���������� ���� {ATK} �ð� {time}");
        }
        if (sniping) 
        {
            ATK += ATK * 2f * Time.deltaTime;
            Debug.Log($"���������� ���� {ATK} �ð� {time}");
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
            _direction = income + normal * (-2 * Vector2.Dot(income, income));
            transform.right = _direction;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//TO DEL �� �κ��� ��������1201�� ����Ͽ� �ۼ��Ͽ��� �մϴ�
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) //���ຮ�̶��
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("���̶󼭻���");
            return;
        }
        //���� ��ų�� �ƴ� ������ �Ѿ��̶�� ���Ͱ� �ƴ϶�� ����
        else if (targets.ContainsValue((int)BulletTarget.Player)
            && !targets.ContainsValue((int)BulletTarget.Enemy)
            && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("�����Ѿ��� �÷��̾ �ε����� ����");
            return;
        }
        //���� ������ �÷��̾��� �Ѿ��̶�� ���϶�����
        else if (Penetrate)
        {
            Debug.Log("����");
            return;
        }
        //�÷��̾��� �Ѿ��� ���Ϳ��Ժε����� ����
        else if (targets.ContainsValue((int)BulletTarget.Enemy) && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("�÷��̾����Ѿ��� ���𰡿� �ε����� ����");
            return;
        }
    }
}
