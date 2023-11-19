using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0122_1 : MonoBehaviour
{
    public float time=0;
    public float damege=0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            GameObject target = collision.gameObject;
            Debuff.Instance.GiveFire(target, damege);
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 5f) 
        {
            Destroy(gameObject);
        }
    }
    
}
