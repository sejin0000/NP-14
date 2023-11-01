using ExitGames.Client.Photon;
using Photon.Pun;
//using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviourPun
{
    [Header("button")]
    public Button ReadyButton;
    public Button StartButton;
    public Button BackButton;

    //private object isPlayerReady;    

    public void Start()
    {
        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady))
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "IsPlayerReady", false } });
        }

        StartButton.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.gameObject.SetActive(false);
        }

        // 레디 버튼 적용
        ReadyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    public void SetPlayerReady(bool playerReady)
    {
        if (playerReady)
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "취소";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(130 / 255f, 241 / 255f, 96 / 255f);
        }
        else
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "준비";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(255 / 255f, 182 / 255f, 182 / 255f);
        }
    }

    public void OnReadyButtonClicked()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsPlayerReady", out object isPlayerReady))
        {
            isPlayerReady = !(bool)isPlayerReady;
            SetPlayerReady((bool)isPlayerReady);
        }
        else
        {
            isPlayerReady = true;
            SetPlayerReady(true);
        }
        var props = new Hashtable() { { "IsPlayerReady", isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void OnBackButtonClicked()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "IsPlayerReady", false } });
        PhotonNetwork.LeaveRoom();
    }
}
