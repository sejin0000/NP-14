using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : MonoBehaviourPun
{
    [Header("button")]
    public Button ReadyButton;
    public Button StartButton;

    private bool isPlayerReady;

    public void Start()
    {
        StartButton.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.gameObject.SetActive(false);
        }

        // ���� ��ư ����
        ReadyButton.onClick.AddListener(OnReadyButtonClicked);
    }

    public void SetPlayerReady(bool playerReady)
    {
        if (!playerReady)
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(130 / 255f, 241 / 255f, 96 / 255f);
        }
        else
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "�غ�";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(255 / 255f, 182 / 255f, 182 / 255f);
        }
    }

    public void OnReadyButtonClicked()
    {
        isPlayerReady = !isPlayerReady;
        SetPlayerReady(isPlayerReady);
        var props = new ExitGames.Client.Photon.Hashtable() { { "IsPlayerReady", isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
}
