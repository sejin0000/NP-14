using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomEntry : MonoBehaviourPun
{
    public Button RoomEntryButton;
    public TextMeshProUGUI RoomNameText;
    public TextMeshProUGUI RoomMemberText;

    public void Initialize(string name, int memberNum)
    {
        RoomNameText.text = name;
        RoomMemberText.text = $"{memberNum} / 3";
        RoomEntryButton.onClick.AddListener(OnRoomEntryButtonClicked);
    }

    public void OnRoomEntryButtonClicked()
    {
        PhotonNetwork.JoinRoom(RoomNameText.text);
    }
}
