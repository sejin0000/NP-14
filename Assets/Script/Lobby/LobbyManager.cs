using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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

    [Header("MainRoomPanel")]
    public RoomPanel RoomP;

    [Header("CharaceterSelectPopup")]
    public CharacterSelectPopup CharacterSelect;
    public GameObject _CharacterSelectPopup;
    public bool IsCharacterSelectInitialized;

    [Header("TestLobbyPanel")]
    public TestLobbyPanel TestLobbyP;

    [Header("TestRoomPanel")]
    public TestRoomPanel TestRoomP;

    [Header("ClientPlayer")]
    public GameObject instantiatedPlayer;
    public int ViewID;
    public int ClassNum;
    public GameObject DataSettingPrefab;
    public PlayerDataSetting dataSetting;
    

    // RoomEntries
    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;
    public Dictionary<string, RoomInfo> cachedRoomList;
    public Dictionary<int, GameObject> playerPartyDict;

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

        // DESC : DataSetting 초기화
        dataSetting = DataSettingPrefab.GetComponent<PlayerDataSetting>();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"LobbyManager - Current State : {Enum.GetName(typeof(PanelType), CurrentState)}");

        if (cachedRoomList != null) 
        {
            cachedRoomList.Clear();
            Debug.Log($"cahcedRoomList Cleared - Current State : {Enum.GetName(typeof(PanelType), CurrentState)}");
        }
        if (cachedTestRoomList != null)
        {
            cachedTestRoomList.Clear();
            Debug.Log($"cachedTestRoomList Cleared - Current State : {Enum.GetName(typeof(PanelType), CurrentState)}");
        }
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        cachedTestRoomList.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            info.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testBool);

            // DESC : 기존 룸인포 지우기
            if (testBool == null)
            {
                info.CustomProperties.Add(CustomProperyDefined.TEST_OR_NOT, false);
                cachedRoomList.Remove(info.Name);
            }
            else
            {
                cachedTestRoomList.Remove(info.Name);
            }

            if (cachedRoomList.ContainsKey(info.Name) && !(bool)testBool)
            {
                cachedRoomList[info.Name] = info;
            }
            if (cachedTestRoomList.ContainsKey(info.Name) && (bool)testBool)
            {
                cachedTestRoomList[info.Name] = info;
            }

            // DESC : 방이 존재하지 않을 조건일 시, 지우기.
            if (!info.IsOpen || info.RemovedFromList || !info.IsVisible || info.PlayerCount == 0)
            {
                if ((bool)testBool || cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }
                if (!(bool)testBool || cachedTestRoomList.ContainsKey(info.Name))
                {
                    cachedTestRoomList.Remove(info.Name);
                }
                continue;
            }
        }

        UpdateTestRoomListView();
    }

    public override void OnJoinedRoom()
    {
        // DESC : 빠른 시작과 테스트룸 구분
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testProperty);
        if (!(bool)testProperty)
        {
            SetRoomPanel();
        }
        else
        {
            SetTestRoomPanel();
        }
    }

    
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // DESC : 룸 생성
        Debug.Log($"OnJoinRoomFailed");
    }

    // DESC : 랜덤룸 진입 (OnGameButton) 에 실패했을 경우, 방을 만듦.
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = $"RandRoom{Random.Range(1, 200)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 3 };
        options.CustomRoomProperties = new Hashtable() { { CustomProperyDefined.TEST_OR_NOT, false } };
        PhotonNetwork.CreateRoom(roomName, options, null);
        Debug.Log($"LobbyManager - Room Created {roomName}");
    }

    private void UpdateTestRoomListView()
    {
        testRoomListEntries.Clear();

        foreach (RoomInfo info in cachedTestRoomList.Values)
        {
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/TestRoomEntry"), TestLobbyP.RoomScrollViewContent.transform, false);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
            TestLobbyP.OnEntryClicked += entry.GetComponent<RoomListEntry>().OnSelectRoomButtonClicked;
            testRoomListEntries[info.Name] = entry;
        }
    }

    public void SetRoomPanel()
    {
        SetPanel(PanelType.RoomPanel);

        instantiatedPlayer = RoomP.InstantiatePlayer();
        ViewID = instantiatedPlayer.GetPhotonView().ViewID;
        instantiatedPlayer.GetComponent<ClassIdentifier>().playerData = dataSetting;

        dataSetting.ownerPlayer = instantiatedPlayer;
        dataSetting.viewID = ViewID;

        RoomP.SetPartyPlayerInfo();
    }

    public void SetTestRoomPanel()
    {
        SetPanel(PanelType.TestRoomPanel);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
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

    #endregion

    #region Utility
    public void SetPanel(PanelType panelType)
    {
        CurrentState = panelType;
        string panelName = Enum.GetName(typeof(PanelType), panelType);
        LoginP.gameObject.SetActive(panelName.Equals(LoginP.name));
        MainLobbyP.gameObject.SetActive(panelName.Equals(MainLobbyP.name));
        RoomP.gameObject.SetActive(panelName.Equals(RoomP.name));
        TestLobbyP.gameObject.SetActive(panelName.Equals(TestLobbyP.name));
        TestRoomP.gameObject.SetActive(panelName.Equals(TestRoomP.name));
    }
    #endregion
}
