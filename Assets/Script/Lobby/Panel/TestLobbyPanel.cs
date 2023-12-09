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
using System.Net;

public class TestLobbyPanel : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    public Button EnterTestRoomButton;
    public Button CreateTestRoomButton;
    public Button SceneSelectButton;
    public Button SceneSelectStartButton;
    public Button CharacterSelectButton;
    public Button BackButton;


    [Header("CurrentRoomBoard")]
    [SerializeField] public GameObject RoomScrollViewContent;    

    [Header("CreateRoomBoard")]
    [SerializeField] public TMP_InputField RoomNameSetup;
    [SerializeField] public TMP_InputField RoomMemberSetup;
    [SerializeField] private GameObject SceneScrollViewContent;
    [SerializeField] private GameObject SceneScrollView;

    [Header("TestRoomInfo")]
    [SerializeField] private string selectedSceneName;
    public string SelectedSceneName
    {
        get { return selectedSceneName; } 
        set 
        {
            if (selectedSceneName != value) 
            {
                LobbyManager.Instance.SelectedSceneName = value;
                Debug.Log($"TestLobbyPanel - ConnectedScene : {LobbyManager.Instance.SelectedSceneName}");
                selectedSceneName = value;
            }
        }
    }
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
    private Dictionary<string, GameObject> testRoomListEntries;
    private Dictionary<string, RoomInfo> cachedTestRoomList;

    public event Action OnEntryClicked;
    public GameObject canvas;
    private LobbyPanel lobbyPanel;
    public List<SceneConnectButton> sceneConnectButtons;

    [HideInInspector]
    private string folderPath;
    private string sceneEntryPath;
    private string testOrNotRP;
    private string testSCeneRP;

    public void Awake()
    {
        // DESC : get CustomProperty key
        testOrNotRP = CustomProperyDefined.TEST_OR_NOT;
        testSCeneRP = CustomProperyDefined.TEST_SCENE;

        // DESC : Initialize testRoomListEntries
        testRoomListEntries = new Dictionary<string, GameObject>();
        cachedTestRoomList = new Dictionary<string, RoomInfo>();

        // DESC : get path
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

        // DESC : connect buttons
        EnterTestRoomButton.onClick.AddListener(OnEnterTestRoomButtonClicked);
        SceneSelectButton.onClick.AddListener(OnSceneSelectButtonClicked);
        SceneSelectStartButton.onClick.AddListener(OnSceneSelectStartButtonClicked);
        CreateTestRoomButton.onClick.AddListener(OnCreateTestRoomButtonClicked);
        CharacterSelectButton.onClick.AddListener(LobbyManager.Instance.CharacterSelect.OnCharacterButtonClicked);
        BackButton.onClick.AddListener(LobbyManager.Instance.OnBackButtonClickedInTestLobbyPanel);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (LobbyManager.Instance.CurrentState != PanelType.TestLobbyPanel)
        {
            return;
        }

        ClearTestRoomListView();
        UpdateCachedTestRoomList(roomList);
        UpdateTestRoomListView();
    }

    #region TestRoomListUpdate

    private void ClearTestRoomListView()
    {
        foreach (GameObject entry in testRoomListEntries.Values)
        {
            Destroy(entry.gameObject);
        }

        testRoomListEntries.Clear();
    }

    private void UpdateTestRoomListView()
    {
        var cachedTestRoomList = NetworkManager.Instance.cachedTestRoomList;
        testRoomListEntries.Clear();

        foreach (RoomInfo info in cachedTestRoomList.Values)
        {
            GameObject entry = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/TestRoomEntry"), RoomScrollViewContent.transform, false);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, (byte)info.MaxPlayers);
            OnEntryClicked += entry.GetComponent<RoomListEntry>().OnSelectRoomButtonClicked;
            testRoomListEntries[info.Name] = entry;
        }
    }

    private void UpdateCachedTestRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (cachedTestRoomList.ContainsKey(info.Name))
            {
                cachedTestRoomList[info.Name] = info;
            }
            else
            {
                cachedTestRoomList.Add(info.Name, info);
            }

            if (info.CustomProperties.TryGetValue(testOrNotRP, out object testBool))
            {
                if (
                    testBool != null
                    &&(!info.IsOpen 
                        || !info.IsVisible 
                        || info.RemovedFromList 
                        || info.PlayerCount == 0 
                        || (testBool != null && !(bool)testBool))
                    )
                {
                    if (cachedTestRoomList.ContainsKey(info.Name))
                    {
                        cachedTestRoomList.Remove(info.Name);
                    }
                    continue;
                }
            }

        }
    }

    #endregion

    public void GetSceneArray(Transform parentTransform)    
    {
        //string[] sceneFiles = Directory.GetFiles(folderPath, "*.unity").Select(Path.GetFileNameWithoutExtension).ToArray();
        // TODO
        string[] sceneFiles = new string[2] { "Test_JiMin", "Test_WooMin" };
        GameObject SceneEntry;
        foreach (string sceneName in sceneFiles)
        {
            SceneEntry = Instantiate(Resources.Load<GameObject>(sceneEntryPath));
            SceneEntry.transform.SetParent(parentTransform, false);
            SceneScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 75);
            var sceneConnectButton = SceneEntry.GetComponent<SceneConnectButton>();
            sceneConnectButton.Initialize(sceneName, false, null, this);
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
        SelectedSceneName = clickedButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        Debug.Log($"{SelectedSceneName} º±≈√µ .");
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
        foreach (GameObject entry in NetworkManager.Instance.testRoomListEntries.Values)
        {
            var entryInfo = entry.GetComponent<RoomListEntry>();
            if (entryInfo.isEntryClicked)
            {
                SelectedRoomName = entryInfo.roomName;
            }
        }
        PhotonNetwork.JoinRoom(selectedRoomName);        
    }

    private void OnCreateTestRoomButtonClicked()
    {
        string roomName = RoomMemberSetup.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + UnityEngine.Random.Range(1000, 10000) : roomName;

        byte maxPlayers;
        byte.TryParse(RoomMemberSetup.text, out maxPlayers);
        maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };
        if (SelectedSceneName == null)
        {
            SelectedSceneName = sceneConnectButtons[0].sceneNameText.text;
        }
        options.CustomRoomPropertiesForLobby = new string[] { testOrNotRP, testSCeneRP };
        options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { testOrNotRP, true }, { testSCeneRP, SelectedSceneName } };

        PhotonNetwork.CreateRoom(roomName, options);
    }

    #endregion
}
