using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;

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

    public Button StartButton;
    public Button ReadyButton;

    public TMP_InputField ChatInput;
    public GameObject ChatLogPrefab;
    public GameObject ChatBoxScrollContent;
    
    [Header("CharacterSelectPopup")]
    public GameObject CharacterSelectPopup;
    public TextMeshProUGUI PlayerClassText;
    public GameObject StatInfo;
    public TextMeshProUGUI SkillInfoText;
        
    private Dictionary<int, GameObject> playerInfoListEntries;
    private Dictionary<string, RoomInfo> cachedRoomList;
    public GameObject player;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        cachedRoomList = new Dictionary<string, RoomInfo>();
        playerInfoListEntries = new Dictionary<int, GameObject>();

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
            SetPanel(LoginPanel.name);
        }
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
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
        GameObject go = Instantiate(Resources.Load<GameObject>("Pefabs/Player"));
        go.transform.SetParent(player.transform);
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

    // ������ ã�� �������� ��
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = $"RandRoom{Random.Range(1,200)}";
        RoomOptions options = new RoomOptions { MaxPlayers = 3 };
        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName} ����");
        if (cachedRoomList != null)
        { 
            cachedRoomList.Clear();        
        }

        SetPanel(RoomPanel.name);

        if (playerInfoListEntries == null)
        {
            playerInfoListEntries = new Dictionary<int, GameObject>();
        }

        // PartyPlayerInfo���� ���� ������ ������ ������ �����տ� ����.
        SetPartyPlayerInfo();

        StartButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnLeftRoom()
    {
        SetPanel(MainLobbyPanel.name);

        foreach (GameObject playerInfo in playerInfoListEntries.Values) 
        {
            Destroy(playerInfo.gameObject);
        }

        playerInfoListEntries.Clear();
        playerInfoListEntries = null;

        if (ChatBoxScrollContent.transform.childCount > 0)
        {
            for (int i = 0; i < ChatBoxScrollContent.transform.childCount; i++)
            {
                Destroy(ChatBoxScrollContent.transform.GetChild(i).gameObject);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName} ����");

        SetPartyPlayerInfo();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (playerInfoListEntries[otherPlayer.ActorNumber].gameObject != null)
        {
            Destroy(playerInfoListEntries[otherPlayer.ActorNumber].gameObject);
        }
        playerInfoListEntries.Remove(otherPlayer.ActorNumber);

        Debug.Log($"{otherPlayer.NickName} ����");

        SetPartyPlayerInfo();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            // TODO : ȣ��Ʈ�� �ٲ��, �ٲ� ������Ը� StartButton�� Ȱ��ȭ�Ǿ����.
            // �� readybutton ��Ȱ��ȭ�� ������Ŭ���̾�Ʈ������ �̷������...
            ReadyButton.gameObject.SetActive(false);
            StartButton.gameObject.SetActive(CheckPlayersReady());
        }
        else
        {
            if (ReadyButton.gameObject == null)
            {
                ReadyButton.gameObject.SetActive(true);
            }
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
            SetPartyPlayerInfo();
        }

        StartButton.gameObject.SetActive(CheckPlayersReady());
    }


    #region Button

    public void OnLoginButtonClicked()
    {
        // �α��� �κ� üũ ��, ����
        // �ϴ���, �ٸ� �г� Ȱ��ȭ�� ��ü
        if (PlayerIdInput.text == "")
        {
            PhotonNetwork.LocalPlayer.NickName = $"Player_{Random.Range(0, 2000)}";
        }
        else
        {
            PhotonNetwork.LocalPlayer.NickName = PlayerIdInput.text;
        }
        // �г��� ���� �κ� ���� �ľ���.
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����
        }

    }

    public void OnJoinRandomRoomButtonClicked()
    {
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

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainGameScene");
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

    public bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        if (PhotonNetwork.PlayerList.Count() < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            return false;
        }
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue("IsPlayerReady", out isPlayerReady))
            {
                if (!(bool) isPlayerReady && !p.IsMasterClient)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    public void SetPartyPlayerInfo()
    {
        playerInfoListEntries.Clear();

        for (int i = 0; i < PartyBox.transform.childCount; i++)
        {
            if (PartyBox.transform.GetChild(i).childCount > 0)
            {
                Destroy(PartyBox.transform.GetChild(i).GetChild(0).gameObject);
            }
        }

        int cnt = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject playerInfoPrefab = Instantiate(PlayerInfo);
            playerInfoPrefab.transform.SetParent(PartyBox.transform.GetChild(cnt), false);
            playerInfoPrefab.transform.localScale = Vector3.one;
            playerInfoPrefab.GetComponent<PartyPlayerInfo>().Initialize(cnt, p);

            playerInfoListEntries.Add(p.ActorNumber, playerInfoPrefab);
            cnt++;
        }
    }
    #endregion
}
