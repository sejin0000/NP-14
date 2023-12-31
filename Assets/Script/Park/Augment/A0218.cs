using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A0218 : MonoBehaviour
{
    List<GameObject> target=new List<GameObject>();
    float time = 0;
    Vector3 dir;
    private void Update()
    {
        time += Time.deltaTime;

        foreach (GameObject star in target)
        {
            if (star != null && star.GetComponent<Rigidbody2D>()) 
            {
                dir = this.transform.position - star.transform.position;
                dir = dir * 10f;
                star.GetComponent<Rigidbody2D>().AddForce(dir);
            }
        }
        if (time >= 3) 
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.Remove(collision.gameObject);
    }
}
