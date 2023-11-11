using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float ATK;
    public float BulletLifeTime;
    public float BulletSpeed = 20;

    //내일하자
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

        Debug.Log("레이어에 충돌함");

        Debug.Log("레이어 정보" + collLayer);
        //비트연산
        //(1 << collLayer) 레이어 번호 이진수 변환(<<) : 해당하는 비트(충돌한 레이어)를 1로 만듬
        //(target & (1 << collLayer) 타겟 레이어면서, 충돌한 경우
        // != 0 : 즉, 참인경우
        if ((target & (1 << collLayer)) != 0)
        {
            Debug.Log("타겟 레이어에 충돌함");
            Destroy();
        }
    }
}
