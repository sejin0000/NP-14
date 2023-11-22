using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    private PlayerStatHandler playerStat;
    private PhotonView PV;

    public CapsuleCollider2D footCollider;
    public Rigidbody2D rigidbody;
    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();   
        PV = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(playerStat.isDie && collision.gameObject.GetComponent<Bullet>().canresurrection && this.gameObject.layer ==12)
        {
            Bullet _bullet = collision.gameObject.GetComponent<Bullet>();
            PhotonView photonView = PhotonView.Find(_bullet.BulletOwner);
            WeaponSystem stat = photonView.gameObject.GetComponent<WeaponSystem>();
            if (stat.canresurrection) 
            {
                playerStat.Regen(playerStat.HP.total);
                this.gameObject.layer = 8;
                int PvNum = _bullet.BulletOwner;

                playerStat.photonView.RPC("thankyouLife",RpcTarget.All, PvNum);

                if (MainGameManager.Instance != null) 
                {
                    MainGameManager.Instance.photonView.RPC("RemovePartyDeathCount",RpcTarget.All);
                }
            }
        }


        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet") 
            && !playerStat.Invincibility 
            && !playerStat.isDie
            && !playerStat.isRegen
            && collision.gameObject.GetComponent<Bullet>().targets.ContainsValue((int)BulletTarget.Player))
        {
            if (PV.IsMine)
            {

                Bullet _bullet = collision.gameObject.GetComponent<Bullet>();

                float damage = _bullet.ATK;
                int targetID = _bullet.BulletOwner;

                if (_bullet.IsDamage)
                {
                    if (playerStat.CanReflect)
                    {
                        playerStat.CallReflectEvent(damage, targetID);
                        damage *= (1 - playerStat.ReflectCoeff);
                    }
                    playerStat.Damage(damage);
                    Debug.Log($"데미지 받음 : {damage} / 남은 체력 : {playerStat.CurHP} ");
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
                    if (_bullet.sniperAtkBuff) 
                    {
                        Debuff.Instance.GiveAtkBuff(gameObject);
                    }
                }
            }
            Destroy(collision.gameObject);
        }
    }

}
