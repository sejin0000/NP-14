using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0128_1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Bullet targetB = collision.GetComponent<Bullet>();
        if (targetB.targets.ContainsValue((int)BulletTarget.Player)) 
        {
            Destroy(targetB);
        }
    }
}
