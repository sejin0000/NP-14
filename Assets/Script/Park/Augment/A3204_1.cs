using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3204_1 : MonoBehaviour
{
    PlayerStatHandler playerstat;
    int maxHp = 15;
    float hp = 15;//실드체
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
        if (collision.gameObject.GetComponent<Bullet>()) 
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet")
            && bullet.targets.ContainsValue((int)BulletTarget.Player))
            {
                deadcheck(bullet.ATK);
                Destroy(collision.gameObject);
            }
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

    // Update is called once per frame
    //애도공방 해놔야겠는데
}
