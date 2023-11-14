using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2103 : MonoBehaviourPun
{
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    PlayerStatHandler me;
    public GameObject Player;
    private TopDownCharacterController controller;

    public float power;
    public float oldPower;

    int runtime = 1;

    public void Init()
    {
        me = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        controller = GetComponent<TopDownCharacterController>();
        target = null;
        oldPower = 0;
        MainGameManager.Instance.OnGameStartedEvent += Restart;
        photonView.RPC("Check",RpcTarget.All);
    }
    public void Restart()
    {
        target = null;
    }
    IEnumerator Check()
    {
        if (photonView.IsMine) 
        {
            while (true) 
            {
                me.ATK.added -= oldPower;
                me.AtkSpeed.added -= oldPower;
                me.AmmoMax.added -= oldPower;
                me.BulletSpread.added += oldPower;
                me.Speed.added -= oldPower;

                power = target.Count*0.1f;

                me.ATK.added += power;
                me.AtkSpeed.added += power;
                me.AmmoMax.added += power;
                me.BulletSpread.added -= power;
                me.Speed.added += power;

                oldPower = power;

                yield return runtime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !target.Contains(collision.GetComponent<PlayerStatHandler>()) && (collision.GetComponent<PlayerStatHandler>() != null))
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("적입장");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("적퇴장");
        }
    }
}
