using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1107 : MonoBehaviour
{
    float time = 0;
    PlayerStatHandler target;
    private void OnTriggerStay2D(Collider2D collision)
    {
        time += Time.deltaTime;
        if (collision.tag == "Player") 
        {
            target = collision.GetComponent<PlayerStatHandler>();
        }
    }
}
