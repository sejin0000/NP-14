using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3301_1 : MonoBehaviour
{
    public float time = 0f;
    public float shieldHP = 1f;
    public float shieldSurvivalTime =0.5f;
    public LayerMask 
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= shieldSurvivalTime) 
        {
            Destroy();
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
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
            //collision.gameObject.layer =

            //아래는 반사
            collision.gameObject.transform.right = -collision.gameObject.transform.right;
        }
    }
}
