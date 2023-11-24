using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

public enum PanelType
{
    LoginPanel,
    MainLobbyPanel,
    RoomPanel,
    TestLobbyPanel,
    TestRoomPanel,
    ShopPanel,
}
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    [Header("State")]
    public PanelType CurrentState;

    [Header("LoginPanel")]
    public LoginPanel LoginP;

    [Header("MainLobbyPanel")]
    public MainLobbyPanel MainLobbyP;

    //[Header("MainRoomPanel")]
    //public MainRoomPanel MainRoomP;

    [Header("CharaceterSelectPopup")]
    public CharacterSelectPopup CharacterSelect;
    public GameObject _CharacterSelectPopup;
    public bool IsCharacterSelectInitialized;

    [Header("TestLobbyPanel")]
    public TestLobbyPanel TestLobbyP;

    //[Header("TestRoomPanel")]
    //public TestRoomPanel TestRoomP;

    [Header("ClientPlayer")]
    public GameObject instantiatedPlayer;
    public int ViewID;
    public int ClassNum;

    // RoomEntries
    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;
    private Dictionary<string, RoomInfo> cachedRoomList;

    public void Awake()
    {
        // DESC : singleton
        if (Instance == null)
        {
            Instance = this;
        }

        // DESC : Current State
        CurrentState = PanelType.LoginPanel;

        // DESC : Instantiate Dictionaries
        cachedRoomList = new Dictionary<string, RoomInfo>();        
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        testRoomListEntries = new Dictionary<string, GameObject>();

        // DESC : CharacterSelectPopup 초기화
        CheckCharacterSelectPopup();
        if (!IsCharacterSelectInitialized)
        {
            InstantiateCharacterSelectPopup();
        }
    }

    public override void OnJoinedLobby()
    {
        if (cachedRoomList != null) 
        {
            cachedRoomList.Clear();
        }
        if (cachedTestRoomList != null)
        {
            cachedTestRoomList.Clear();
        }
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        cachedTestRoomList.Clear();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(PhotonNetwork.NetworkingClient.LoadBalancingPeer.DebugOut);
        SetPanel(PanelType.MainLobbyPanel);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetPanel(PanelType.MainLobbyPanel);
        PhotonNetwork.LeaveRoom();
    }


    #region CharacterSelectPopup
    public void CheckCharacterSelectPopup()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<CharacterSelectPopup>())
            {
                IsCharacterSelectInitialized = true;
                var caseTransform = this.transform.parent.GetChild(i);
                _CharacterSelectPopup = caseTransform.gameObject;
                CharacterSelect = caseTransform.GetComponent<CharacterSelectPopup>();
                break;
            }
        }
    }
    public void InstantiateCharacterSelectPopup()
    {
        Debug.Log("LobbyManager - CharacterSelectPopup 인스턴스화");
        _CharacterSelectPopup = Instantiate(Resources.Load<GameObject>(PrefabPathes.CHARACTER_SELECT_POPUP), this.transform);
        _CharacterSelectPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        CharacterSelect = _CharacterSelectPopup.GetComponent<CharacterSelectPopup>();
        _CharacterSelectPopup.SetActive(false);
        IsCharacterSelectInitialized = true;
    }
    #endregion

    #region Button
    public void OnTestLobbyButtonClicked()
    {
        SetPanel(PanelType.TestLobbyPanel);
    }

    public void OnGameRoomButtonClicked()
    {
        SetPanel(PanelType.RoomPanel);
    }
    #endregion

    #region Utility
    public void SetPanel(PanelType panelType)
    {
        CurrentState = panelType;
        string panelName = Enum.GetName(typeof(PanelType), PanelType.RoomPanel);
        LoginP.gameObject.SetActive(panelName.Equals(LoginP.name));
        MainLobbyP.gameObject.SetActive(panelName.Equals(MainLobbyP.name));
        //RoomP.SetActive(panelName.Equals(RoomP.name));
        TestLobbyP.gameObject.SetActive(panelName.Equals(TestLobbyP.name));
        //TestRoomP.SetActive(panelName.Equals(TestRoomP.name));
    }
    #endregion
}
