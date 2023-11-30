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
                Debug.Log($"쉴드 받은 데미지 : {shieldHP - value}");
                ShieldDamage = shieldHP - value;
                shieldHP = value;
                SendShieldHP();
            }
            else if (value > shieldHP)
            {
                Debug.Log($"쉴드 생성 : {value}");
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
            Debug.Log($"타격한 enemey ID : {value}");
        }
    }

    public float ShieldDamage;
    private float reflectCoeff;
    //public float shieldPower;//커밋에서 내가 넣었다고 뜨는데 내가 넣은 기억이 없음 일단 주석처리후 나중에 확인해서 지우기
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
        //if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>())
        //    && transform.parent.GetComponent<A3302>() != null)
        //{
        //    PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        //    target.Add(targetstat);
        //    buffAmount = transform.parent.GetComponent<A3302>().BuffAmount;
        //    targetstat.AtkSpeed.added += buffAmount;
        //    targetstat.Speed.added += buffAmount;
        //    Debug.Log("플레이어입장");
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
            Debug.Log("플레이어입장");
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
            Debug.Log("플레이어퇴장");
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
