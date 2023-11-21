using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203 : MonoBehaviour
{
    float time = 0;
    int maxtime = 5;//������½ð� ����5��
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
            target[i].HPadd(healP);
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
