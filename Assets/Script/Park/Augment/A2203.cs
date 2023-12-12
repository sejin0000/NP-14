using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203 : MonoBehaviour
{
    private float time = 0;
    [SerializeField] private int maxtime;//������½ð� ����5��
    [SerializeField] private int healP;
    private List<PlayerStatHandler> target;

    int stack;
    private void Awake()
    {
        target= new List<PlayerStatHandler>();
        healP = 4;
        maxtime = 5;
        stack = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatHandler handler = collision.gameObject.GetComponent<PlayerStatHandler>();
        if (handler != null) 
        {
            target.Add(handler);
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStatHandler handler = collision.gameObject.GetComponent<PlayerStatHandler>();
        if (handler != null)
        {
            target.Remove(handler);
        }
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 1f) 
        {
            heal();
            stack++;
            time = 0f;
        }
        if (stack > maxtime) 
        {
            Destroy(gameObject);
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

}
