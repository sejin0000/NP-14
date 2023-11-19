using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A0120_1 : MonoBehaviourPun
{
    public float time = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("몬스터 스프라이트 변경시 코드 변경");
            Debuff.Instance.GiveIce(collision.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("몬스터 스프라이트 변경시 코드 변경");
            Debuff.Instance.GiveIce(collision.gameObject);
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
