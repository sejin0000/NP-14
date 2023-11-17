using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0122 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStatHandler;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStatHandler= GetComponent<PlayerStatHandler>();
            controller.OnRollEvent += CreateFire; // 중요한부분
        }
    }
    // Update is called once per frame
    void CreateFire()
    {
        GameObject A =PhotonNetwork.Instantiate("AugmentList/A0122", this.transform.localPosition, Quaternion.identity);
        A0122_1 AB= A.GetComponent<A0122_1>();
        AB.damege = playerStatHandler.ATK.total;
    }
}
