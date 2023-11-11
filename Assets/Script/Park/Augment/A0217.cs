using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0217 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnEndRollEvent += MakeHeal;
        }

    }
    // Update is called once per frame
    void MakeHeal()
    {
        GameObject fire = PhotonNetwork.Instantiate("AugmentList/A2203", transform.localPosition, Quaternion.identity);
        A2203 a2203 = fire.GetComponent<A2203>();
        a2203.Init(playerStat);
    }
}
