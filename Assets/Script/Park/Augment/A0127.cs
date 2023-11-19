using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0127 : MonoBehaviourPun
{
    //private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    //private CoolTimeController coolTimeController;
    int autoTime = 5;
    float regenhp = 5f;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerStat = GetComponent<PlayerStatHandler>();
            photonView.RPC("AutoHealingStart", RpcTarget.All);
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
            playerStat.CurHP += regenhp;
            yield return new WaitForSeconds(autoTime);
            Debug.Log("코루틴 돌리는중 new 문제있으면 패치");
        }
    }
}
