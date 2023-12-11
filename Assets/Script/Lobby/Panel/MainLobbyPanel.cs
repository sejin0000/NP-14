using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class MainLobbyPanel : MonoBehaviourPun
{
    [Header("Button")]
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button testLobbyButton;
    [SerializeField] private Button quickStartButton;
    [SerializeField] private Button findRoomButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button backButton;
    private List<Button> ButtonList;

    [Header("CharacterSelectPopup")]
    private CharacterSelectPopup playerInfo;
    private GameObject characterSelectPopup;

    [Header("Player")]
    private GameObject Player;
    private string playerPrefabPath;

    [Header("SetupPopup")]
    [SerializeField] public GameObject SetupPopup;
    //

    public TextMeshProUGUI Gold; // TODO : 뒤끝베이스에 골드량 추가 예정

    private GraphicRaycaster raycaster;

    private void Start()
    {
        // DESC
        PhotonNetwork.AutomaticallySyncScene = true;

        // DESC : check CharacterSelectPopup
        if (LobbyManager.Instance.IsCharacterSelectInitialized)
        {
            characterSelectPopup = LobbyManager.Instance._CharacterSelectPopup;
            playerInfo = LobbyManager.Instance.CharacterSelect;
        }
        else
        {
            Debug.Log("LobbyManager didn't Initialize characterSelectPopup");
        }

        // DESC : connect buttons        
        characterSelectButton.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
        testLobbyButton.onClick.AddListener(OnTestLobbyButtonClicked);
        quickStartButton.onClick.AddListener(OnQuickStartButtonClicked);
        findRoomButton.onClick.AddListener(OnFindRoomButtonClicked);
        settingButton.onClick.AddListener(OnSettingButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);

        ButtonList = new List<Button>
        {           
            findRoomButton,
            quickStartButton,
            settingButton,
        };

        GetButtonEventTrigger();
        // DESC : instantiate Player
        //InstantiatePlayer();

        // DESC : 커스텀 프로퍼티 - Char_Class 추가
        LobbyManager.Instance.ClassNum = playerInfo.GetCharClass();

        // DESC : GraphicRaycaster 컴포넌트
        raycaster = GetComponent<GraphicRaycaster>();
    }


    private void GetButtonEventTrigger()
    {
        foreach (Button button in ButtonList) 
        {
            button.AddComponent<EventTrigger>();
            var buttonEvent = button.GetComponent<EventTrigger>();
            var selectedMark = button.transform.GetChild(1).gameObject;


            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { selectedMark.SetActive(true); });

            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { selectedMark.SetActive(false); });

            buttonEvent.triggers.Add(entryEnter);
            buttonEvent.triggers.Add(entryExit);

        }
    }

    

    #region Buttons
    private void OnQuickStartButtonClicked()
    {
        var CustomRoomProperties = new Hashtable() { { CustomProperyDefined.TEST_OR_NOT, false }, { CustomProperyDefined.RANDOM_OR_NOT, true } };

        if (NetworkManager.Instance.cachedRoomList == null
            || NetworkManager.Instance.cachedRoomList.Count == 0)
        {
            Debug.Log("MainLobbyPanel : cachedRoomList is Null");
            string roomName = $"Room {Random.Range(0, 200)}";

            RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 1500 };            
            options.CustomRoomProperties = CustomRoomProperties;
            options.CustomRoomPropertiesForLobby = new string[] { CustomProperyDefined.TEST_OR_NOT, CustomProperyDefined.RANDOM_OR_NOT };

            PhotonNetwork.CreateRoom(roomName, options);            
        }
        else
        {
            Debug.Log("MainLobbyPanel : cachedRoomList is Not Null");
            PhotonNetwork.JoinRandomRoom(CustomRoomProperties, 0);            
        }
    }

    private void OnFindRoomButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.RoomFindPanel);
    }

    public void OnTestLobbyButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.TestLobbyPanel);
    }

    private void OnSettingButtonClicked()
    {
        //TODO : 테스트용
        SetupPopup.SetActive(true);
    }

    private void OnBackButtonClicked()
    {
        var announcePopup = Instantiate(Resources.Load<GameObject>(PrefabPathes.SETUP_ANNOUNCE_POPUP), transform, false);
        var setupAnnouncePopup = announcePopup.GetComponent<SetupAnnouncePopup>();
        setupAnnouncePopup.Initialize(AnnouncementType.GameEnd);
        announcePopup.transform.localPosition = Vector3.zero;
    }
    #endregion

    #region Garauge
    private void InstantiatePlayer()
    {
        LobbyManager.Instance.instantiatedPlayer = Instantiate(Resources.Load<GameObject>(PrefabPathes.PLAYER_INLOBBY_PREFAB_PATH));
    }
    #endregion
}
