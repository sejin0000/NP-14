using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRoomPanel : MonoBehaviour
{
    [Header("Buttons")]
    public Button TestStartButton;
    public Button BackButton;
    public Button OpenOptionButton;

    [Header("SceneStatus")]
    public TextMeshProUGUI ConnectedSceneText;
    [SerializeField] private string connectedScene;
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
    public GameObject TestRoomOptionPopup;

    [Header("LobbyPanel")]
    public GameObject MainCanvas;
    public LobbyPanel lobbyPanel;

    private void Start()
    {
        Initialize();   
    }
    public void Initialize()
    {
        lobbyPanel = MainCanvas.GetComponent<LobbyPanel>();
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
        TestRoomOptionPopup.SetActive(true);
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

        PhotonNetwork.LoadLevel(ConnectedScene);
    }
}
