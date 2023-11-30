using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3204_1 : MonoBehaviour
{
    PlayerStatHandler playerstat;
    int maxHp = 15;
    float hp = 15;//Ω«µÂ√º
    public void Init(PlayerStatHandler playerstatHandler)
    {
        playerstat = playerstatHandler;
        hp = maxHp;
        GameObject scalechange = playerstat.gameObject;
        transform.localScale = scalechange.transform.localScale;
    }
    public void reloading() 
    {
        hp = maxHp;
        Debug.Log("hp");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet!=null && bullet.targets.ContainsValue((int)BulletTarget.Player)) 
        {     
                deadcheck(bullet.ATK);
                Destroy(collision.gameObject);
        }

    }
    void deadcheck(float a) 
    {
        hp -= a;
        if (hp < 0) 
        {
            Destroy(gameObject);
        }
    }
}
