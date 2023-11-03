using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203 : MonoBehaviour
{
    float time = 0;
    int stack = 5;//��/ȸ
    List<PlayerStatHandler> target= new List<PlayerStatHandler>();
    int healP=2;
    public void Init(PlayerStatHandler playerstatHandler)
    {
        target.Add(playerstatHandler);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("�÷��̾�����");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("�÷��̾�����");
        }

    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 1f) 
        {
                heal();
                stack++;
        }
        if (stack >5) 
        {
            Destroy(gameObject);
        }
    }
    private void heal() 
    {
        for (int i = 0; i < target.Count; ++i) 
        {
            target[i].CurHP += healP;
        }
    }
}
