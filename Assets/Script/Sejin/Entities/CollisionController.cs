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
    public float HealedTotal        // �ɷ��ִ� ������Ʈ�� ���� �̺�Ʈ ���� 
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
                    Debug.Log($"������ ���� : {damage} / ���� ü�� : {playerStat.CurHP} ");
                }
                else
                {
                    Debug.Log("ü�� ȸ�� ");
                    // ADD : ���� ����
                    damage = (damage + playerStat.CurHP > playerStat.HP.total)? playerStat.HP.total - playerStat.CurHP : damage;
                    photonView.RPC("AddHealAmount", RpcTarget.All, damage, _bullet.BulletOwner);
                    PhotonView.Find(_bullet.BulletOwner).RPC("InvokeHealedEvent", RpcTarget.All, damage, _bullet.BulletOwner);

                    playerStat.HPadd(damage);
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
