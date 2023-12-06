using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
//using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class RoomPanel : MonoBehaviourPunCallbacks
{
    [Header("button")]
    [SerializeField] public Button ReadyButton;
    [SerializeField] public Button StartButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button characterSelectButton;

    [Header("Chat")]
    public TMP_InputField ChatInputField;
    public Button SubmitButton;
    public GameObject ChatBox;
    private GameObject ChatLog;
    public GameObject ChatScrollContent;
    public ScrollRect ChatScrollRect;
    private bool isChatBoxActive;
    public bool IsChatBoxActive
    {
        get { return  isChatBoxActive; }
        set
        {
            ChatBox.SetActive(value);
            isChatBoxActive = value;
            if (value)
            {
                if (DeactiveChatBox != null)
                {
                    StopCoroutine(DeactiveChatBox);
                    DeactiveChatBox = null;
                }
                DeactiveChatBox = StartCoroutine(DeActiveChatBox());
            }
        }
    }
    private bool isChatInputActive;
    public bool IsChatInputActive
    {
        get { return isChatInputActive; }
        set
        {
            if (LobbyManager.Instance == null)
            {
                isChatInputActive = false;
                return;
            }
            var inputActions = LobbyManager.Instance.instantiatedPlayer.GetComponent<PlayerInput>().actions;
            if (value)
            {
                ChatInputField.ActivateInputField();
                isChatInputActive = value;
                inputActions.Disable();
            }
            else
            {
                ChatInputField.DeactivateInputField();
                isChatInputActive = value;
                inputActions.Enable();
                LobbyManager.Instance.instantiatedPlayer.GetComponent<PlayerInputController>().ResetSetting();
            }
        }
    }

    [Header("PartyBox")]
    public GameObject PartyBox;

    [HideInInspector]
    private string askReadyProp;
    private Dictionary<int, GameObject> _playerPartyDict;
    private Coroutine DeactiveChatBox;


    public void Awake()
    {
        askReadyProp = CustomProperyDefined.ASK_READY_PROPERTY;        
    }

    public void OnEnable()
    {
        // DESC : �÷��̾� ���� �ʱ�ȭ
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(askReadyProp, out object isPlayerReady);
        if (isPlayerReady == null)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { askReadyProp, false } });
        }

        // DESC : ������ Ŭ���̾�Ʈ ���� Ȯ��
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("RoomPanel - This is MasterClient");
        }

        // DESC : ��ŸƮ ��ư ��Ȱ��ȭ ( ��� �غ���� ��, Ȱ��ȭ )
        StartButton.gameObject.SetActive(false);

        // DESC : ���� ���� ��ư ��Ȱ��ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            ReadyButton.gameObject.SetActive(false);
        }
        else
        {
            ReadyButton.gameObject.SetActive(true);
        }

        // DESC : ChatBox Ȱ��ȭ
        IsChatBoxActive = true;
        IsChatInputActive = false;
    }
    public void Start()
    {
        // DESC : ��ư ����
        ReadyButton.onClick.AddListener(OnReadyButtonClicked);
        StartButton.onClick.AddListener(OnStartButtonClicked);
        BackButton.onClick.AddListener(NetworkManager.Instance.OnBackButtonClickedInRoomPanel);        
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
        characterSelectButton.onClick.AddListener(LobbyManager.Instance.CharacterSelect.OnCharacterButtonClicked);
    }

    public void Update()
    {        
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ActivateChatMode();
        }
    }

    public void ActivateChatMode()
    {
        if (IsChatBoxActive)
        {
            if (!IsChatInputActive)
            {                
                IsChatInputActive = true;
            }
            else
            {
                OnSubmitButtonClicked();                
                IsChatInputActive = false;
            }
        }
        else
        {
            IsChatBoxActive = true;
            IsChatInputActive = true;
        }       
    }

    public IEnumerator DeActiveChatBox()
    {
        yield return new WaitForSeconds(5f);
        if (!IsChatInputActive)
        {
            IsChatBoxActive = false;
        }
        else
        {
            StartCoroutine(DeActiveChatBox());
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

            var readyProp = p.CustomProperties[askReadyProp];
            bool isReady;
            if (readyProp == null)
            {
                isReady = false;
            }
            else
            {
                isReady = (bool)p.CustomProperties[askReadyProp];
            }
            //p.CustomProperties.TryGetValue(askReadyProp, out object isReady);
            //if (isReady == null)
            //{
            //    isReady = false;
            //}
            Debug.Log($"Player : {p.ActorNumber} / IsReady : {(bool)isReady}");
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
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "���";
            ReadyButton.GetComponentInChildren<Image>().color = new Color(130 / 255f, 241 / 255f, 96 / 255f);
        }
        else
        {
            ReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "�غ�";
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
        Destroy(AudioManager.Instance.gameObject);
        PhotonNetwork.LoadLevel("MainGameScene");
    }

    public void OnSubmitButtonClicked()
    {
        if (ChatInputField.text == "")
        {
            ChatInputField.DeactivateInputField();
            return;
        }
        string inputText = ChatInputField.text;
        string nickName = PhotonNetwork.LocalPlayer.NickName;

        ChatLog = Resources.Load<GameObject>(PrefabPathes.CHAT_LOG_PREFAB);
        GameObject chatPrefab = Instantiate(ChatLog, ChatScrollContent.transform, false);
        ChatLog chatLog = chatPrefab.GetComponent<ChatLog>();

        chatLog.Initialize(nickName, inputText);

        float increasedHeight = chatLog.ConfirmTextSize(ChatInputField);

        photonView.RPC("ChatInput", RpcTarget.Others, inputText, nickName, increasedHeight);

        ChatScrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, increasedHeight);
        ChatScrollRect.verticalNormalizedPosition = 0f;
                
        ChatInputField.text = ""; 
    }

    [PunRPC]
    public void ChatInput(string inputText, string nickName, float increasedHeight)
    {
        GameObject chatPrefab = Instantiate(ChatLog, ChatScrollContent.transform, false);
        ChatLog chatLog = chatPrefab.GetComponent<ChatLog>();

        chatLog.Initialize(nickName, inputText);

        chatLog.GetCurrentAmount();
        chatLog.GetComponent<RectTransform>().sizeDelta = new Vector2(chatLog.prefabWidth, increasedHeight);
        chatLog.ChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(chatLog.ChatText.gameObject.GetComponent<RectTransform>().rect.width, increasedHeight);
        ChatScrollContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, increasedHeight);

        // DESC : Ÿ Ŭ���̾�Ʈ ä��â ������� �ʰ�
        IsChatBoxActive = true;

        // DESC : ä��â ���� �Ʒ�(���� �ֱ� ��)�� ����ǰ�
        ChatScrollRect.verticalNormalizedPosition = 0f;
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

        // DESC : �ٽ� �غ� ����..
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
