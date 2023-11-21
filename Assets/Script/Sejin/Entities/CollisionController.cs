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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") 
            && !playerStat.Invincibility 
            && !playerStat.isDie
            && !playerStat.isRegen
            && collision.gameObject.GetComponent<Bullet>().targets.ContainsValue((int)BulletTarget.Player))
        {
            if (PV.IsMine)
            {

                Bullet _bullet = collision.gameObject.GetComponent<Bullet>();

                float damage = collision.gameObject.GetComponent<Bullet>().ATK;

                Debug.Log("�ݸ��� �������� �ִ°�?");
                Debug.Log(_bullet.IsDamage);

                if (_bullet.IsDamage)
                {                    
                    Debug.Log($"������ ���� : {damage} / ���� ü�� : {playerStat.CurHP} ");
                    playerStat.Damage(damage);
                }
                else
                {
                    Debug.Log("ü�� ȸ�� ");
                    playerStat.HPadd(damage);
                    //if ( bool �� �ް� )
                    //{
                    //    playerStat.HPadd(damage);
                    //}
                    //else
                    //{
                    //    playerStat.ATK.Add(damage);
                    //}
                }
            }
            Destroy(collision.gameObject);
        }
    }
}
