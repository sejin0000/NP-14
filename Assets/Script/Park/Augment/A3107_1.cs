using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3107_1 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy") 
        {
            //대충 데미지 주기
        }
    }
}
