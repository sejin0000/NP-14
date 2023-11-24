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
    [SerializeField] private Button ReadyButton;
    [SerializeField] private Button StartButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button characterSelectButton;

    [Header("Chat")]
    public TMP_InputField ChatInputField;
    public Button SubmitButton;
    public GameObject ChatLog;
    public GameObject ChatScrollContent;

    [HideInInspector]
    private string askReadyProp;
    
    public void Start()
    {
        askReadyProp = CustomProperyDefined.ASK_READY_PROPERTY;

        if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady))
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { askReadyProp, false } });
        }

        StartButton.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.gameObject.SetActive(false);
        }

        // DESC : 버튼 연결
        ReadyButton.onClick.AddListener(OnReadyButtonClicked);
        StartButton.onClick.AddListener(OnStartButtonClicked);
        BackButton.onClick.AddListener(OnBackButtonClicked);
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
        characterSelectButton.onClick.AddListener(LobbyManager.Instance.CharacterSelect.OnCharacterButtonClicked);
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
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady))
        {
            isPlayerReady = !(bool)isPlayerReady;
            SetPlayerReady((bool)isPlayerReady);
        }
        else
        {
            isPlayerReady = true;
            SetPlayerReady(true);
        }
        var props = new Hashtable() { { askReadyProp, isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainGameScene");
    }

    public void OnBackButtonClicked()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { askReadyProp, false } });
        PhotonNetwork.LeaveRoom();
    }

    public void OnSubmitButtonClicked()
    {
        string inputText = ChatInputField.text;
        string nickName = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("ChatInput", RpcTarget.All, inputText, nickName);
        ChatInputField.text = "";
        ChatInputField.ActivateInputField();
    }

    [PunRPC]
    public void ChatInput(string inputText, string nickName)
    {
        GameObject chatPrefab = Instantiate(ChatLog, ChatScrollContent.transform, false);
        ChatLog chatLog = chatPrefab.GetComponent<ChatLog>();

        chatLog.NickNameText.text = nickName;
        chatLog.ChatText.text = inputText;
        chatPrefab.transform.SetParent(ChatScrollContent.transform, false);
        chatLog.ConfirmTextSize(ChatInputField);
    }
}
