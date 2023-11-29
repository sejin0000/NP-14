using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MainLobbyPanel : MonoBehaviourPun
{
    [Header("Button")]
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button testLobbyButton;
    [SerializeField] private Button gameRoomButton;

    [Header("CharacterSelectPopup")]
    private CharacterSelectPopup playerInfo;
    private GameObject characterSelectPopup;

    [Header("Player")]
    private GameObject Player;
    private string playerPrefabPath;

    public TextMeshProUGUI Gold; // TODO : 뒤끝베이스에 골드량 추가 예정

    private void Start()
    {
        // DESC
        PhotonNetwork.AutomaticallySyncScene = true;

        // DESC : check CharacterSelectPopup
        if (LobbyManager.Instance.IsCharacterSelectInitialized)
        {
            characterSelectPopup = LobbyManager.Instance._CharacterSelectPopup;
            playerInfo = LobbyManager.Instance.CharacterSelect;
        }
        else
        {
            Debug.Log("LobbyManager didn't Initialize characterSelectPopup");
        }

        // DESC : connect buttons        
        characterSelectButton.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
        testLobbyButton.onClick.AddListener(OnTestLobbyButtonClicked);
        gameRoomButton.onClick.AddListener(OnGameRoomButtonClicked);

        // DESC : instantiate Player
        //InstantiatePlayer();

        // DESC : 커스텀 프로퍼티 - Char_Class 추가
        LobbyManager.Instance.ClassNum = playerInfo.GetCharClass();
    }

    private void InstantiatePlayer()
    {
        LobbyManager.Instance.instantiatedPlayer = Instantiate(Resources.Load<GameObject>(PrefabPathes.PLAYER_INLOBBY_PREFAB_PATH));
    }
  

    #region Buttons
    private void OnGameRoomButtonClicked()
    {
        var CustomRoomProperties = new Hashtable() { { CustomProperyDefined.TEST_OR_NOT, false } };

        if (NetworkManager.Instance.cachedRoomList == null
            || NetworkManager.Instance.cachedRoomList.Count == 0)
        {
            Debug.Log("MainLobbyPanel : cachedRoomList is Null");
            string roomName = $"Room {Random.Range(0, 200)}";

            RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 1500 };            
            options.CustomRoomProperties = CustomRoomProperties;
            options.CustomRoomPropertiesForLobby = new string[] { CustomProperyDefined.TEST_OR_NOT };

            PhotonNetwork.CreateRoom(roomName, options);            
        }
        else
        {
            Debug.Log("MainLobbyPanel : cachedRoomList is Not Null");            
            PhotonNetwork.JoinRandomRoom(CustomRoomProperties, 3);            
        }
    }

    public void OnTestLobbyButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.TestLobbyPanel);
    }
    #endregion
}
