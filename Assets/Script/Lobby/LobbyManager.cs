using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Android.Gradle;
using UnityEngine;

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

    [Header("LoginPanel")]
    public LoginPanel LoginP;

    [Header("MainLobbyPanel")]
    public MainLobbyPanel MainLobbyP;

    //[Header("MainRoomPanel")]
    //public MainRoomPanel MainRoomP;

    [Header("CharaceterSelectPopup")]
    public CharacterSelectPopup CharacterSelect;
    public GameObject _CharacterSelectPopup;
    public bool IsCharacterSelectInitialized;

    //[Header("TestLobbyPanel")]
    //public TestLobbyPanel TestLobbyP;

    //[Header("TestRoomPanel")]
    //public TestRoomPanel TestRoomP;

    [Header("ClientPlayer")]
    public GameObject instantiatedPlayer;
    public int ViewID;
    public int ClassNum;

    // RoomEntries
    public Dictionary<string, RoomInfo> cachedTestRoomList;
    public Dictionary<string, GameObject> testRoomListEntries;
    private Dictionary<string, RoomInfo> cachedRoomList;

    public void Awake()
    {
        // DESC : singleton
        if (Instance == null)
        {
            Instance = this;
        }

        // DESC : Instantiate Dictionaries
        cachedRoomList = new Dictionary<string, RoomInfo>();        
        cachedTestRoomList = new Dictionary<string, RoomInfo>();
        testRoomListEntries = new Dictionary<string, GameObject>();

        // DESC : CharacterSelectPopup 초기화
        CheckCharacterSelectPopup();
        if (!IsCharacterSelectInitialized)
        {
            InstantiateCharacterSelectPopup();
        }

        // DESC : 커스텀 프로퍼티 - Char_Class 추가
        ClassNum = CharacterSelect.GetCharClass();
    }

    public override void OnJoinedLobby()
    {
        if (cachedRoomList != null) 
        {
            cachedRoomList.Clear();
        }
    }


    #region CharacterSelectPopup
    public void CheckCharacterSelectPopup()
    {
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            if (this.transform.parent.GetChild(i).GetComponent<CharacterSelectPopup>())
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
        _CharacterSelectPopup = Instantiate(Resources.Load<GameObject>("Prefabs/LobbyScene/CharacterSelectPopup"), this.transform.parent);
        _CharacterSelectPopup.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        CharacterSelect = _CharacterSelectPopup.GetComponent<CharacterSelectPopup>();
        _CharacterSelectPopup.SetActive(false);
    }
    #endregion

    #region Button
    public void OnTestLobbyButtonClicked()
    {
        SetPanel(Enum.GetName(typeof(PanelType), PanelType.TestLobbyPanel));
    }

    public void OnGameRoomButtonClicked()
    {
        SetPanel(Enum.GetName(typeof(PanelType), PanelType.RoomPanel));
    }
    #endregion

    #region Utility
    public void SetPanel(string panelName)
    {
        LoginP.gameObject.SetActive(panelName.Equals(LoginP.name));
        MainLobbyP.gameObject.SetActive(panelName.Equals(MainLobbyP.name));
        //RoomP.SetActive(panelName.Equals(RoomP.name));
        //TestLobbyP.SetActive(panelName.Equals(TestLobbyP.name));
        //TestRoomP.SetActive(panelName.Equals(TestRoomP.name));
    }
    #endregion
}
