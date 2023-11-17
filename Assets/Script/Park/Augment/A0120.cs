using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class A0120 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            controller.OnRollEvent += CreateWater; // �߿��Ѻκ�
        }
    }
    // Update is called once per frame
    void CreateWater()
    {
        PhotonNetwork.Instantiate("AugmentList/A0120", this.transform.localPosition, Quaternion.identity);
    }

}
