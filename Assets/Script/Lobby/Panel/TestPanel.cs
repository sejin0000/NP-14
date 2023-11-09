using Photon.Pun;
using Photon.Realtime;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public Button SceneSelectButton;
    public Button SceneSelectStartButton;


    [Header("CurrentRoomBoard")]
    [SerializeField] public GameObject RoomScrollViewContent;    

    [Header("CreateRoomBoard")]
    [SerializeField] public TMP_InputField RoomNameSetup;
    [SerializeField] public TMP_InputField RoomMemberSetup;
    [SerializeField] public GameObject SceneScrollViewContent;
    [SerializeField] public GameObject SceneScrollView;

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
    private string selectedSceneName;

    [Header("RoomPanel")]    
    public GameObject TestRoomPanel;


    public event Action OnEntryClicked;
    public GameObject canvas;
    private LobbyPanel lobbyPanel;
    public List<SceneConnectButton> sceneConnectButtons;

    [HideInInspector]
    private string folderPath;
    private string sceneEntryPath;

    public void Initialize()
    {        
        EnterTestRoomButton.onClick.AddListener(OnEnterTestRoomButtonClicked);
        CreateTestRoomButton.onClick.AddListener(OnCreateTestRoomButtonClicked);         
        BackButton.onClick.AddListener(OnBackButtonClicked);
        SceneSelectButton.onClick.AddListener(OnSceneSelectButtonClicked);
        SceneSelectStartButton.onClick.AddListener(OnSceneSelectStartButtonClicked);

        lobbyPanel = canvas.GetComponent<LobbyPanel>();
        if (folderPath == null)
        {
            folderPath = "Assets/Scenes/TestScenes";
        }
        if (sceneEntryPath == null)
        {
            sceneEntryPath = "Prefabs/LobbyScene/SceneConnectButton";
        }
        GetSceneArray(SceneScrollViewContent.transform);
        SceneScrollView.SetActive(false);
    }

    public void GetSceneArray(Transform parentTransform)    
    {
        string[] sceneFiles = Directory.GetFiles(folderPath, "*.unity").Select(Path.GetFileNameWithoutExtension).ToArray();
        GameObject SceneEntry;
        foreach (string sceneName in sceneFiles)
        {
            SceneEntry = Instantiate(Resources.Load<GameObject>(sceneEntryPath));
            SceneEntry.transform.SetParent(parentTransform, false);
            SceneScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 75);
            var sceneConnectButton = SceneEntry.GetComponent<SceneConnectButton>();
            sceneConnectButton.Initialize(sceneName, false);
            sceneConnectButtons.Add(sceneConnectButton);
        }
        foreach (var sceneButtons in sceneConnectButtons)
        {
            sceneButtons.InitializeButtonList(sceneConnectButtons);
        }
    }

    #region Button
    public string OnRoomNameChanged(string roomName)
    {
        return roomName;
    }
    private void OnSceneSelectButtonClicked()
    {
        SceneSelectStartButton.gameObject.SetActive(true);
        SceneSelectButton.gameObject.SetActive(false);
        SceneScrollView.SetActive(false);
    }
    private void OnSceneSelectStartButtonClicked()
    {      
        SceneSelectStartButton.gameObject.SetActive(false);
        SceneSelectButton.gameObject.SetActive(true);
        SceneScrollView.SetActive(true);
    }

    public void OnSceneConnectButtonClicked(SceneConnectButton clickedButton)
    {
        selectedSceneName = clickedButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        SceneSelectStartButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = selectedSceneName;
        foreach (SceneConnectButton button in sceneConnectButtons)
        {
            if (button != clickedButton)
            {
                button.IsSelected = false;
            }
        }
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
        if (selectedRoomName == null)
        {
            selectedRoomName = sceneConnectButtons[0].sceneNameText.text;
        }
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "IsTest", true }, { "Scene", selectedRoomName } };

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
