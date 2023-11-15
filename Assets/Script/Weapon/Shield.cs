using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldSurvivalTime = 3;
    public float shieldHP= 20;
    //public float shieldPower;//커밋에서 내가 넣었다고 뜨는데 내가 넣은 기억이 없음 일단 주석처리후 나중에 확인해서 지우기
    private float time = 0;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > shieldSurvivalTime) 
        {
            Destroy();
        }
    }
    public void Initialized(float hp, float scale,float time)
    {
        transform.localScale =new Vector3(scale, scale, 0);
        shieldHP= hp;
        shieldSurvivalTime = time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet"))
        {
            shieldHP -= collision.gameObject.GetComponent<Bullet>().ATK;
            if (shieldHP < 0)
            {
                Destroy();
            }
            Destroy(collision.gameObject);
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
