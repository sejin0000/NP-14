using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3107 : MonoBehaviour
{
    // Start is called before the first frame update
    int objSize;// ���ư��� ����ü ����
    public float circleR=1f; //������
    public float deg; //����
    public float objSpeed= 20f; //��� �ӵ�
    public GameObject[] target;

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
                target[i].transform.rotation = Quaternion.Euler(0, 0, (deg + (i * (360 / objSize))) * -1);
            }

        }
        else
        {
            deg = 0;
        }

    }
}