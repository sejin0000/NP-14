using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class LobbyPanel : MonoBehaviourPunCallbacks
{
    public enum LobbyType
    {
        Panel,
        Popup,
    }
    public enum PanelList
    {
        LoginPanel,
        MainLobbyPanel,
        RoomPanel,
        None,
    }

    public enum PopupList
    {
        CharacterSelectPopup,
        None,
    }

    public enum CharClass
    {
        Soldier,
        Shotgun,
        Sniper,
    }

    [System.Serializable]
    public class LobbyUI
    {
        public GameObject UIObject;
        public LobbyType LobbyType;
        public PanelList panelList;
        public PopupList popupList;
    }

    [Header("Login")]
    public GameObject LoginPanel;

    public TMP_InputField PlayerIdInput;
    public TMP_InputField PlayerPswdInput;

    [Header("MainLobby")]
    public GameObject MainLobbyPanel;

    public TextMeshProUGUI Gold;
    //public Player player;

    [Header("Room")]
    public GameObject RoomPanel;

    public GameObject PartyBox;
    public GameObject PlayerInfo;

    public TMP_InputField ChatInput;
    public GameObject ChatObject;

    [Header("CharacterSelectPopup")]
    public GameObject CharacterSelectPopup;
    public TextMeshProUGUI PlayerClassText;
    public GameObject StatInfo;
    public TextMeshProUGUI SkillInfoText;
        
    private Dictionary<int, GameObject> playerInfoListEntries;
    private Dictionary<string, RoomInfo> cachedRoomList;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        ExitGames.Client.Photon.Hashtable playerCP = PhotonNetwork.LocalPlayer.CustomProperties;
        if (!playerCP.ContainsKey("Char_Class"))
        {
            PhotonNetwork.SetPlayerCustomProperties(
            new ExitGames.Client.Photon.Hashtable()
            { {"Char_Class", CharClass.Soldier} }
            );
        }

    }

    public void Start()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            Debug.Log("연결완료");
            SetPanel(LoginPanel.name);
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateCachedRoomList(roomList);
    }

    public override void OnJoinedLobby()
    {
        if (cachedRoomList != null)
        {
            cachedRoomList.Clear();
        }
        SetPanel(MainLobbyPanel.name);
    }

    public override void OnLeftLobby()
    {
        cachedRoomList.Clear();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetPanel(MainLobbyPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetPanel(MainLobbyPanel.name);
    }

    // 랜덤룸 찾기 실패했을 때
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = $"RandRoom{Random.Range(1,200)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 3 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        if (cachedRoomList != null)
        { 
            cachedRoomList.Clear();        
        }

        SetPopup("None");
        SetPanel(RoomPanel.name);

        if (playerInfoListEntries == null)
        {
            playerInfoListEntries = new Dictionary<int, GameObject>();
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            GameObject playerInfo = Instantiate(PlayerInfo);
            playerInfo.transform.SetParent(PartyBox.transform.GetChild(i));

            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
            {
                Debug.Log("YOU");
            }

            // Ready 판별에 관한 부분 추가해야함
            // playerInfoPrefab에 관한 스크립트 제작 후,,
        }
    }

    public override void OnLeftRoom()
    {
        SetPopup("None");
        SetPanel(MainLobbyPanel.name);

        foreach (GameObject playerInfo in playerInfoListEntries.Values) 
        {
            Destroy(playerInfo.gameObject);
        }

        playerInfoListEntries.Clear();
        playerInfoListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject playerInfo = Instantiate(PlayerInfo);
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerInfoListEntries[otherPlayer.ActorNumber].gameObject);
        playerInfoListEntries.Remove(otherPlayer.ActorNumber);

        for (int i = 0; i < PhotonNetwork.PlayerList.Count(); i++)
        {
            GameObject playerInfo = Instantiate(PlayerInfo);
            playerInfo.transform.SetParent(PartyBox.transform.GetChild(i));
            // TODO : 플레이어 인포 부분 다른 스크립트에서 제작.
        }
            // TODO : 시작버튼 활성화 재확인
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            // TODO : 호스트가 바뀌면, 바뀐 사람에게만 StartButton이 활성화되어야함.
            //StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (playerInfoListEntries == null)
        {
            playerInfoListEntries = new Dictionary<int, GameObject>();
        }

        GameObject playerInfo;
        if (playerInfoListEntries.TryGetValue(targetPlayer.ActorNumber, out playerInfo))
        {
            // TODO : 각 CharClass 번호에 맞추어 적용 시켜주는 거
        }

        // TODO : 플레이어가 준비를 눌렀나에 따라 변해야함.
    }


    #region Button

    public void OnLoginButtonClicked()
    {
        // 로그인 부분 체크 후, 변경
        // 일단은, 다른 패널 활성화로 대체
        if (PlayerIdInput.text == "")
        {
            PhotonNetwork.LocalPlayer.NickName = $"Player_{Random.Range(0, 2000)}";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerIdInput.text;
        }
        // 닉네임 설정 부분 새로 파야함.
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결
        }

    }

    public void OnCharacterSelectButtonClicked()
    {
        SetPopup(CharacterSelectPopup.name);
    }

    public void OnClosePopupButtonClicked()
    {
        SetPopup("None");
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        //PhotonNetwork.JoinRandomOrCreateRoom();
        if (cachedRoomList == null)
        {
            string roomName = $"Room {Random.Range(0, 200)}";

            RoomOptions options = new RoomOptions { MaxPlayers = 3, PlayerTtl = 10000 };
            
            PhotonNetwork.CreateRoom(roomName, options, null);
        }
        else
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }
    #endregion


    #region UTILITY

    public void SetPanel(string panelName)
    {
        LoginPanel.SetActive(panelName.Equals(LoginPanel.name));
        MainLobbyPanel.SetActive(panelName.Equals(MainLobbyPanel.name));
        RoomPanel.SetActive(panelName.Equals(RoomPanel.name));
    }

    public void SetPopup(string popupName) 
    {
        CharacterSelectPopup.SetActive(popupName.Equals(CharacterSelectPopup.name));
    }

    // Find 써야해서 폐기
    //public void SetPanel(string activePanel)
    //{
    //    Array panelListArray = Enum.GetValues(typeof(PanelList));
    //    Array popupListArray = Enum.GetValues(typeof(PopupList));
        
    //    foreach (string panelName in panelListArray)
    //    {
    //        Transform panelTransform = transform.Find(panelName);

    //    }
    //    //LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
        
    //}

    public void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || info.RemovedFromList)
            {
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList.Remove(info.Name);
                }

                continue;
            }

            if (cachedRoomList.ContainsKey(info.Name))
            {
                cachedRoomList[info.Name] = info;
            }

            else
            {
                cachedRoomList.Add(info.Name, info);
            }
        }

    }
    #endregion
}
