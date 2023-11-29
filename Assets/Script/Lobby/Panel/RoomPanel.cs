using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    [Header("button")]
    [SerializeField] private Button ReadyButton;
    [SerializeField] public Button StartButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button characterSelectButton;

    [Header("Chat")]
    public TMP_InputField ChatInputField;
    public Button SubmitButton;
    public GameObject ChatLog;
    public GameObject ChatScrollContent;

    [Header("PartyBox")]
    public GameObject PartyBox;

    [HideInInspector]
    private string askReadyProp;
    private Dictionary<int, GameObject> _playerPartyDict;

    public void Awake()
    {
        askReadyProp = CustomProperyDefined.ASK_READY_PROPERTY;        
    }

    public void OnEnable()
    {
        // DESC : 플레이어 레디 초기화
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady);
        if (isPlayerReady == null)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { askReadyProp, false } });
        }
    }
    public void Start()
    {

        // DESC : 버튼 연결
        ReadyButton.onClick.AddListener(OnReadyButtonClicked);
        StartButton.onClick.AddListener(OnStartButtonClicked);
        BackButton.onClick.AddListener(NetworkManager.Instance.OnBackButtonClickedInRoomPanel);        
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
        characterSelectButton.onClick.AddListener(LobbyManager.Instance.CharacterSelect.OnCharacterButtonClicked);

        // DESC : 스타트 버튼 비활성화 ( 모두 준비됬을 시, 활성화 )
        StartButton.gameObject.SetActive(false);

        // DESC : 방장 레디 버튼 비활성화
        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.gameObject.SetActive(false);
        }
    }

    public void SetPartyPlayerInfo()
    {
        _playerPartyDict = LobbyManager.Instance.playerPartyDict;

        if (_playerPartyDict == null)
        {
            _playerPartyDict = new Dictionary<int, GameObject>();
        }

        _playerPartyDict.Clear();

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
            GameObject playerInfoPrefab = Instantiate(Resources.Load<GameObject>(PrefabPathes.PLAYER_INROOM_PARTY_ELEMENT), PartyBox.transform.GetChild(cnt), false);            
            playerInfoPrefab.transform.localScale = Vector3.one;
            var partyPlayerInfo = playerInfoPrefab.GetComponent<PartyPlayerInfo>();
            partyPlayerInfo.Initialize(cnt, p);

            p.CustomProperties.TryGetValue(askReadyProp, out object isReady);

            if (isReady == null)
            {
                isReady = false;
            }
            partyPlayerInfo.SetReady((bool)isReady);

            _playerPartyDict.Add(p.ActorNumber, playerInfoPrefab);

            cnt++;
        }
    }

    public GameObject InstantiatePlayer()
    {
        string playerPath = PrefabPathes.PLAYER_INROOM_PREFAB_PATH;
                
        PlayerDataSetting playerData = LobbyManager.Instance.dataSetting;
        GameObject player = PhotonNetwork.Instantiate(playerPath, Vector3.zero, Quaternion.identity);

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomProperyDefined.CLASS_PROPERTY, out object classNum);
        playerData.SetClassType((int)classNum, player);

        return player;
    }

    public void SetPlayerReady(bool playerReady)
    {
        if (playerReady)
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "취소";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(130 / 255f, 241 / 255f, 96 / 255f);
        }
        else
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "준비";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(255 / 255f, 182 / 255f, 182 / 255f);
        }
    }

    public void OnReadyButtonClicked()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady))
        {
            isPlayerReady = !(bool)isPlayerReady;
            SetPlayerReady((bool)isPlayerReady);
        }
        else
        {
            isPlayerReady = true;
            SetPlayerReady(true);
        }
        var props = new Hashtable() { { askReadyProp, isPlayerReady } };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public void OnStartButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("MainGameScene");
    }

    public void OnSubmitButtonClicked()
    {
        string inputText = ChatInputField.text;
        string nickName = PhotonNetwork.LocalPlayer.NickName;
        photonView.RPC("ChatInput", RpcTarget.All, inputText, nickName);
        ChatInputField.text = "";
        ChatInputField.ActivateInputField();
    }

    [PunRPC]
    public void ChatInput(string inputText, string nickName)
    {
        GameObject chatPrefab = Instantiate(ChatLog, ChatScrollContent.transform, false);
        ChatLog chatLog = chatPrefab.GetComponent<ChatLog>();

        chatLog.NickNameText.text = nickName;
        chatLog.ChatText.text = inputText;
        chatPrefab.transform.SetParent(ChatScrollContent.transform, false);
        chatLog.ConfirmTextSize(ChatInputField);
    }

    public void LeaveRoomSetting()
    {
        if (ChatScrollContent.transform.childCount > 0)
        {
            for (int i = 0; i < ChatScrollContent.transform.childCount; i++)
            {
                Destroy(ChatScrollContent.transform.GetChild(i).gameObject);
            }
        }

        // DESC : 다시 준비 해제..
        PhotonNetwork.LocalPlayer.CustomProperties[askReadyProp] = false;
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
            if (p.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady))
            {
                if (!(bool)isPlayerReady && !p.IsMasterClient)
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
}
