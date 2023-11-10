using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRoomPanel : MonoBehaviourPun
{
    [Header("Buttons")]
    public Button TestStartButton;
    public Button BackButton;
    public Button OpenOptionButton;

    [Header("SceneStatus")]
    public TextMeshProUGUI ConnectedSceneText;
    [SerializeField] public string connectedScene;
    public string ConnectedScene
    {
        get { return connectedScene; }
        set
        {
            if (connectedScene != value)
            {
                connectedScene = value;
                OnConnectedSceneChanged(connectedScene);
            }
        }
    }

    [Header("RoomOptionPopup")]
    public GameObject testRoomOptionPopupObject;
    public TestRoomOptionPopup testRoomOptionPopup;

    [Header("LobbyPanel")]
    public GameObject MainCanvas;
    public LobbyPanel lobbyPanel;

    [Header("RoomInfo")]
    public string currentTestScene;
    public string currentRoomNameText;
    public string currentRoomMemberText;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        //룸 현재 옵션 적용
        if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("Scene", out object roomScene))
        {
            currentTestScene = roomScene.ToString();
        }
        currentRoomNameText = PhotonNetwork.CurrentRoom.Name;
        currentRoomMemberText = PhotonNetwork.CurrentRoom.MaxPlayers.ToString();
        lobbyPanel = MainCanvas.GetComponent<LobbyPanel>();
        if (lobbyPanel.selectedSceneInTestLobbyPanel != null)
        {
            currentTestScene = lobbyPanel.selectedSceneInTestLobbyPanel;
            ConnectedSceneText.text = $"Selected Scene : {lobbyPanel.selectedSceneInTestLobbyPanel}";
        }
        connectedScene = currentTestScene;

        // 옵션 팝업 Init
        testRoomOptionPopup = testRoomOptionPopupObject.GetComponent<TestRoomOptionPopup>();
        testRoomOptionPopup.Initialize();

        // 로비 패널 연결
        lobbyPanel = MainCanvas.GetComponent<LobbyPanel>();

        // 버튼 연결
        TestStartButton.onClick.AddListener(OnTestStartButtonClickedInTest);
        BackButton.onClick.AddListener(OnTestBackButtonClickedInTest);
        OpenOptionButton.onClick.AddListener(OnOpenOptionButtonClicked);
    }
    public string OnConnectedSceneChanged(string connectedScene)
    {
        return connectedScene;
    }

    public void OnOpenOptionButtonClicked()
    {
        testRoomOptionPopupObject.SetActive(true);
    }

    public void OnTestBackButtonClickedInTest()
    {
        if (lobbyPanel == null)
        {
            Initialize();
        }
        lobbyPanel.SetPanel(lobbyPanel.TestLobbyPanel.name);
    }


    public void OnTestStartButtonClickedInTest()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        Debug.Log(currentTestScene);
        PhotonNetwork.LoadLevel(currentTestScene);
    }
}
