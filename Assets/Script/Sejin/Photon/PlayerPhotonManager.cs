using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhotonManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("辑滚 立加");
        PhotonNetwork.JoinOrCreateRoom("规", new RoomOptions { MaxPlayers = 5 }, null);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("冯 立加");
        PhotonNetwork.Instantiate("Pefabs/Player", Vector2.zero, Quaternion.identity);
    }
}
