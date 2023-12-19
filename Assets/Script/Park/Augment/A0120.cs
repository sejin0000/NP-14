using Photon.Pun;
using UnityEngine;


public class A0120 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            controller.OnRollEvent += CreateWater;
        }
    }
    // Update is called once per frame
    void CreateWater()
    {
        PhotonNetwork.Instantiate("AugmentList/A0120", this.transform.localPosition, Quaternion.identity);
    }

}
