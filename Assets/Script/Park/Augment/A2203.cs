using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203 : MonoBehaviour
{
    private float time = 0;
    [SerializeField] private int maxtime;//사라지는시간 현재5초
    [SerializeField] private int healP;
    private List<PlayerStatHandler> target;

    private void Awake()
    {
        target= new List<PlayerStatHandler>();
        healP = 2;
        maxtime = 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어입장");
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어퇴장");
        }
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 1f) 
        {
                heal();
        }
        if (time > maxtime) 
        {
            goodbye();
        }
    }
    private void heal() 
    {
        for (int i = 0; i < target.Count; ++i) 
        {
            if (!target[i].isDie) 
            {
                target[i].HPadd(healP);
            }
        }
    }
    private void goodbye()
    {
        if (target != null)
        {
            for (int i = 0; i < target.Count; ++i)
            {
                target.Remove(target[i]);
            }
        }
        Destroy(gameObject);
    }
}
