using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3107 : MonoBehaviour
{
    // Start is called before the first frame update
    int objSize;// ���ư��� ����ü ����
    public float circleR=1f; //������
    public float deg; //����
    public float objSpeed= 120f; //��� �ӵ�
    public GameObject[] target;
    public PlayerStatHandler playerStat;

    private void Start()
    {
        objSize = target.Length;
        transform.localPosition = Vector3.zero;
        objSpeed = 120f;
    }
    public void Init(GameObject pl)
    {
        GameObject player1 = pl;
        playerStat = player1.GetComponent<PlayerStatHandler>();
        MainGameManager.Instance.OnGameStartedEvent += damege;
        damege();
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
    void damege() 
    {
        for (int i = 0; i < target.Length; ++i)
        {
            target[i].GetComponent<A3107_1>().DamegeUpdate(playerStat.ATK.total);
        }
    }
}