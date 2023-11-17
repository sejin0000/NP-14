using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0120_1 : MonoBehaviourPun
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
            if (collision.gameObject.tag == "Enemy")
            {
                GameObject target = collision.gameObject;
                Debuff.GiveIce(target);
            }
    }
}
