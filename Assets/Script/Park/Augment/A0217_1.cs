using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class A0217_1 : MonoBehaviour
{
    float time = 0;
    int stack = 0;
    int maxtime = 5; //사라지는시간
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    public void Init(PlayerStatHandler playerstatHandler)
    {
        target.Add(playerstatHandler);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
            target.Add(targetstat);
            targetstat.AtkSpeed.added += 5;
            targetstat.Speed.added += 5;
            Debug.Log("플레이어입장");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
            target.Remove(targetstat);
            targetstat.AtkSpeed.added -=5;
            targetstat.Speed.added -= 5;
            Debug.Log("플레이어퇴장");
        }
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 1f)
        {
            stack++;
        }
        if (stack > maxtime)
        {
            goodbye();
        }
    }
    private void goodbye() 
    {
        if (target != null) 
        {
            for (int i = 0; i< target.Count; ++i) 
            {
                target[i].AtkSpeed.added -= 10;
                target[i].Speed.added -= 10;
                target.Remove(target[i]);
            }
        }
        Destroy(gameObject);
    }
}
