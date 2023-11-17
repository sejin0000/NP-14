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
        //ó���� Ÿ�� ����Ʈ �ȵ��Ű��� �̹̾ȿ��ִ¾ֵ� ���̽��� �ϱ� ����� �ּ�
    }
    private void Update()
    {
        time += Time.deltaTime;

        foreach (GameObject star in target)
        {
            float dis = Vector3.Distance(this.transform.position, star.transform.position);

                // �༺���κ��� ��Ȧ�� ���ϴ� ������ ���Ѵ�.
                dir = this.transform.position - star.transform.position;
                // �༺�� ��ġ�� ��Ȧ�� �������� õõ�� �̵���Ų��.
                star.transform.position += dir * 0.1f * Time.deltaTime;
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
