using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3204 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    GameObject Prefabs;
    A3204_1 nullcheck;

    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnEndRollEvent += make;

            nullcheck = null;
        }

    }
    void make()
    {
        photonView.RPC("Makeshield", RpcTarget.All);
    }
    // Update is called once per frame
    [PunRPC]
    void Makeshield()
    {
        if (photonView.IsMine)
        {
            if (nullcheck == null)
            {
                Prefabs = PhotonNetwork.Instantiate("AugmentList/A3204_1", Vector3.zero, Quaternion.identity);
                int PvNum= Prefabs.GetPhotonView().ViewID;
                nullcheck = Prefabs.GetComponent<A3204_1>();
                nullcheck.Init(playerStat);
                photonView.RPC("FindMaster", RpcTarget.All, PvNum);
            }
            else
            {
                nullcheck.reloading();
            }
        }
    }
    [PunRPC]
    private void FindMaster(int num)
    {
        PhotonView a = PhotonView.Find(num);
        a.transform.SetParent(this.gameObject.transform);
        a.transform.localPosition = Vector3.zero;
        //Prefabs.transform.SetParent(targetPlayer.transform);
    }
}
