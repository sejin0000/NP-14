using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0128 : MonoBehaviour
{
    // Start is called before the first frame update
    int objSize;// ���ư��� ����ü ����
    public float circleR = 1f; //������
    private float deg; //����
    public float objSpeed = 140f; //��� �ӵ�
    public GameObject[] target;
    public PlayerStatHandler playerStat;

    private void Start()
    {
        objSize = target.Length;
        transform.localPosition = Vector3.zero;
    }
    void Update()
    {
        deg += Time.deltaTime * objSpeed;
        if (deg < 360)
        {
            for (int i = 0; i < objSize; i++)
            {
                var rad = Mathf.Deg2Rad * (deg + (i * (360 / objSize)));
                var x = circleR * Mathf.Sin(rad);
                var y = circleR * Mathf.Cos(rad);
                target[i].transform.position = transform.position + new Vector3(x, y);
            }
        }
        else
        {
            deg = 0;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        //{
        //    Bullet a = collision.GetComponent<Bullet>();
        //    if (collision.gameObject.GetComponent<Bullet>().targets.ContainsValue((int)BulletTarget.Player))
        //    {
        //        Destroy(collision.gameObject);
        //    }

        //}
    }
}
