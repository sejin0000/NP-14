using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

public class LobbyPanel : MonoBehaviourPunCallbacks
{
    public enum LobbyType
    {
        Panel,
        Popup,
    }
    public enum PanelList
    {
        LoginPanel,
        MainLobbyPanel,
        RoomPanel,
        None,
    }

    public enum PopupList
    {
        CharacterSelectPopup,
        None,
    }

    public enum CharClass
    {
        Soldier,
        Shotgun,
        Sniper,
    }

    public enum RoomType
    {
        MainGameRoom,
        TestRoom,
    }
    [System.Serializable]
    public class LobbyUI
    {
        public GameObject UIObject;
        public LobbyType LobbyType;
        public PanelList panelList;
        public PopupList popupList;
    }

    [Header("Login")]
    public GameObject LoginPanel;

    public TMP_InputField PlayerIdInput;
    public TMP_InputField PlayerPswdInput;

    [Header("MainLobby")]
    public GameObject MainLobbyPanel;

    public Button CharacterSelectButtonInLobby;
    public TextMeshProUGUI Gold;

    [Header("RoomPanel")]
    public GameObject RoomPanel;

    public GameObject PartyBox;
    public GameObject PlayerInfo;

    public Button CharacterSelectButtonInRoom;
    public Button StartButton;
    public Button ReadyButton;

    public TMP_InputField ChatInput;
    public GameObject ChatLogPrefab;
    public GameObject ChatBoxScrollContent;
    
    [Header("CharacterSelectPopup")]
    public GameObject CharacterSelectPopup;
    public TextMeshProUGUI PlayerClassText;
    public GameObject StatInfo;
    public TextMeshProUGUI SkillInfoText;

    [Header("Shop")]
    public GameObject Shop;        

    [Header("TestLobbyPanel")]
    public GameObject TestLobbyPanel;
    public Button CharacterSelectButtonInTestPanel;
    public TestPanel testPanel;

    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;

    [Header("TestRoomPanel")]
    public GameObject TestRoomPanel;

    [Header("ETC")]
    public GameObject playerDataSetting;
    public GameObject playerContainer;
    private Dictionary<int, GameObject> playerInfoListEntries;
    private Dictionary<string, RoomInfo> cachedRoomList;

    [Header("ClientPlayer")]
    [SerializeField] private GameObject instantiatedPlayer;
    [SerializeField] private int viewID;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        playerInfoListEntries = new Dictionary<int, GameObject>();
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        testRoomListEntries = new Dictionary<string, GameObject>();


