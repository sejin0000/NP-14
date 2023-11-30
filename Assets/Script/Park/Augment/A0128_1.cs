using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0128_1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && collision.GetComponent<Bullet>().targets.ContainsValue((int)BulletTarget.Player)) 
        {
            Destroy(collision.gameObject);
        }
    }
}
