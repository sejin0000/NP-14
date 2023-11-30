using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class A0217_1 : MonoBehaviour
{
    float buffAmount = 1;
    float time = 0;
    int maxtime = 5; //사라지는시간
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        if (targetstat != null)
        {
            target.Add(targetstat);
            buffAmount = 1.5f;
            targetstat.AtkSpeed.added += buffAmount;
            targetstat.Speed.added += buffAmount;
            Debug.Log("플레이어입장");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStatHandler targetstat = collision.GetComponent<PlayerStatHandler>();
        if (targetstat != null)
        {
            target.Remove(targetstat);
            targetstat.AtkSpeed.added -= buffAmount;
            targetstat.Speed.added -= buffAmount;
            Debug.Log("플레이어퇴장");
        }
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= maxtime)
        {
            goodbye();
        }
    }
    private void goodbye() 
    {
        if (target != null) 
        {
            Debug.Log($"{target.Count}");
            for (int i = 0; i< target.Count; ++i) 
            {
                target[i].AtkSpeed.added -= buffAmount;
                target[i].Speed.added -= buffAmount;
                target.Remove(target[i]);
            }
        }
        Destroy(gameObject);
    }
}
