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
            targetstat.AtkSpeed.added += buffAmount;
            targetstat.Speed.added += buffAmount;

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
        }
    }
    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= maxtime)
        {
            Destroy(gameObject);
        }
    }
}
