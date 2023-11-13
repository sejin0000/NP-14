using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0303 : MonoBehaviourPun
{
    // 분신 소환 오브젝트
    public GameObject Partner;
    public GameObject Player;

    public int viewID;
    public string PartnerLayerName = "Partner";

    public void SpawnPartner()
    {
        if (MainGameManager.Instance != null) 
        {
            Player = MainGameManager.Instance.InstantiatedPlayer;
            viewID = Player.GetPhotonView().ViewID;
            string playerPrefabPath = "Pefabs/Player";
            Player = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
            Player.GetComponent<PlayerStatHandler>().ATK.coefficient *= 0.5f;
            Player.layer = LayerMask.NameToLayer(PartnerLayerName);
        }        
    }
    }    
}
