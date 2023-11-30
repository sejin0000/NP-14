using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;
using Random = UnityEngine.Random;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum PanelType
{
    LoginPanel,
    MainLobbyPanel,
    RoomPanel,
    TestLobbyPanel,
    TestRoomPanel,
    ShopPanel,
}
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;

    [Header("State")]
    public PanelType CurrentState;

    [Header("LoginPanel")]
    public LoginPanel LoginP;

    [Header("MainLobbyPanel")]
    public MainLobbyPanel MainLobbyP;

    [Header("MainRoomPanel")]
    public RoomPanel RoomP;

    [Header("CharaceterSelectPopup")]
    public CharacterSelectPopup CharacterSelect;
    public GameObject _CharacterSelectPopup;
    public bool IsCharacterSelectInitialized;

    [Header("TestLobbyPanel")]
    public TestLobbyPanel TestLobbyP;
    public string SelectedSceneName;

    [Header("TestRoomPanel")]
    public TestRoomPanel TestRoomP;

    [Header("ClientPlayer")]
    public GameObject instantiatedPlayer;
    public int ViewID;
    public int ClassNum;
    public GameObject DataSettingPrefab;
    public PlayerDataSetting dataSetting;

    [Header("LoadingPanel")]
    public LoadingPanel LoadP;

    // RoomEntries
    public Dictionary<int, GameObject> playerPartyDict;

    [HideInInspector]
    public event Action<PanelType> OnLeaveRoom;

    public void Awake()
    {
        // DESC : singleton
        if (Instance == null)
        {
            Instance = this;
        }

        // DESC : playerPartyDict 초기화
        playerPartyDict = new Dictionary<int, GameObject>();

        // DESC : CharacterSelectPopup 초기화
        CheckCharacterSelectPopup();
        if (!IsCharacterSelectInitialized)
        {
            InstantiateCharacterSelectPopup();
        }

        // DESC : DataSetting 초기화
        dataSetting = DataSettingPrefab.GetComponent<PlayerDataSetting>();
    }

    public void SetRoomPanel()
    {
        instantiatedPlayer = RoomP.InstantiatePlayer();
        ViewID = instantiatedPlayer.GetPhotonView().ViewID;
        instantiatedPlayer.GetComponent<ClassIdentifier>().playerData = dataSetting;

        dataSetting.ownerPlayer = instantiatedPlayer;
        dataSetting.viewID = ViewID;

        SetPanel(PanelType.RoomPanel);
        CharacterSelect.Initialize();

        RoomP.SetPartyPlayerInfo();
    }

    public void SetRoomPanelEntered()
    {
        object classNum;
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomProperyDefined.CLASS_PROPERTY, out classNum);
        instantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, dataSetting.viewID);
    }

    public void SetRoomPanelLeft(Player otherPlayer)
    {
        if (playerPartyDict[otherPlayer.ActorNumber].gameObject != null)
        {
            Destroy(playerPartyDict[otherPlayer.ActorNumber].gameObject);
        }
        playerPartyDict.Remove(otherPlayer.ActorNumber);

        Debug.Log($"{otherPlayer.NickName} 퇴장");

        RoomP.SetPartyPlayerInfo();
    }

    public void SetTestRoomPanel()
    {
        SetPanel(PanelType.TestRoomPanel);

        instantiatedPlayer = RoomP.InstantiatePlayer();
        ViewID = instantiatedPlayer.GetPhotonView().ViewID;
        instantiatedPlayer.GetComponent<ClassIdentifier>().playerData = dataSetting;

        dataSetting.ownerPlayer = instantiatedPlayer;
        dataSetting.viewID = ViewID;
        CharacterSelect.Initialize();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {        
        SetPanel(PanelType.MainLobbyPanel);        
        PhotonNetwork.LeaveRoom();
    }

    #region CharacterSelectPopup
    public void CheckCharacterSelectPopup()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (this.transform.GetChild(i).GetComponent<CharacterSelectPopup>())
            {
                IsCharacterSelectInitialized = true;
                var caseTransform = this.transform.parent.GetChild(i);
                _CharacterSelectPopup = caseTransform.gameObject;
                CharacterSelect = caseTransform.GetComponent<CharacterSelectPopup>();
                break;
            }
        }
    }
    public void InstantiateCharacterSelectPopup()
    {
        Debug.Log("LobbyManager - CharacterSelectPopup 인스턴스화");
        _CharacterSelectPopup = Instantiate(Resources.Load<GameObject>(PrefabPathes.CHARACTER_SELECT_POPUP), this.transform);
        _CharacterSelectPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        CharacterSelect = _CharacterSelectPopup.GetComponent<CharacterSelectPopup>();
        _CharacterSelectPopup.SetActive(false);
        IsCharacterSelectInitialized = true;
    }
    #endregion

    #region Button
    public void OnBackButtonClickedInTestLobbyPanel()
    {
        SetPanel(PanelType.MainLobbyPanel);
    }
    #endregion

    #region Utility
    public void SetPanel(PanelType panelType)
    {
        CurrentState = panelType;
        string panelName = Enum.GetName(typeof(PanelType), panelType);
        LoginP.gameObject.SetActive(panelName.Equals(LoginP.name));
        MainLobbyP.gameObject.SetActive(panelName.Equals(MainLobbyP.name));
        RoomP.gameObject.SetActive(panelName.Equals(RoomP.name));
        TestLobbyP.gameObject.SetActive(panelName.Equals(TestLobbyP.name));
        TestRoomP.gameObject.SetActive(panelName.Equals(TestRoomP.name));
    }
    #endregion
}
