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

public class MainLobbyPanel : MonoBehaviourPunCallbacks
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

    public TextMeshProUGUI Gold; // TODO : �ڳ����̽��� ��差 �߰� ����

    private void Awake()
    {
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

        // DESC : Ŀ���� ������Ƽ - Char_Class �߰�
        LobbyManager.Instance.ClassNum = playerInfo.GetCharClass();
    }

    private void InstantiatePlayer()
    {
        LobbyManager.Instance.instantiatedPlayer = Instantiate(Resources.Load<GameObject>(PrefabPathes.PLAYER_INLOBBY_PREFAB_PATH));
    }

    #region MonoBehaviorPunCallbacks
    public override void OnJoinedLobby()
    {
        Debug.Log($"MainLobbyPanel - Current State : {Enum.GetName(typeof(PanelType), LobbyManager.Instance.CurrentState)}");
        if (LobbyManager.Instance.CurrentState != PanelType.MainLobbyPanel)
        {
            return;
        }
    }

    // DESC : ������ ���� (OnGameButton) �� �������� ���, ���� ����.
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (LobbyManager.Instance.CurrentState != PanelType.MainLobbyPanel)
        {
            return;
        }

        // DESC : �� ����
        string roomName = $"RandRoom{Random.Range(1, 200)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 3 };     
        options.CustomRoomProperties = new Hashtable() { { CustomProperyDefined.TEST_OR_NOT, false } };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        if (LobbyManager.Instance.CurrentState != PanelType.MainLobbyPanel)
        {
            return;
        }

        LobbyManager.Instance.SetPanel(PanelType.RoomPanel);
    }

    public override void OnCreatedRoom()
    {
        if (LobbyManager.Instance.CurrentState != PanelType.MainLobbyPanel)
        {
            return;
        }

        LobbyManager.Instance.SetPanel(PanelType.RoomPanel);
    }
    #endregion

    #region Buttons
    private void OnGameRoomButtonClicked()
    {
        var CustomRoomProperties = new Hashtable() { { CustomProperyDefined.TEST_OR_NOT, false } };
        PhotonNetwork.JoinRandomRoom(CustomRoomProperties, 0);        
    }

    public void OnTestLobbyButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.TestLobbyPanel);
    }
    #endregion
}
