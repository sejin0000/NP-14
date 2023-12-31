using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviourPun
{
    private PlayerStatHandler playerStat;
    private PhotonView PV;
    private int LastHealedViewID; 

    // ADDED
    private float healedTotal;
    public float HealedTotal        // 걸려있는 컴포넌트에 따라 이벤트 시작 
    {
        get { return healedTotal; }
        set 
        {
            if (value != healedTotal)
            {
                healedTotal = value;
            }
        }
    }

    public event Action<float, int> OnHealedEvent;

    public bool CanPayBack;
    public bool CanSupport;

    public CapsuleCollider2D footCollider;
    public Rigidbody2D rigidbody;
    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();   
        PV = GetComponent<PhotonView>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Bullet>()) //무적처리
        {
            if (playerStat.isDie && collision.gameObject.GetComponent<Bullet>().canresurrection && this.gameObject.layer == 12)
            {
                Bullet _bullet = collision.gameObject.GetComponent<Bullet>();
                PhotonView photonView = PhotonView.Find(_bullet.BulletOwner);
                WeaponSystem stat = photonView.gameObject.GetComponent<WeaponSystem>();
                if (stat.canresurrection)
                {
                    playerStat.photonView.RPC("ImLive", RpcTarget.All);
                    int PvNum = _bullet.BulletOwner;

                    playerStat.photonView.RPC("thankyouLife", RpcTarget.All, PvNum);

                    if (MainGameManager.Instance != null)
                    {
                        MainGameManager.Instance.photonView.RPC("RemovePartyDeathCount", RpcTarget.All);
                    }

                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.PV.RPC("RemovePartyDeathCount", RpcTarget.All);
                    }
                }
                Destroy(collision.gameObject);
            }
        }

        if(collision.gameObject.layer == LayerMask.NameToLayer("AttackArea"))
        {
            //넉백

            float Boss_Dragon_atk = collision.gameObject.GetComponentInParent<BossAI_Dragon>().bossSO.atk;
            //playerStat.GiveDamege(Boss_Dragon_atk);
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
                }
                else
                {
                    playerStat._DebuffControl.Init(PlayerDebuffControl.buffName.Heal, 1f);
                    Debug.Log("체력 회복 ");
                    // ADD : 힐량 누적
                    damage = (damage + playerStat.CurHP > playerStat.HP.total)? playerStat.HP.total - playerStat.CurHP : damage;
                    photonView.RPC("AddHealAmount", RpcTarget.All, damage, _bullet.BulletOwner);
                    PhotonView.Find(_bullet.BulletOwner).RPC("InvokeHealedEvent", RpcTarget.All, damage, _bullet.BulletOwner);

                    playerStat.HPadd(damage);
                    if (_bullet.sniperAtkBuff) 
                    {
                        Debuff.Instance.GiveAtkBuff(gameObject);
                    }

                }
            }
            Destroy(collision.gameObject);
        }
    }


    [PunRPC]
    private void AddHealAmount(float healedAmount, int viewID)
    {
        HealedTotal += healedAmount;
        LastHealedViewID = viewID;
    }

    [PunRPC]
    private void InvokeHealedEvent(float healedAmount, int viewID)
    {
        OnHealedEvent?.Invoke(healedAmount, viewID);
    }

}
