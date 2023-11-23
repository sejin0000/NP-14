using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class A1106 : MonoBehaviourPun
{
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    PlayerStatHandler StatHandler;
    public GameObject Player;
    private TopDownCharacterController controller;
    public void Init()
    {
        if(photonView.IsMine)
        {
            Player = transform.parent.gameObject;
            StatHandler = Player.GetComponent<PlayerStatHandler>();
            controller = Player.GetComponent<TopDownCharacterController>();
            StatHandler.GetDamege += divide;
        }
    }
    public void divide(float damege) 
    {
        if (target.Contains(StatHandler)) 
        {
            target.Remove(StatHandler);
        }
        int count = target.Count+1;
        StatHandler.DamegeTemp = damege / count;
        float giveDamege = StatHandler.DamegeTemp;
        for (int i = 0; i < target.Count; ++i) 
        {
            target[i].photonView.RPC("GiveDamege", RpcTarget.All,giveDamege);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>()) && (collision.GetComponent<PlayerStatHandler>()))
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("�÷��̾�����");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("�÷��̾�����");
        }
    }
}
