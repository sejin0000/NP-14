using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class A3107 : MonoBehaviourPun
{
    // Start is called before the first frame update
    int objSize;// ���ư��� ����ü ����
    public float circleR=1.5f; //������
    public float deg; //����
    public float objSpeed= 120f; //��� �ӵ�
    public GameObject[] target;
    public PlayerStatHandler playerStat;
    public float damege;
    public int PvNum;

    private void Start()
    {
        objSize = target.Length;
        transform.localPosition = Vector3.zero;
        objSpeed = 160f;
    }
    public void Init(GameObject pl)
    {
        GameObject player1 = pl;
        playerStat = player1.GetComponent<PlayerStatHandler>();
        PvNum = playerStat.photonView.ViewID;
        DamegeUpdate();
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
        if (photonView.IsMine) 
        {
            DamegeUpdate();
        }


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") && photonView.IsMine)
        {
            EnemyAI wjr = collision.GetComponent<EnemyAI>();
            wjr.PV.RPC("DecreaseHPByObject", RpcTarget.All, damege, PvNum);
        }
    }
    public void DamegeUpdate()
    {
        damege = playerStat.ATK.total * 0.8f;
    }
}