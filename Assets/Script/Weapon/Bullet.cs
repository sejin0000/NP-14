using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ATK;
    public float BulletLifeTime;
    public float BulletSpeed = 20;

    //��������
    public LayerMask target;

    void Start()
    {
        BulletLifeTime = Random.Range(BulletLifeTime * 0.15f, BulletLifeTime * 0.2f);
        Invoke("Destroy", BulletLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * BulletSpeed * Time.deltaTime);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        int collLayer = collision.gameObject.layer;

        Debug.Log("���̾ �浹��");

        Debug.Log("���̾� ����" + collLayer);
        //��Ʈ����
        //(1 << collLayer) ���̾� ��ȣ ������ ��ȯ(<<) : �ش��ϴ� ��Ʈ(�浹�� ���̾�)�� 1�� ����
        //(target & (1 << collLayer) Ÿ�� ���̾�鼭, �浹�� ���
        // != 0 : ��, ���ΰ��
        if ((target & (1 << collLayer)) != 0)
        {
            Debug.Log("Ÿ�� ���̾ �浹��");
            Destroy();
        }
    }
}
