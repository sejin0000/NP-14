using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class A0127 : MonoBehaviourPun
{
    //private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    //private CoolTimeController coolTimeController;
    int time = 5;
    float regenhp;
    WaitForSeconds autoTime;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerStat = GetComponent<PlayerStatHandler>();
            photonView.RPC("AutoHealingStart", RpcTarget.All);
            autoTime = new WaitForSeconds(time);
        }
    }
    private void OnEnable()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("AutoHealingStart", RpcTarget.All);
        }
    }
    // Update is called once per frame
    [PunRPC]
    void AutoHealingStart()
    {
        if (photonView.IsMine)
        {
            StopCoroutine("AutoHealing");
            StartCoroutine("AutoHealing");
        }
    }
    IEnumerator AutoHealing()
    {
        while (true)
        {
            //if(gameObject.SetActive() == true)
            if (playerStat.CurHP <= 0) 
            {
                yield return null;
            }
            playerStat.HPadd(regenhp);
            yield return autoTime;
        }
    }
}
