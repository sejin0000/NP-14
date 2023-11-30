using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class RoomFindPanel : MonoBehaviour
{
    [Header("RoomFindPanel")]
    public Button BackButton;
    public Button RoomCreateButton;

    private bool isCreatePopupOpen;
    public bool IsCreatePopupOpen
    {
        get { return isCreatePopupOpen; }
        set 
        { 
            isCreatePopupOpen = value;
            if (value)
            {
                CreateRoomPopup.SetActive(true);
                JoinRoomPopup.SetActive(false);
            }
            else
            {
                CreateRoomPopup.SetActive(false);
                JoinRoomPopup.SetActive(true);
            }
        }
    }

    [Header("CreateRoomPopup")]
    public GameObject CreateRoomPopup;
    public TMP_InputField RoomNameInput;
    private bool isInputActivated;    
    public Button DenyRandomButton;
    public Button AllowRandomButton;    
    public Button CancelCreateButton;
    private Color denyOriginColor;
    private Color allowOriginColor;

    [Header("JoinRoomPopup")]
    public GameObject JoinRoomPopup;
    //public ScrollView RoomScrollView;
    public GameObject RoomScrollViewContent;
    public Button ResetButton;
    public Button RoomSearchButton;
    public TMP_InputField RoomSearchInput;

    [Header("RoomEntries")]
    public GameObject RoomEntryPrefab;

    [HideInInspector]
    public Hashtable CustomRoomProperties;
    private string testProp;
    private string randomProp;
    private bool isRandom;

    public event Action OnRoomEntryClicked;

    public void Start()
    {
        denyOriginColor = DenyRandomButton.GetComponent<Image>().color;
        allowOriginColor = AllowRandomButton.GetComponent<Image>().color;

        testProp = CustomProperyDefined.TEST_OR_NOT;
        randomProp = CustomProperyDefined.RANDOM_OR_NOT;
        isRandom = true;
        CustomRoomProperties = new Hashtable() { { testProp, false }, { randomProp, isRandom } };

        BackButton.onClick.AddListener(OnBackButtonClicked);
        RoomCreateButton.onClick.AddListener(OnRoomCreateButtonClicked);
        DenyRandomButton.onClick.AddListener(OnDenyRandomButtonClicked);
        AllowRandomButton.onClick.AddListener(OnAllowRandomButtonClicked);
        CancelCreateButton.onClick.AddListener(OnCancelCreateButtonClicked);
        RoomSearchButton.onClick.AddListener(OnRoomSearchButtonClicked);
        ResetButton.onClick.AddListener(OnResetButtonClicked);
    }

    public void OnEnable()
    {
        IsCreatePopupOpen = false;
        isInputActivated = false;
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            && !IsCreatePopupOpen)
        {
            if (!isInputActivated)
            {
                isInputActivated = true;
                RoomSearchInput.ActivateInputField();
            }
            else
            {
                // TODO : 받은 RoomSearchInput으로 방 검색하는 메서드
                OnRoomSearchButtonClicked();
            }
        }
    }
    public void OnBackButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
    }

    public void OnRoomCreateButtonClicked()
    {
        if (isCreatePopupOpen)
        {
            RoomCreateButton.GetComponent<Image>().color = Color.white;

            string roomName = RoomNameInput.text;
            if (roomName == "")
            {
                roomName = $"Room {Random.Range(0, 200)}";
            }
            RoomNameInput.text = "";

            RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 1500 };
            options.CustomRoomProperties = CustomRoomProperties;
            options.CustomRoomPropertiesForLobby = new string[] { testProp, randomProp };

            PhotonNetwork.CreateRoom(roomName, options);
        }
        else
        {
            IsCreatePopupOpen = true;
            RoomCreateButton.GetComponent<Image>().color = new Color(125f / 255f, 236f / 255f, 129f / 255f);
        }
    }

    public void OnDenyRandomButtonClicked()
    {
        CustomRoomProperties[randomProp] = false;
        DenyRandomButton.GetComponent<Image>().color = new Color(145f / 255f, 52f / 255f, 52f / 255f);
        AllowRandomButton.GetComponent<Image>().color = allowOriginColor;
    }

    public void OnAllowRandomButtonClicked()
    {
        CustomRoomProperties[randomProp] = true;
        DenyRandomButton.GetComponent<Image>().color = denyOriginColor;
        AllowRandomButton.GetComponent<Image>().color = new Color(200f / 255f, 200f / 255f, 200f / 255f);
    }

    public void OnCancelCreateButtonClicked()
    {
        DenyRandomButton.GetComponent<Image>().color = denyOriginColor;
        AllowRandomButton.GetComponent<Image>().color = allowOriginColor;
        RoomCreateButton.GetComponent<Image>().color = allowOriginColor;
        CustomRoomProperties[randomProp] = false;
        RoomNameInput.text = "";

        IsCreatePopupOpen = false;
    }
    public void ClearRoomListView()
    {
        for (int i = 0; i < RoomScrollViewContent.transform.childCount; i++) 
        {
            Destroy(RoomScrollViewContent.transform.GetChild(i).gameObject);
        }
    }
    public void OnRoomSearchButtonClicked()
    {
        ClearRoomListView();
        foreach (RoomInfo info in NetworkManager.Instance.cachedRoomList.Values)
        {
            if (info.Name.Contains(RoomSearchInput.text)) 
            {
                GameObject entry = Instantiate(RoomEntryPrefab, RoomScrollViewContent.transform, false);
                var gridLG = RoomScrollViewContent.GetComponent<GridLayoutGroup>();
                RoomScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, gridLG.cellSize.y + gridLG.padding.bottom);
                entry.transform.localScale = Vector3.one;

                entry.GetComponent<RoomEntry>().Initialize(info.Name, (byte)info.PlayerCount);
                OnRoomEntryClicked += entry.GetComponent<RoomEntry>().OnRoomEntryButtonClicked;
                NetworkManager.Instance.RoomFindEntriesList[info.Name] = entry;
            }
        }
    }

    public void OnResetButtonClicked()
    {
        RoomSearchInput.text = "";
        OnRoomSearchButtonClicked();
    }
    public void UpdateFindRoomListView()
    {
        ClearRoomListView();
        foreach (RoomInfo info in NetworkManager.Instance.cachedRoomList.Values)
        {
            GameObject entry = Instantiate(RoomEntryPrefab, RoomScrollViewContent.transform, false);
            var gridLG = RoomScrollViewContent.GetComponent<GridLayoutGroup>();
            RoomScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, gridLG.cellSize.y + gridLG.padding.bottom);
            entry.transform.localScale = Vector3.one;

            entry.GetComponent<RoomEntry>().Initialize(info.Name, (byte)info.PlayerCount);
            OnRoomEntryClicked += entry.GetComponent<RoomEntry>().OnRoomEntryButtonClicked;
            NetworkManager.Instance.RoomFindEntriesList[info.Name] = entry;
        }
    }
}
