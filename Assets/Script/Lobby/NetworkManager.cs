using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    // RoomEntries
    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;
    public Dictionary<string, RoomInfo> cachedRoomList;

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

        // DESC : Instantiate Dictionaries
        cachedRoomList = new Dictionary<string, RoomInfo>();
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        testRoomListEntries = new Dictionary<string, GameObject>();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log($"LobbyManager - Current State : {Enum.GetName(typeof(PanelType), LobbyManager.Instance.CurrentState)}");

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
            if (testBool != null
                && (!info.IsOpen || info.RemovedFromList || !info.IsVisible || info.PlayerCount == 0))
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

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
        cachedTestRoomList.Clear();
    }

    public override void OnJoinedRoom()
    {
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
            Debug.Log($"LobbyManager - OnJoinedRoom Current RoomName : {PhotonNetwork.CurrentRoom.Name}");
            Debug.Log($"LobbyManager - OnJoinedRoom Current ClientState : {Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
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

    public override void OnLeftRoom()
    {
        Debug.Log("LEAVING...");
        if (PhotonNetwork.NetworkClientState == ClientState.Leaving)
        {
            StartCoroutine(WaitForLeaving());
        }

        if (LobbyManager.Instance.CurrentState == PanelType.RoomPanel)
        {
            Debug.Log($"LobbyManager - LeftRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"LobbyManager - LeftRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            LobbyManager.Instance.playerPartyDict.Clear();
        }

        if (LobbyManager.Instance.CurrentState == PanelType.TestRoomPanel)
        {
            Debug.Log($"LobbyManager - LeftTestRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
            LobbyManager.Instance.SetPanel(PanelType.TestLobbyPanel);
            PhotonNetwork.ConnectUsingSettings();
            Debug.Log($"LobbyManager - LeftTestRoom : Client State : {PhotonNetwork.NetworkClientState}{Enum.GetName(typeof(ClientState), PhotonNetwork.NetworkClientState)}");
        }

    }

    private void UpdateTestRoomListView()
    {
        testRoomListEntries.Clear();
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
