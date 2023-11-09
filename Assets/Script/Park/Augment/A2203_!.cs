using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2203_1 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    GameObject Prefabs;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            controller.OnEndRollEvent += MakeHeal;
            Prefabs = Resources.Load<GameObject>("AugmentList/A2203");
        }

    }


    // Update is called once per frame
    void MakeHeal()
    {
        GameObject fire = PhotonNetwork.Instantiate("AugmentList/A2203",transform.localPosition,Quaternion.identity);
        //fire.transform.SetParent(player.transform);
        A2203 a2203 = fire.GetComponent<A2203>();
        //fire.transform.localPosition= playerStat.gameObject.transform.localPosition;
        a2203.Init(playerStat);
    }
}