        ExitGames.Client.Photon.Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!playerCP.ContainsKey("Char_Class"))
        {
            PhotonNetwork.SetPlayerCustomProperties(
            new ExitGames.Client.Photon.Hashtable()
            { {"Char_Class", CharClass.Soldier} }
            );
        }

        playerDataSetting = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterData/PlayerCharacterSetting"));

        testPanel = TestLobbyPanel.GetComponent<TestPanel>();
        testPanel.Initialize();
    }

    public virtual void Start()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            SetPanel(LoginPanel.name);
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);

        ClearTestRoomListView();

        UpdateCachedTestRoomList(roomList);
        UpdateTestRoomListView();
    }

    public override void OnJoinedLobby()
    {
        if (cachedRoomList != null)
        {
            cachedRoomList.Clear();
        }
        SetPanel(MainLobbyPanel.name);

        if (playerContainer.transform.childCount == 0)
        {
            GameObject playerPrefab = Resources.Load<GameObject>("Pefabs/Player");
            GameObject go = Instantiate(playerPrefab);
            go.transform.SetParent(playerContainer.transform);
            
            Player localPlayer = PhotonNetwork.LocalPlayer;

            localPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum);

            if (CharacterSelectPopup == null)
            {
                CharacterSelectPopup = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/CharacterSelectPopup"));
                CharacterSelectPopup.transform.SetParent(this.transform);
                CharacterSelectPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                var playerInfo = CharacterSelectPopup.GetComponent<PlayerInfo>();
                PlayerClassText = playerInfo.playerClassText;
                SkillInfoText = playerInfo.playerSkillText;
                playerInfo.playerDataSetting = playerDataSetting.GetComponent<PlayerDataSetting>();

                CharacterSelectButtonInLobby.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
                CharacterSelectButtonInRoom.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
                CharacterSelectButtonInTestPanel.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
            }

            var PlayerData = playerDataSetting.GetComponent<PlayerDataSetting>();  
            PlayerData.playerContainer = playerContainer;
        }

        Shop.SetActive(true);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetPanel(MainLobbyPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetPanel(MainLobbyPanel.name);
    }

    // 랜덤룸 찾기 실패했을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = $"RandRoom{Random.Range(1,200)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 3 };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsTest", false } };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnCreatedRoom()
    {
            
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} 입장");
        Shop.SetActive(false);

        instantiatedPlayer = InstantiatePlayer();
        viewID = instantiatedPlayer.GetPhotonView().ViewID;
        instantiatedPlayer.GetComponent<ClassIdentifier>().playerData = playerDataSetting.GetComponent<PlayerDataSetting>();

        // 
        PlayerDataSetting playerData = playerDataSetting.GetComponent<PlayerDataSetting>();
        playerData.ownerPlayer = instantiatedPlayer;
        playerData.viewID = viewID;

        var playerInfo = CharacterSelectPopup.GetComponent<PlayerInfo>();
        playerInfo.Initialize();
        
        //
        object classNum;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out classNum);
        instantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);

        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsTest", out object isTest);
        Debug.Log($"isTest : {(bool)isTest}");
        if ((bool)isTest == false)
        {
            SetPanel(RoomPanel.name);
            
            if (cachedRoomList != null)
            {
                cachedRoomList.Clear();
            }
            
            if (playerInfoListEntries == null)
            {
                playerInfoListEntries = new Dictionary<int, GameObject>();
            }

            // PartyPlayerInfo에서 받은 프리팹 정보를 각각의 프리팹에 적용.
            SetPartyPlayerInfo();

            // 스타트 버튼 동기화
            StartButton.gameObject.SetActive(CheckPlayersReady());
        }
        else
        {
            SetPanel(TestRoomPanel.name);

            if (cachedTestRoomList != null)
            {
                cachedTestRoomList.Clear();
            }

            if (cachedTestRoomList == null)
            {
                testRoomListEntries = new Dictionary<string, GameObject>();
            }

            StartButton.gameObject.SetActive(true);
            //here
        }
        
    }

    public GameObject InstantiatePlayer()
    {
        // 네트워크 인스턴스
        // 추후 수정 : prefab 경로
        GameObject playerPrefab = playerContainer.transform.GetChild(0).gameObject;
        playerPrefab.name = "Pefabs/Player";

        PlayerDataSetting playerData = playerDataSetting.GetComponent<PlayerDataSetting>();
        GameObject playerNet = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum);
        playerData.SetClassType((int)classNum, playerNet);

        Destroy(playerPrefab);
        return playerNet;
    }

    public override void OnLeftRoom()
    {
        SetPanel(MainLobbyPanel.name);

        foreach (GameObject playerInfoEntry in playerInfoListEntries.Values) 
        {
            Destroy(playerInfoEntry.gameObject);
        }

        playerInfoListEntries.Clear();
        playerInfoListEntries = null;

        if (ChatBoxScrollContent.transform.childCount > 0)
        {
            for (int i = 0; i < ChatBoxScrollContent.transform.childCount; i++)
            {
                Destroy(ChatBoxScrollContent.transform.GetChild(i).gameObject);
            }
        }

        PlayerDataSetting playerData = playerDataSetting.GetComponent<PlayerDataSetting>();
        GameObject playerPrefab = Resources.Load<GameObject>("Pefabs/Player");
        GameObject go = Instantiate(playerPrefab);
        go.transform.SetParent(playerContainer.transform);
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            playerData.SetClassType((int)classNum, go);
        }
        else
        {
            playerData.SetClassType(0, go);
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Room currentRoom = PhotonNetwork.CurrentRoom;

        Debug.Log($"{newPlayer.NickName} 입장");

        //
        object classNum;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out classNum);
        instantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);

        //
        SetPartyPlayerInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerInfoListEntries[otherPlayer.ActorNumber].gameObject != null)
        {
            Destroy(playerInfoListEntries[otherPlayer.ActorNumber].gameObject);
        }
        playerInfoListEntries.Remove(otherPlayer.ActorNumber);

        Debug.Log($"{otherPlayer.NickName} 퇴장");

        SetPartyPlayerInfo();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            // TODO : 호스트가 바뀌면, 바뀐 사람에게만 StartButton이 활성화되어야함.
            // 이 readybutton 비활성화는 마스터클라이언트에서만 이루어지게...
            ReadyButton.gameObject.SetActive(false);
            StartButton.gameObject.SetActive(CheckPlayersReady());
        }
        else
        {
            if (ReadyButton.gameObject == null)
            {
                ReadyButton.gameObject.SetActive(true);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (playerInfoListEntries == null)
        {
            playerInfoListEntries = new Dictionary<int, GameObject>();
        }

        GameObject playerInfoEntry;
        if (playerInfoListEntries.TryGetValue(targetPlayer.ActorNumber, out playerInfoEntry))
        {
            SetPartyPlayerInfo();
        }

        StartButton.gameObject.SetActive(CheckPlayersReady());
    }


    #region Button

    public void OnLoginButtonClicked()
    {
        // 로그인 부분 체크 후, 변경
        // 일단은, 다른 패널 활성화로 대체
        if (PlayerIdInput.text == "")
        {
            PhotonNetwork.LocalPlayer.NickName = $"Player_{Random.Range(0, 2000)}";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerIdInput.text;
        }
        // 닉네임 설정 부분 새로 파야함.
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결
        }

    }

    public void OnJoinRandomRoomButtonClicked()
    {
        if (cachedRoomList == null)
        {
            string roomName = $"Room {Random.Range(0, 200)}";

            RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 10000 };
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsTest", false } };
            
            PhotonNetwork.CreateRoom(roomName, options, null);
        }
        else
        {
            var testProperty = new ExitGames.Client.Photon.Hashtable() { { "IsTest", false } };
            PhotonNetwork.JoinRandomRoom(testProperty, 0);
        }
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainGameScene");
    }

    public void OnTestRoomButtonClicked()
    {
        MainLobbyPanel.SetActive(false);
        TestLobbyPanel.SetActive(true);
    }
    #endregion


    #region UTILITY

    public void SetPanel(string panelName)
    {
        LoginPanel.SetActive(panelName.Equals(LoginPanel.name));
        MainLobbyPanel.SetActive(panelName.Equals(MainLobbyPanel.name));
        RoomPanel.SetActive(panelName.Equals(RoomPanel.name));
        TestLobbyPanel.SetActive(panelName.Equals(TestLobbyPanel.name));
        TestRoomPanel.SetActive(panelName.Equals(TestRoomPanel.name));
    }

    public void SetPopup(string popupName) 
    {
        CharacterSelectPopup.SetActive(popupName.Equals(CharacterSelectPopup.name));
    }

    public void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }

            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateCachedTestRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.CustomProperties.TryGetValue("IsTest", out object testBool))
            {
                if ((!info.IsOpen || !info.IsVisible || info.RemovedFromList) & !(bool)testBool)
                {
                    if (cachedTestRoomList.ContainsKey(info.Name))
                    {
                        cachedTestRoomList.Remove(info.Name);
                    }
                    continue;
                }
            }

            if (cachedTestRoomList.ContainsKey(info.Name))
            {
                cachedTestRoomList[info.Name] = info;
            }
            else
            {
                cachedTestRoomList.Add(info.Name, info);
            }
        }
    }

    private void UpdateTestRoomListView()
    {
        testRoomListEntries.Clear();
        
        foreach (RoomInfo info in cachedTestRoomList.Values)
        {
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/TestRoomEntry"));
            entry.transform.SetParent(testPanel.RoomScrollViewContent.transform, false);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
            testPanel.OnEntryClicked += entry.GetComponent<RoomListEntry>().OnSelectRoomButtonClicked;
            testRoomListEntries[info.Name] = entry;
        }
    }

    private void ClearTestRoomListView()
    {
        foreach (GameObject entry in testRoomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        testRoomListEntries.Clear();
    }

    public bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        if (PhotonNetwork.PlayerList.Count() < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            return false;
        }
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                if (!(bool) isPlayerReady && !p.IsMasterClient)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void SetPartyPlayerInfo()
    {
        playerInfoListEntries.Clear();

        for (int i = 0; i < PartyBox.transform.childCount; i++)
        {
            if (PartyBox.transform.GetChild(i).childCount > 0)
            {
                Destroy(PartyBox.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        int cnt = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerInfoPrefab = Instantiate(PlayerInfo);
            playerInfoPrefab.transform.SetParent(PartyBox.transform.GetChild(cnt), false);
            playerInfoPrefab.transform.localScale = Vector3.one;
            playerInfoPrefab.GetComponent<PartyPlayerInfo>().Initialize(cnt, p);

            playerInfoListEntries.Add(p.ActorNumber, playerInfoPrefab);

            cnt++;
        }
    }
    #endregion
}
