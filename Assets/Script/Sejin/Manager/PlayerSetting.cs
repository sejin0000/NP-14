using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetting : MonoBehaviour
{
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }


    public void InstantiatePlayer()
    {
        if (PV == null)
        {
            PV = GetComponent<PhotonView>();
        }

        string playerPrefabPath = "Pefabs/Player";
        GameManager.Instance.clientPlayer = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);


        int viewID = GameManager.Instance.clientPlayer.GetPhotonView().ViewID;
        PV.RPC("PlayerInfoDictionarySetting", RpcTarget.AllBuffered, viewID);
    }


    [PunRPC]
    private void PlayerInfoDictionarySetting(int viewID)
    {
        GameObject clientPlayer = PhotonView.Find(viewID).gameObject;
        GameManager.Instance.playerInfoDictionary.Add(viewID, clientPlayer.transform);
    }
}
