using Photon.Pun;
using System.Collections;
using UnityEngine;

public class A0127 : MonoBehaviourPun
{
    private PlayerStatHandler playerStat;
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
            StartCoroutine("AutoHealing");
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
            if (playerStat.CurHP <= 0) 
            {
                yield return null;
            }
            playerStat.HPadd(regenhp);
            yield return autoTime;
        }
    }
}
