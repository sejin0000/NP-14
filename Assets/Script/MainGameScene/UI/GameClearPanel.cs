using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameClearPanel : UIBase
{
    [Header("Result")]
    [SerializeField] private TextMeshProUGUI ResultText;
    private bool isCleared;

    [Header("Button")]
    public Button ToLobbyButton;

    [Header("PartyBox")]
    public GameObject ResultPartyBox;

    [Header("Color")]
    [SerializeField] public Color[] ClassColor;

    public override void Initialize()
    {
        // Ư�� state���� ����Ǵ� �̺�Ʈ�� �߰��ϰ� ������ ��..
        GameManager.Instance.OnGameClearEvent += OnGameCleared;
        GameManager.Instance.OnGameOverEvent += OnGameOver;
    }
    
    public void OnGameCleared()
    {
        Open();

        ResultText.text = "Game Clear";
        isCleared = true;
        string endPartyInfoPath = "Prefabs/MainGameScene/EndPlayerInfo";
        string imagePath = "Images/CharClass";
        var playerPrefab = Resources.Load<GameObject>(endPartyInfoPath);
        var playerArray = new List<int>(GameManager.Instance.playerInfoDictionary.Keys).ToArray();

        for (int i = 0; i < playerArray.Length; i++) 
        {
            var childObject = ResultPartyBox.transform.GetChild(i).gameObject;
            var playerUI = Instantiate(playerPrefab, childObject.transform, false);
            var playerInfo = playerUI.GetComponent<EndPlayerInfo>();
            var playerPV = PhotonView.Find(playerArray[i]);
            var playerNickName = playerPV.Owner.NickName;
            playerPV.Owner.CustomProperties.TryGetValue("Char_Class", out object playerClass);
            Debug.Log($"CustomProperty of {playerPV.ViewID} :  {(int)playerClass}");
            var playerImage = Resources.Load<Sprite>(imagePath + ((int)playerClass).ToString());
            playerInfo.PlayerImage.sprite = playerImage;
            playerInfo.PlayerNickName.text = playerNickName;

            //modify color;
            childObject.GetComponentInChildren<Image>().color = ClassColor[(int)playerClass];
        }
        ToLobbyButton.onClick.AddListener(LoadRoomInLobbyScene);
    }

    public void OnGameOver()
    {
        Open();

        ResultText.text = "Game Over";
        isCleared = false;
        string endPartyInfoPath = "Prefabs/MainGameScene/EndPlayerInfo";
        string imagePath = "Images/CharClass";
        var playerPrefab = Resources.Load<GameObject>(endPartyInfoPath);
        var playerArray = new List<int>(GameManager.Instance.playerInfoDictionary.Keys).ToArray();

        for (int i = 0; i < playerArray.Length; i++)
        {
            var childObject = ResultPartyBox.transform.GetChild(i).gameObject;
            var playerUI = Instantiate(playerPrefab, childObject.transform, false);
            var playerInfo = playerUI.GetComponent<EndPlayerInfo>();
            var playerPV = PhotonView.Find(playerArray[i]);
            var playerNickName = playerPV.Owner.NickName;
            playerPV.Owner.CustomProperties.TryGetValue("Char_Class", out object playerClass);
            Debug.Log($"CustomProperty of {playerPV.ViewID} : {(int)playerClass}");
            var playerImage = Resources.Load<Sprite>(imagePath + ((int)playerClass).ToString());
            playerInfo.PlayerImage.sprite = playerImage;
            playerInfo.PlayerNickName.text = playerNickName;

            //modify color;
            childObject.GetComponentInChildren<Image>().color = ClassColor[(int)playerClass];
        }
        ToLobbyButton.onClick.AddListener(LoadRoomInLobbyScene);
    }
    private void LoadRoomInLobbyScene()
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;

        PhotonNetwork.LoadLevel("LobbyScene");
    }
}
