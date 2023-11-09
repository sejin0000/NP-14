using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    public Button EnterTestRoomButton;
    public Button CreateTestRoomButton;    
    public Button BackButton;

    [Header("CurrentRoomBoard")]
    [SerializeField] public GameObject ScrollViewContent;

    [Header("CreateRoomBoard")]
    [SerializeField] public TMP_InputField RoomNameSetup;
    [SerializeField] public TMP_InputField RoomMemberSetup;

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

    [Header("RoomPanel")]    
    public GameObject TestRoomPanel;


    public event Action OnEntryClicked;
    public GameObject canvas;
    private LobbyPanel lobbyPanel;

    public void Initialize()
    {        
        EnterTestRoomButton.onClick.AddListener(OnEnterTestRoomButtonClicked);
        CreateTestRoomButton.onClick.AddListener(OnCreateTestRoomButtonClicked);            
        BackButton.onClick.AddListener(OnBackButtonClicked);
        lobbyPanel = canvas.GetComponent<LobbyPanel>();
    }

    #region Button
    public string OnRoomNameChanged(string roomName)
    {
        return roomName;
    }
    private void OnEnterTestRoomButtonClicked()
    {
        // ¹ß°ß
        foreach (GameObject entry in lobbyPanel.testRoomListEntries.Values)
        {
            var entryInfo = entry.GetComponent<RoomListEntry>();
            if (entryInfo.isEntryClicked)
            {
                selectedRoomName = entryInfo.roomName;
            }
        }
        PhotonNetwork.JoinRoom(selectedRoomName);
        this.gameObject.SetActive(false);
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
        TestRoomPanel.SetActive(true);
        Debug.Log("TestRoomPanel·Î ,,");
    }

    private void OnBackButtonClicked()
    {
        this.gameObject.SetActive(false);
    }
    #endregion
}
