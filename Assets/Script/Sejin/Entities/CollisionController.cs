using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private PlayerStatHandler playerStat;
    private PhotonView PV;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();   
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") && !playerStat.Invincibility && !playerStat.isDie && collision.gameObject.GetComponent<Bullet>().target == BulletTarget.Player&&PV.IsMine)
        {

            Bullet _bullet = collision.gameObject.GetComponent<Bullet>();

            float damage = collision.gameObject.GetComponent<Bullet>().ATK;

            Debug.Log("�ݸ��� �������� �ִ°�?");
            Debug.Log(_bullet.IsDamage);

            if (_bullet.IsDamage)
            {
                Debug.Log("������ ���� ");
                Debug.Log(playerStat.CurHP);
                playerStat.Damage(damage);
                Destroy(collision.gameObject);
            }
            else
            {
                Debug.Log("ü�� ȸ�� ");
                playerStat.HPadd(damage);
                Destroy(collision.gameObject);
            }
        }
    }
}
