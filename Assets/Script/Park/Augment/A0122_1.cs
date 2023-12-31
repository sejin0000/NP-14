using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0122_1 : MonoBehaviour
{
    public float time=0;
    public float damage=0;
    public int viewID;
    public void Init(int ViewId ,float Damage)
    {
        viewID=ViewId;
        damage=Damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject target = collision.gameObject;
            Debuff.Instance.GiveFire(target, damage, viewID);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameObject target = collision.gameObject;
            Debuff.Instance.GiveFire(target, damage, viewID);
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
