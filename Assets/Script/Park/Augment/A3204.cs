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
    GameObject nullcheck;

    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnEndRollEvent += make;
            Prefabs = Resources.Load<GameObject>("AugmentList/A3204_1");
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
                GameObject shield = Instantiate(Prefabs, transform);
                A3204_1 a3204_1 = shield.GetComponent<A3204_1>();
                a3204_1.Init(playerStat);
                nullcheck = shield;
            }
            else
            {
                Debug.Log("실드 재생");
            }
        }
    }
}
