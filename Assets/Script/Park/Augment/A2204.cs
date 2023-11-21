using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2204 : MonoBehaviourPun
{
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    PlayerStatHandler me;
    public GameObject Player;
    private TopDownCharacterController controller;
        // Update is called once per frame
    void TogetherParty()
    {
        for (int i = 0; i < target.Count; ++i) 
        {
            Debuff.Instance.GiveLowSteamPack(target[i].gameObject);
        }
    }
    
    public void Init()
    {
        me = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        controller = me.gameObject.GetComponent<TopDownCharacterController>();
        controller.OnSkillEvent += TogetherParty;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>()) && (collision.GetComponent<PlayerStatHandler>()!=null))
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("2204 현재 염려 되는 버그는 이트리거가 맞는것이다 이경우 리지드바디 키네마틱을 추가해줘");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("2204 현재 염려 되는 버그는 이트리거가 맞는것이다 이경우 리지드바디 키네마틱을 추가해줘");
        }
    }
}
