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
    public Button CharacterSelectButton;

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

    [Header("RoomInfo")]
    public string currentTestScene;
    public string currentRoomNameText;
    public string currentRoomMemberText;

    private void OnEnable()
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
        
        if (LobbyManager.Instance.SelectedSceneName != null)
        {
            currentTestScene = LobbyManager.Instance.SelectedSceneName;
            ConnectedSceneText.text = $"TestRoomPanel - Selected Scene : {LobbyManager.Instance.SelectedSceneName}";
        }
        connectedScene = currentTestScene;

        // 옵션 팝업 Init
        testRoomOptionPopup = testRoomOptionPopupObject.GetComponent<TestRoomOptionPopup>();
        testRoomOptionPopup.Initialize();

        // 로비 패널 연결
        // lobbyPanel = MainCanvas.GetComponent<LobbyPanel>();

        // 버튼 연결
        TestStartButton.onClick.AddListener(OnTestStartButtonClickedInTest);
        BackButton.onClick.AddListener(NetworkManager.Instance.OnBackButtonClickedInTestRoomPanel);
        OpenOptionButton.onClick.AddListener(OnOpenOptionButtonClicked);
        CharacterSelectButton.onClick.AddListener(LobbyManager.Instance.CharacterSelect.OnCharacterButtonClicked);
    }

    public void LeaveRoomSetting()
    {
        LobbyManager.Instance.SelectedSceneName = null;
    }

    public string OnConnectedSceneChanged(string connectedScene)
    {
        return connectedScene;
    }

    public void OnOpenOptionButtonClicked()
    {
        testRoomOptionPopupObject.SetActive(true);
    }

    public void OnTestStartButtonClickedInTest()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        Debug.Log(currentTestScene);
        PhotonNetwork.LoadLevel(currentTestScene);
    }
}
