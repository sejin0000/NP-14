using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A0218 : MonoBehaviour
{
    List<GameObject> target=new List<GameObject>();
    float time = 0;
    Vector2 a;
    Vector3 dir;
    private void Awake()
    {
        //처음에 타겟 리스트 안들어갈거같음 이미안에있는애들 레이쏴서 하기 고려용 주석
    }
    private void Update()
    {
        time += Time.deltaTime;

        foreach (GameObject star in target)
        {
            dir = this.transform.position - star.transform.position;
            dir = dir * 0.1f * Time.deltaTime;
            star.GetComponent<Rigidbody2D>().AddForce(dir);
        }
        if (time >= 3) 
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        target.Add(collision.gameObject);
        Debug.Log("입장체크");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        target.Remove(collision.gameObject);
        Debug.Log("퇴장체크");
    }
}
