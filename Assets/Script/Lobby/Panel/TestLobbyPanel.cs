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

public class TestLobbyPanel : MonoBehaviourPun
{
    [Header("Button")]
    public Button EnterTestRoomButton;
    public Button SceneSelectButton;
    public Button SceneSelectStartButton;


    [Header("CurrentRoomBoard")]
    [SerializeField] public GameObject RoomScrollViewContent;    

    [Header("CreateRoomBoard")]
    [SerializeField] public TMP_InputField RoomNameSetup;
    [SerializeField] public TMP_InputField RoomMemberSetup;
    [SerializeField] private GameObject SceneScrollViewContent;
    [SerializeField] private GameObject SceneScrollView;

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
        SceneSelectButton.onClick.AddListener(OnSceneSelectButtonClicked);
        SceneSelectStartButton.onClick.AddListener(OnSceneSelectStartButtonClicked);

        lobbyPanel = canvas.GetComponent<LobbyPanel>();
        Debug.Log("lobbyPanel Instantiated");
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
        selectedSceneName = clickedButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
        SceneSelectStartButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = selectedSceneName;
        lobbyPanel.selectedSceneInTestLobbyPanel = selectedSceneName;
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
        foreach (GameObject entry in LobbyManager.Instance.testRoomListEntries.Values)
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

    #endregion
}
