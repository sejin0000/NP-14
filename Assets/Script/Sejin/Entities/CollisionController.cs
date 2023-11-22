using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : MonoBehaviourPun
{
    private PlayerStatHandler playerStat;
    private PhotonView PV;
    private int ViewID;

    // ADDED
    private float healedTotal;
    public float HealedTotal        // �ɷ��ִ� ������Ʈ�� ���� �̺�Ʈ ���� 
    {
        get { return healedTotal; }
        set 
        {
            if (value != healedTotal
                && CanPayBack)         // A1103 : ���̹�
            {                
                CallHealedEvent(value - healedTotal);
            }
            if (value != healedTotal
                && CanSupport)         // A1206 : ������ �������� ��
            {
                CallHealedEvent(value - healedTotal);
            }
            if (value != healedTotal)
            {
                healedTotal = value;
            }
        }
    }

    public event Action<float> OnHealedEvent;

    public bool CanPayBack;
    public bool CanSupport;

    private void Awake()
    {
        playerStat = GetComponent<PlayerStatHandler>();   
        PV = GetComponent<PhotonView>();
        ViewID = photonView.ViewID;
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
                    photonView.RPC("AddHealAmount", RpcTarget.All, damage);

                    playerStat.HPadd(damage);
                }
            }
            Destroy(collision.gameObject);
        }
    }

    public void CallHealedEvent(float healedAmount)
    {
        OnHealedEvent?.Invoke(healedAmount);
    }

    [PunRPC]
    private void AddHealAmount(float healedAmount)
    {
        HealedTotal += healedAmount;
    }
}
