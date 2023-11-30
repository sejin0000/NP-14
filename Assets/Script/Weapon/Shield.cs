using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [HideInInspector] PlayerStatHandler playerStat;
    [HideInInspector] Player2Skill playerSkill;
    public List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    public List<int> InShieldViewIDList;
    private float buffAmount;

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
                SendShieldHP();
            }
            else if (value > shieldHP)
            {
                Debug.Log($"���� ���� : {value}");
                shieldHP = value;
                SendShieldHP();
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
        target = new List<PlayerStatHandler>();
        InShieldViewIDList = new List<int>();
        playerStat = transform.parent.GetComponent<PlayerStatHandler>();
        playerSkill = transform.parent.GetComponent<Player2Skill>();
        if (transform.parent.GetComponent<A3302>() != null)
        {
            buffAmount = transform.parent.GetComponent<A3302>().BuffAmount;
        }
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
        //if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>())
        //    && transform.parent.GetComponent<A3302>() != null)
        //{
        //    PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        //    target.Add(targetstat);
        //    buffAmount = transform.parent.GetComponent<A3302>().BuffAmount;
        //    targetstat.AtkSpeed.added += buffAmount;
        //    targetstat.Speed.added += buffAmount;
        //    Debug.Log("�÷��̾�����");
        //}

        PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        
        if (targetstat != null)
        {
            target.Add(targetstat);
            SendShieldHP();
            targetstat.IsInShield = true;
            int targetViewID = collision.gameObject.GetPhotonView().ViewID;

            A3302 a3302 = transform.parent.GetComponent<A3302>();
            if (a3302 != null)
            {
                buffAmount = a3302.BuffAmount;
                targetstat.AtkSpeed.added += buffAmount;
                targetstat.Speed.added += buffAmount;
            }
            Debug.Log("�÷��̾�����");
        }
    }

    public void SendShieldHP()
    {
        foreach (PlayerStatHandler playerStat in target)
        {
            playerStat.InShieldHP = ShieldHP;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        if (targetstat != null)
        {
            target.Remove(targetstat);
            targetstat.InShieldHP = 0;
            targetstat.IsInShield = false;
            int targetViewID = collision.gameObject.GetPhotonView().ViewID;

            A3302 a3302 = transform.parent.GetComponent<A3302>();
            if (a3302 != null)
            {
                buffAmount = a3302.BuffAmount;
                targetstat.AtkSpeed.added -= buffAmount;
                targetstat.Speed.added -= buffAmount;
            }
            Debug.Log("�÷��̾�����");
        }
    }

    private void Destroy()
    {
        if (transform.parent.GetComponent<A3302>() != null)
        {
            foreach (PlayerStatHandler playerStat in target)
            {
                playerStat.InShieldHP = 0;
            }
            buffAmount = transform.parent.GetComponent<A3302>().BuffAmount;
            for (int i = 0; i < target.Count; ++i)
            {
                target[i].AtkSpeed.added -= buffAmount;
                target[i].Speed.added -= buffAmount;
                target.Remove(target[i]);
            }
        }
        Destroy(gameObject);
    }
}
