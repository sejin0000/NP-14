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
                Debug.Log($"���� ���� ������ : {shieldHP - value}");
                ShieldDamage = shieldHP - value;
                shieldHP = value;
            }
            else if (value > shieldHP)
            {
                Debug.Log($"���� ���� : {value}");
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
            Debug.Log($"Ÿ���� enemey ID : {value}");
        }
    }

    public float ShieldDamage;
    private float reflectCoeff;
    //public float shieldPower;//Ŀ�Կ��� ���� �־��ٰ� �ߴµ� ���� ���� ����� ���� �ϴ� �ּ�ó���� ���߿� Ȯ���ؼ� �����
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
            Debug.Log($"�ݻ��� : {reflectCoeff}");
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
