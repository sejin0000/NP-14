using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static MainGameManager;

public class TestPanel : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    public Button EnterTestRoomButton;
    public Button CreateTestRoomButton;    
    public Button BackButton;

    [Header("CurrentRoomBoard")]
    [SerializeField] private GameObject ScrollViewContent;

    [Header("CreateRoomBoard")]
    [SerializeField] private TMP_InputField RoomNameSetup;
    [SerializeField] private TMP_InputField RoomMemberSetup;

    [Header("TestRoomInfo")]
    private string selectedRoomName;
    public string SelectedRoomName
    {
        get { return selectedRoomName; }
        set
        {
            if (selectedRoomName != value)
            {
                selectedRoomName = value;
                OnRoomNameChanged(value);
            }
        }
    }
    private Dictionary<string, RoomInfo> cachedTestRoomList;
    private Dictionary<string, GameObject> testRoomEntryList;

    public event Action OnEntryClicked;

    [Header("RoomPanel")]
    public GameObject RoomPanel;

    private void Start()
    {
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        EnterTestRoomButton.onClick.AddListener(OnEnterTestRoomButtonClicked);
        CreateTestRoomButton.onClick.AddListener(OnCreateTestRoomButtonClicked);            
        BackButton.onClick.AddListener(OnBackButtonClicked);
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearTestRoomListView();
        Debug.Log("룸 추가여");
        UpdateCachedTestRoomList(roomList);
        UpdateTestRoomListView();
    }

    private void UpdateCachedTestRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            info.CustomProperties.TryGetValue("IsTest", out object testBool);
            if ((!info.IsOpen || !info.IsVisible || info.RemovedFromList ) & !(bool)testBool )
            {
                if (cachedTestRoomList.ContainsKey(info.Name))
                {
                    cachedTestRoomList.Remove(info.Name);
                }
                continue;
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
        testRoomEntryList.Clear();
        foreach (RoomInfo info in cachedTestRoomList.Values)
        {
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/TestRoomEntry"));            
            entry.transform.SetParent(ScrollViewContent.transform, false);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
            OnEntryClicked += entry.GetComponent<RoomListEntry>().OnSelectRoomButtonClicked;
            testRoomEntryList[info.Name] = entry;
        }
    }

    private void ClearTestRoomListView()
    {
        foreach (GameObject entry in testRoomEntryList.Values)
        {
            Destroy(entry.gameObject);
        }

        testRoomEntryList.Clear();
    }

    #region Button
    public string OnRoomNameChanged(string roomName)
    {
        return roomName;
    }
    private void OnEnterTestRoomButtonClicked()
    {
        // 발견
        foreach (GameObject entry in testRoomEntryList.Values)
        {
            var entryInfo = entry.GetComponent<RoomListEntry>();
            if (entryInfo.isEntryClicked)
            {
                selectedRoomName = entryInfo.roomName;
            }
        }
        PhotonNetwork.JoinRoom(selectedRoomName);
    }

    private void OnCreateTestRoomButtonClicked()
    {
        string roomName = RoomNameSetup.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + UnityEngine.Random.Range(1000, 10000) : roomName;

        byte maxPlayers;
        byte.TryParse(RoomMemberSetup.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsTest", true } };

        PhotonNetwork.CreateRoom(roomName, options, null);
        this.gameObject.SetActive(false);
        RoomPanel.SetActive(true);
    }

    private void OnBackButtonClicked()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
