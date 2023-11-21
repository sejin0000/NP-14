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

                Debug.Log("콜리전 데미지를 주는가?");
                Debug.Log(_bullet.IsDamage);

                if (_bullet.IsDamage)
                {                    
                    Debug.Log($"데미지 받음 : {damage} / 남은 체력 : {playerStat.CurHP} ");
                    playerStat.Damage(damage);
                }
                else
                {
                    Debug.Log("체력 회복 ");
                    playerStat.HPadd(damage);
                    //if ( bool 값 받고 )
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
