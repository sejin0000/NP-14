using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;
using System.Linq;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    // RoomEntries
    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;
    public Dictionary<string, RoomInfo> cachedRoomList;
    public Dictionary<string, GameObject> RoomFindEntriesList;

    private AudioLibrary audioLibrary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Debug.Log($"NetworkManager - Current State Awake : {Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
        if (PhotonNetwork.NetworkClientState == ClientState.PeerCreated)
        {
            LobbyManager.Instance.SetPanel(PanelType.LoginPanel);
        }
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            PhotonNetwork.LeaveRoom();
            LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
        }
        // DESC : Instantiate Dictionaries
        cachedRoomList = new Dictionary<string, RoomInfo>();
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        testRoomListEntries = new Dictionary<string, GameObject>();
        RoomFindEntriesList = new Dictionary<string, GameObject>();

        audioLibrary = AudioManager.Instance.AudioLibrary;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        // LoadingUI 생성후 파괴?
        Debug.Log($"LobbyManager - Current State : {Enum.GetName(typeof(PanelType), LobbyManager.Instance.CurrentState)}");
        PhotonNetwork.AutomaticallySyncScene = true;

        if (cachedRoomList != null)
        {
            cachedRoomList.Clear();
        }
        if (cachedTestRoomList != null)
        {
            cachedTestRoomList.Clear();

        }

        if (LobbyManager.Instance.CurrentState == PanelType.LoginPanel)
        {
            LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
        }

        audioLibrary.CallLobbySoundEvent();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);

        ClearTestRoomListView();

        UpdateCachedTestRoomList(roomList);
        UpdateTestRoomListView();
        LobbyManager.Instance.RoomFindP.UpdateFindRoomListView();
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        cachedTestRoomList.Clear();
    }

    public override void OnJoinedRoom()
    {
        LobbyManager.Instance.LoadP.Initialize(1.5f);

        // DESC : 빠른 시작과 테스트룸 구분
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testProperty);
        if (!(bool)testProperty)
        {
            LobbyManager.Instance.SetRoomPanel();
            Debug.Log($"LobbyManager - OnJoinedRoom Current RoomName : {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"LobbyManager - OnJoinedRoom Current ClientState : {Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");

        }
        else
        {
            LobbyManager.Instance.SetTestRoomPanel();
            Debug.Log($"LobbyManager - OnJoinedTestRoom Current RoomName : {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"LobbyManager - OnJoinedTestRoom Current ClientState : {Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
        }
    }


    public override void OnJoinRoomFailed(short returnCode, string message)
    {
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testProperty);
        LobbyManager.Instance.SetRoomPanelEntered();
        if (PhotonNetwork.IsMasterClient)
        {
            var FirstPartyMember = LobbyManager.Instance.RoomP.FirstPartyMember;
            var FirstPartyState = FirstPartyMember.GetComponent<PartyMemberButton>();
            FirstPartyMember.gameObject.GetPhotonView().RPC("GetCurrentMemberButtonState", RpcTarget.Others, FirstPartyState.IsClicked);
            var SecondPartyMember = LobbyManager.Instance.RoomP.SecondPartyMember;
            var SecondPartyState = SecondPartyMember.GetComponent<PartyMemberButton>();
            SecondPartyMember.gameObject.GetPhotonView().RPC("GetCurrentMemberButtonState", RpcTarget.Others, SecondPartyState.IsClicked);
            var ThirdPartyMember = LobbyManager.Instance.RoomP.ThirdPartyMember;
            var ThirdPartyState = ThirdPartyMember.GetComponent<PartyMemberButton>();
            ThirdPartyMember.gameObject.GetPhotonView().RPC("GetCurrentMemberButtonState", RpcTarget.Others, ThirdPartyState.IsClicked);
        }
        if (!(bool)testProperty) 
        {
            LobbyManager.Instance.RoomP.gameObject.GetPhotonView().RPC("RemotePartyPlayerInfo", RpcTarget.All);
        }        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testProperty);
        if (!(bool)testProperty)
        {            
            LobbyManager.Instance.SetRoomPanelLeft(otherPlayer);
        }
    }
    public override void OnLeftRoom()
    {
        Debug.Log("LEAVING...");
        if (PhotonNetwork.NetworkClientState == ClientState.Leaving)
        {
            StartCoroutine(WaitForLeaving());
        }

        LobbyManager.Instance.CharacterSelect.gameObject.SetActive(false);

        if (LobbyManager.Instance.CurrentState == PanelType.RoomPanel)
        {
            Debug.Log($"LobbyManager - LeftRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            PhotonNetwork.LocalPlayer.CustomProperties[CustomProperyDefined.ASK_READY_PROPERTY] = false;
            LobbyManager.Instance.RoomP.ResetPartyBox();
            LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
            PhotonNetwork.ConnectUsingSettings();

            // DESC : 로딩패널 ON
            LobbyManager.Instance.LoadP.Initialize(1.5f);
            
            Debug.Log($"LobbyManager - LeftRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            LobbyManager.Instance.playerPartyDict.Clear();
        }

        if (LobbyManager.Instance.CurrentState == PanelType.TestRoomPanel)
        {
            Debug.Log($"LobbyManager - LeftTestRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            LobbyManager.Instance.SetPanel(PanelType.TestLobbyPanel);
            PhotonNetwork.ConnectUsingSettings();
            
            // DESC : 로딩패널 ON
            LobbyManager.Instance.LoadP.Initialize(1.5f);
            
            Debug.Log($"LobbyManager - LeftTestRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer == newMasterClient)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() {{ CustomProperyDefined.ASK_READY_PROPERTY, false }});
            LobbyManager.Instance.RoomP.ReadyButton.gameObject.SetActive(false);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        foreach (var hashKey in changedProps.Keys) 
        {
            Debug.Log($"OnPlayerPropertiesUpdate : {(string)hashKey}");        
        }
        var playerPartyDict = LobbyManager.Instance.playerPartyDict;
        var _RoomP = LobbyManager.Instance.RoomP;


        // DESC : 플레이어 파티 박스 최신화
        if (LobbyManager.Instance.CurrentState == PanelType.RoomPanel
            && PhotonNetwork.LocalPlayer.CustomProperties[CustomProperyDefined.ASK_READY_PROPERTY] != null)
        {
            _RoomP.SetPartyPlayerInfo();

            // DESC : 레디 상황 최신화 
            if (PhotonNetwork.IsMasterClient)
            {
                _RoomP.StartButton.gameObject.SetActive(_RoomP.CheckPlayersReady());
            }
        }
    }


    public void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            info.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testBool);
            if (testBool == null)
            {
                cachedRoomList.Remove(info.Name);
            }
            if (!info.IsOpen || info.RemovedFromList || !info.IsVisible || info.PlayerCount == 0 || (bool)testBool)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedRoomList.ContainsKey(info.Name) && !(bool)testBool)
            {
                cachedRoomList[info.Name] = info;
            }
            else if (!(bool)testBool)
            {
                cachedRoomList.Add(info.Name, info);
            }
        }
    }

    public void UpdateCachedTestRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            info.CustomProperties.TryGetValue(CustomProperyDefined.TEST_OR_NOT, out object testBool);
            if (testBool == null)
            {
                cachedTestRoomList.Remove(info.Name);
            }
            if (!info.IsOpen || info.RemovedFromList || !info.IsVisible || info.PlayerCount == 0 || !(bool)testBool)
            {
                if (cachedTestRoomList.ContainsKey(info.Name))
                {
                    cachedTestRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedTestRoomList.ContainsKey(info.Name) && (bool)testBool)
            {
                cachedTestRoomList[info.Name] = info;
            }
            else if ((bool)testBool)
            {
                cachedTestRoomList.Add(info.Name, info);
            }
        }
    }

    private void ClearTestRoomListView()
    {
        foreach (GameObject entry in testRoomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        testRoomListEntries.Clear();
        RoomFindEntriesList.Clear();
    }

    private void UpdateTestRoomListView()
    {
        var testLobby = LobbyManager.Instance.TestLobbyP;
        foreach (RoomInfo info in cachedTestRoomList.Values)
        {
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/TestRoomEntry"), testLobby.RoomScrollViewContent.transform, false);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
            testLobby.OnEntryClicked += entry.GetComponent<RoomListEntry>().OnSelectRoomButtonClicked;
            testRoomListEntries[info.Name] = entry;
        }
    }


    public IEnumerator WaitForLeaving()
    {
        while (PhotonNetwork.NetworkClientState == ClientState.Leaving)
            yield return null;
        LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
    }

    #region Button
    public void OnBackButtonClickedInRoomPanel()
    {
        LobbyManager.Instance.RoomP.LeaveRoomSetting();
        PhotonNetwork.LeaveRoom();
    }

    public void OnBackButtonClickedInTestRoomPanel()
    {
        LobbyManager.Instance.TestRoomP.LeaveRoomSetting();
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}
