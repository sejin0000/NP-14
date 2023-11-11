using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
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

    public override void Initialize()
    {
        // 특정 state에서 실행되는 이벤트를 추가하고 연결을 함..
        MainGameManager.Instance.OnGameClearedEvent += Open;
        MainGameManager.Instance.OnGameClearedEvent += OnGameCleared;
        MainGameManager.Instance.OnGameOverEvent += Open;
        MainGameManager.Instance.OnGameOverEvent += OnGameOver;
    }
    
    public void OnGameCleared()
    {
        ResultText.text = "Game Cleared!";
        isCleared = true;
        string endPartyInfoPath = "Prefabs/MainGameScene/EndPlayerInfo";
        string imagePath = "Prefabs/Images/CharClass";
        var playerPrefab = Resources.Load<GameObject>(endPartyInfoPath);
        var playerArray = MainGameManager.Instance.PartyViewIDArray.ToArray();


        for (int i = 0; i < ResultPartyBox.transform.childCount; i++) 
        {
            var childObject = ResultPartyBox.transform.GetChild(i).gameObject;
            var playerUI = Instantiate(playerPrefab, childObject.transform, false);
            var playerInfo = playerUI.GetComponent<EndPlayerInfo>();
            var playerPV = PhotonView.Find(playerArray[i]);
            var playerNickName = playerPV.Owner.NickName;
            playerPV.Owner.CustomProperties.TryGetValue("Char_Class", out object playerClass);
            var playerImage = Resources.Load<Image>(imagePath + (string)playerClass);
            playerInfo.PlayerImage = playerImage;
            playerInfo.PlayerNickName.text = playerNickName;
        }
        ToLobbyButton.onClick.AddListener(LoadRoomInLobbyScene);
    }

    public void OnGameOver()
    {
        ResultText.text = "Game Over,,,";
        isCleared = false;
        string endPartyInfoPath = "Prefabs/MainGameScene/EndPlayerInfo";
        string imagePath = "Prefabs/Images/CharClass";
        var playerPrefab = Resources.Load<GameObject>(endPartyInfoPath);
        var playerArray = MainGameManager.Instance.PartyViewIDArray.ToArray();


        for (int i = 0; i < ResultPartyBox.transform.childCount; i++)
        {
            var childObject = ResultPartyBox.transform.GetChild(i).gameObject;
            var playerUI = Instantiate(playerPrefab, childObject.transform, false);
            var playerInfo = playerUI.GetComponent<EndPlayerInfo>();
            var playerPV = PhotonView.Find(playerArray[i]);
            var playerNickName = playerPV.Owner.NickName;
            playerPV.Owner.CustomProperties.TryGetValue("Char_Class", out object playerClass);
            var playerImage = Resources.Load<Image>(imagePath + (string)playerClass);
            playerInfo.PlayerImage = playerImage;
            playerInfo.PlayerNickName.text = playerNickName;
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
