using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0303 : MonoBehaviourPun
{
    // �н� ��ȯ ������Ʈ
    public GameObject Partner;
    public GameObject Player;

    public int viewID;

    public void Awake()
    {
        
    }
    //
    public void SpawnPartner()
    {
        if (MainGameManager.Instance != null) 
        {
            Player = MainGameManager.Instance.InstantiatedPlayer;
            viewID = Player.GetPhotonView().ViewID;
        }        
    }

    [PunRPC]
    public void SetInfo(int viewID)
    {
        var parentTransform = 
        //Partner = Instantiate(Player, )
    }
}
