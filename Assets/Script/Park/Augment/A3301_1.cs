using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A3301_1 : MonoBehaviour
{
    public float time = 0f;
    public float shieldHP;
    public float shieldSurvivalTime;
    int viewID;
    //public LayerMask
    // Start is called before the first frame update

    // Update is called once per frame
    public void Init(int viewid)
    {
        viewID=viewid;
        shieldHP = 1f;
        shieldSurvivalTime = 0.75f;
    }
    private void Update()
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
        Bullet _bullet = collision.gameObject.GetComponent<Bullet>();
        if (_bullet != null
            && _bullet.targets.ContainsValue((int)BulletTarget.Player))
        { 
            _bullet.BulletLifeTime = 10f;//�ð� �޾ƿ;߰ڴµ� �𸣰����ϱ� �� 10�ʶ����� �Ҹ�����
            _bullet.targets.Remove("Player");            
            _bullet.targets["Enemy"] = (int)BulletTarget.Enemy;
            _bullet.BulletOwner = viewID;
            //�Ʒ��� �ݻ�
            collision.gameObject.transform.right = -collision.gameObject.transform.right;

             Destroy();
        }
    }
}
