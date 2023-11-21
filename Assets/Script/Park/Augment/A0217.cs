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
        GameObject fire = PhotonNetwork.Instantiate("AugmentList/A0217", transform.localPosition, Quaternion.identity);
        A0217_1 a2203 = fire.GetComponent<A0217_1>();
    }
}
