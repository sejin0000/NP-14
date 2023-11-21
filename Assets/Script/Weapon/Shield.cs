using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [HideInInspector] PlayerStatHandler playerStat;
    [HideInInspector] Player2Skill playerSkill;

    [Header("ShieldInfo")]
    public float shieldSurvivalTime = 3;
    private float shieldHP;
    public float ShieldHP
    {
        get { return shieldHP; }
        set
        {
            if (value < shieldHP) 
            {
                Debug.Log($"쉴드 받은 데미지 : {shieldHP - value}");
                ShieldDamage = shieldHP - value;
                shieldHP = value;
            }
            else if (value > shieldHP)
            {
                Debug.Log($"쉴드 생성 : {value}");
                shieldHP = value;
            }
        }
    }

    private int targetID;
    public int TargetID
    {
        get { return targetID; }
        set
        {
            targetID = value;
            Debug.Log($"타격한 enemey ID : {value}");
        }
    }

    public float ShieldDamage;
    private float reflectCoeff;
    //public float shieldPower;//커밋에서 내가 넣었다고 뜨는데 내가 넣은 기억이 없음 일단 주석처리후 나중에 확인해서 지우기
    private float time = 0;

    private void Start()
    {
        playerStat = transform.parent.GetComponent<PlayerStatHandler>();
        playerSkill = transform.parent.GetComponent<Player2Skill>();        
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > shieldSurvivalTime) 
        {
            Destroy();
        }
    }
    public void Initialized(float hp, float scale,float time)
    {
        transform.localScale =new Vector3(scale, scale, 0);
        ShieldHP= hp;
        shieldSurvivalTime = time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")
            && collision.gameObject.GetComponent<Bullet>().targets.ContainsValue((int)BulletTarget.Player))
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();
            TargetID = bullet.BulletOwner;            
            ShieldHP -= bullet.ATK;

            reflectCoeff = playerSkill.ReflectCoeff;
            Debug.Log($"반사계수 : {reflectCoeff}");
            playerStat.CallReflectEvent(ShieldDamage, TargetID);
            ShieldHP += bullet.ATK * reflectCoeff;

            if (ShieldHP < 0)
            {
                Destroy();
                Destroy(collision.gameObject);
                return;
            }
            Destroy(collision.gameObject);   
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
