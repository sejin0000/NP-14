using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3204_1 : MonoBehaviour
{
    PlayerStatHandler playerstat;
    int maxHp = 30;
    int hp = 30;//실드체


    // Start is called before the first frame update
    void Start()
    {
        //transform.localScale*playerstat.gameObject.transform.localScale
        GameObject scalechange = playerstat.gameObject;
        transform.localScale = scalechange.transform.localScale;
    }
    public void Init(PlayerStatHandler playerstatHandler)
    {
        playerstat = playerstatHandler;
        hp = maxHp;
    }
    public void reloading() 
    {
        hp = maxHp;
        Debug.Log("hp");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "적혹은적수타체") 
        {
            //
            deadcheck();
        }
    }
    void deadcheck() 
    {
        if (hp < 0) 
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    //애도공방 해놔야겠는데
}
