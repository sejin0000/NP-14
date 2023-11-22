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
            Debug.Log($"약해지는중 현재 {ATK} 시간 {time}");
        }
        if (sniping) 
        {
            ATK += ATK * 2f * Time.deltaTime;
            Debug.Log($"강해지는중 현재 {ATK} 시간 {time}");
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
            _direction = income + normal * (-2 * Vector2.Dot(income, income));
            transform.right = _direction;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)//TO DEL 이 부분은 스나이퍼1201을 고려하여 작성하여야 합니다
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) //만약벽이라면
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("벽이라서삭제");
            return;
        }
        //만약 팀킬이 아닌 몬스터의 총알이라면 몬스터가 아니라면 삭제
        else if (targets.ContainsValue((int)BulletTarget.Player)
            && !targets.ContainsValue((int)BulletTarget.Enemy)
            && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("몬스터총알이 플레이어에 부딪혀서 삭제");
            return;
        }
        //만약 관통인 플레이어의 총알이라면 벽일때삭제
        else if (Penetrate)
        {
            Debug.Log("관통");
            return;
        }
        //플레이어의 총알이 몬스터에게부딪혀서 삭제
        else if (targets.ContainsValue((int)BulletTarget.Enemy) && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Invoke("Destroy", 0.01f);
            Debug.Log("플레이어의총알이 무언가에 부딪혀서 삭제");
            return;
        }
    }
}
