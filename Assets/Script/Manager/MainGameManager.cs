using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public static MainGameManager Instance;
    public enum GameStates
    {
        Init,
        UIPlaying,
        Start,
        Playing,
        End,
        AugmentListing,
    }

    [Header("ClientPlayer")]
    public GameObject InstantiatedPlayer; // Ŭ���̾�Ʈ�� ��ȯ�� �÷��̾�
    private bool isPlayerInstantiated;    // Ŭ���̾�Ʈ�� �÷��̾ ��ȯ�ߴ��� ����
    public Dictionary<int, Transform> playerInfoDictionary;   // �� �÷��̾��� viewID�� Ű�� �ϰ� Trasform�� value�� �ϴ� ��ųʸ�

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting; // �÷��̾��� ����
    public bool isDie; // �÷��̾� ���� ����
    public int Gold;  // ���

    [HideInInspector]
    public List<int> PartyViewIDList; // ��Ƽ�� viewID ����Ʈ
    public int PartyDeathCount; // ��Ƽ�� ���� ī��Ʈ


    [Header("GameData")]
    public int currentMonsterCount; // ���� ���� ���ͼ�
    public List<MonsterData> monsterDataList; // SO���� �о�� ���� ������ ����Ʈ
    [Space(10f)]
    public StageData stageData; // �������� SO�� ������
    public int EndingStage; // ���� ��������
    [Space(10f)]
    private GameStates gameState; // ���� ������Ʈ
    public GameStates GameState
    {
        get => gameState;
        set
        {
            if (gameState != value)
            {
                gameState = value;
                OnStateChanged(value);
            }
        ; }
    }
    public bool IsStateEnded; // ������Ʈ�� �������� ����
    [Space(10f)]
    public StageDictSO stageInfo; // �������� SO ����


    [Serializable]
    public struct MonsterData // ���� ������
    {
        public int monsterNum;
        public string monsterType;
    }

    [Serializable]
    public struct StageData // ���������� ������
    {
        public int currentArea;
        public int currentStage;
        public bool isFarmingRoom;
        public bool isEventRoom;
        public bool isBossRoom;
    }

    [Header("UI")]
    public GameObject StageInfoUI; //�������� UI

    [Header("Enemy")]
    public GameObject Nav; // nav

    [HideInInspector]
    public event Action OnGameStartedEvent; // ���� ���۽� �̺�Ʈ (������)
    public event Action OnGameEndedEvent; // ���� ������ �̺�Ʈ (������)
    public event Action OnGameClearedEvent; // ���� Ŭ����� �̺�Ʈ
    public event Action OnPlayerDieEvent; // �÷��̾� ����� �̺�Ʈ
    public event Action OnGameOverEvent; // ���� ������ �̺�Ʈ
    public event Action OnOverCheckEvent; // �ٸ� �÷��̾� ����� ���� �����ڸ� ���� �̺�Ʈ

    [HideInInspector]
    public event Action OnUIPlayingStateChanged; // UI�� �÷��̽� �̺�Ʈ
    public event Action OnStartStateChanged; // ��ŸƮ ������Ʈ�� �̺�Ʈ
    public event Action OnPlayingStateChanged; // �÷��� ������Ʈ�� �̺�Ʈ
    public event Action OnEndStateChanged; // ���� �� �̺�Ʈ
    public event Action OnAugmentListingStateChanged; // ���� ������Ʈ�� �̺�Ʈ
     
    [HideInInspector]
    public int tier;  // ������ Ƽ��
    public int Ready; // �ٸ� ������� ������ �����ߴ��� ���θ� �˱� ����



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        GameState = GameStates.Init;
        IsStateEnded = false;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Prefabs/Player/Debuff", Vector3.zero, Quaternion.identity);
        }

        stageData = new StageData
        {
            currentArea = 1,
            currentStage = 1,
            isFarmingRoom = true,
        };

        playerInfoDictionary = new Dictionary<int, Transform>();
        isPlayerInstantiated = false;
        if (!isPlayerInstantiated)
        {
            isPlayerInstantiated = true;
            SpawnPlayer();
            //�̾Ʒ� 2�� �ڹ����̳��� ���ӽ��۽� �����̴°ſ� �÷��̾ ������
            PlayerResultController MakeSetting = InstantiatedPlayer.GetComponent<PlayerResultController>();
            MakeSetting.MakeManager();
            SyncPlayer();
        }
        else
        {
            InstantiatedPlayer.SetActive(true);
        }

        //�׺� ���� �ӽ�-��α�
        Nav = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/NavMesh2D"));


        // �ӽ÷� EndingStage �� 3����
        EndingStage = 3;

        OnStartStateChanged += OnStartStateChangedHandler;
        OnEndStateChanged += OnEndStateChangedHandler;
        OnAugmentListingStateChanged += OnAugmentListingStateChangedHandler;
        OnOverCheckEvent += OverCheck;
    }

    private void Start()
    {
        UIManager.Instance.StartIntro();
        GameState = GameStates.UIPlaying;
    }

    private void Update()
    {
        if (GameState == GameStates.Playing)
        {
            IsStateEnded = false;

            if (currentMonsterCount == 0)
            {
                IsStateEnded = true;
                Debug.Log("���� �� ����");
                GameState = GameStates.End;
                if (PhotonNetwork.IsMasterClient)
                {
                    photonView.RPC("SetEndState", RpcTarget.OthersBuffered);
                }
            }
        }
    }



    #region Events
    public void CallStartEvent()
    {
        OnGameStartedEvent?.Invoke();
    }

    public void CallEndEvent()
    {
        OnGameEndedEvent?.Invoke();
    }

    private void OnStateChanged(GameStates state)
    {
        switch (state)
        {
            case GameStates.UIPlaying:
                OnUIPlayingStateChanged?.Invoke();
                Debug.Log("MainGameManger : UIPlaying");
                break;
            case GameStates.Start:
                OnStartStateChanged?.Invoke();
                Debug.Log("MainGameManger : Start");
                break;
            case GameStates.Playing:
                OnPlayingStateChanged?.Invoke();
                Debug.Log("MainGameManger : Playing");
                break;
            case GameStates.End:
                OnEndStateChanged?.Invoke();
                Debug.Log("MainGameManger : End");
                break;
            case GameStates.AugmentListing:
                OnAugmentListingStateChanged?.Invoke();
                Debug.Log("MainGameManger : AugmentListing");
                break;
        }
    }

    private void OnStartStateChangedHandler()  // ��ŸƮ ������Ʈ�� ���۵� ��
    {        
        InstantiatedPlayer.SetActive(true);

        if (stageData.isFarmingRoom)
        {
            // ���� �� ������ ���� �޼���

            // ���� ��ȯ�ϴ� �޼���
            SpawnMonster();
        }

        // �ܺ� ���� : ���� ��û ���� �������� �� ���� 
        CallStartEvent();

        GameState = GameStates.Playing;
    }

    private void OnEndStateChangedHandler()  // ���� ������Ʈ�� ���۵� ��
    {
        // ���� �������...
        if (stageData.currentStage < EndingStage && PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            OnGameOverEvent();
            Debug.Log("MainGameManger : OverPanel");
            return;
        }
        //InstantiatedPlayer.SetActive(false);
        // �������� ������ �� ��� �г� ���� �� �����ְ�,,

        // ���� ���� ���� �ľ�
        if (stageData.currentStage < EndingStage)
        {
            // ���� ������������ �÷��̾� ��Ȱ ��,
            //PartyDeathCount = 0;
            stageData.currentStage += 1;
            
            // ���⼭ �������� ��, �Ĺ� ���� ��, �̺�Ʈ ���� �� ����.
            stageData.isFarmingRoom = true;

            // ���� ���� ����
            GameState = GameStates.AugmentListing;

        }
        else
        {
            // ���� ��� �г� ����ְ�
            OnGameClearedEvent();
            Debug.Log("MainGameManger : ClearedPanel");

            // ���� ���� ������ ..

        }
    }

    private void OnAugmentListingStateChangedHandler() // ���� ���� ������Ʈ ���۽�
    {
        tier = UnityEngine.Random.Range(1, 4);
        Ready = 0;
        CallEndEvent();
        //GameState = GameStates.UIPlaying;
    }
    
    public void AllReady()  // ���� ���� ���� Ȯ��
    {
        if (Ready == PhotonNetwork.CurrentRoom.MaxPlayers) 
        {
            photonView.RPC("uiscene", RpcTarget.All);
        }
    }
    [PunRPC]
    public void uiscene()  // UI �÷��̽�
    {
        InstantiatedPlayer.SetActive(false);
        GameState = GameStates.UIPlaying;
    }
    #endregion


    private void SpawnPlayer() // �÷��̾� ��ȯ
    {
        // PlayerCharacterSetting 
        string characterSettingPath = "Prefabs/CharacterData/PlayerCharacterSetting";
        GameObject characterSettingGO = Instantiate(Resources.Load<GameObject>(characterSettingPath));
        characterSetting = characterSettingGO.GetComponent<PlayerDataSetting>();

        // PhotonNetwork.Instantiate()
        string playerPrefabPath = "Pefabs/Player";
        InstantiatedPlayer = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
        characterSetting.ownerPlayer = InstantiatedPlayer;
        int viewID = InstantiatedPlayer.GetPhotonView().ViewID;
        characterSetting.viewID = viewID;
        PartyViewIDList.Add(viewID);
        photonView.RPC("SendViewID", RpcTarget.Others, viewID);
        // isDie
        var playerStatHandler = InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        isDie = playerStatHandler.isDie;
        PartyDeathCount = 0;
        //playerStatHandler.OnDieEvent += DiedAfter; ��Ȱ�� ���� 
        // �÷��̾� ������ �߰�
        playerInfoDictionary.Add(viewID, InstantiatedPlayer.transform);
        GameObject sendingPlayer = InstantiatedPlayer;
        photonView.RPC("SendPlayerInfo", RpcTarget.Others, viewID);

        // ClassIdentifier ������ Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;
    }
    [PunRPC]
    public void SendPlayerInfo(int viewID) // playerInfoDictionary ����ȭ �� ä���
    {
        GameObject clientPlayer = PhotonView.Find(viewID).gameObject;
        playerInfoDictionary.Add(viewID, clientPlayer.transform);
        Debug.Log($"{playerInfoDictionary.Count}���� ��ųʸ��� ��ϵ�");
        int cnt = 0;
        foreach (var key in playerInfoDictionary.Keys)
        {
            cnt += 1;
            Debug.Log($"{playerInfoDictionary.Count}���� Ű �� {cnt}��° == {key}");
        }
    }

    [PunRPC]
    private void SendViewID(int viewID)
    {
        PartyViewIDList.Add(viewID);
    }

    private void SyncPlayer() // �÷��̾� ����ȭ
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            characterSetting.SetClassType((int)classNum, InstantiatedPlayer);
            int viewID = characterSetting.viewID;
            InstantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        }
    }

    private void SpawnMonster() // ���� ��ȯ
    {
        foreach (StageSO singleStageInfo in stageInfo.stageDict)
        {
            Debug.Log("�Ŵ����� �������� �̸�: "+GetStageName());
            Debug.Log("SO�� �������� �̸�: " + singleStageInfo.StageName);
            if (singleStageInfo.StageName == GetStageName())
            {
                var monsterDatas = singleStageInfo.MonsterDatas;
                var monsterStructList = monsterDatas.monsters;
                foreach (var monsterStruct in monsterStructList)
                {
                    var targetMonster = monsterStruct.monsterType;
                    var monsterCount = monsterStruct.monsterCount;
                    Debug.Log(targetMonster);
                    Debug.Log(monsterCount);
                    monsterDataList.Add(new MonsterData { monsterNum = monsterCount, monsterType = targetMonster });
                    currentMonsterCount += monsterCount;
                }

                GameObject go = PhotonNetwork.Instantiate("Prefabs/Enemy/SpawnPoint", transform.position, Quaternion.identity);        

                if (PhotonNetwork.IsMasterClient)
                {
                    for (int i = 0; i < currentMonsterCount; i++) 
                    {
                        float destinationX = Random.Range(-13f, 13f);
                        float destinationY = Random.Range(-13f, 13f);
                        go.transform.position = new Vector3(destinationX, destinationY, 0);

                        Debug.Log(currentMonsterCount);
                        EnemySpawn enemySpawn = go.GetComponent<EnemySpawn>();
                        enemySpawn.Spawn("Test_Enemy");
                        Destroy(go);
                    }
                }
            }
        }
    }

    public void DiedAfter() // �������
    {
        photonView.RPC("AddPartyDeathCount", RpcTarget.All);
        Debug.Log("MainGameManager : DiedAfter() => PartyDeath : " + PartyDeathCount.ToString());
        OnOverCheckEvent?.Invoke();
    }

    public void OverCheck() 
    {
        if (PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            photonView.RPC("SetEndState", RpcTarget.All);
        }
    }
    private string GetStageName()
    {
        return $"Stage_{stageData.currentArea}_{stageData.currentStage}";
    }

    [PunRPC]
    public void AddPartyDeathCount()
    {
        PartyDeathCount++;
        OnPlayerDieEvent?.Invoke();
    }
    [PunRPC]
    public void RemovePartyDeathCount()
    {
        PartyDeathCount--;
    }


    [PunRPC]
    public void SetEndState()
    {
        GameState = GameStates.End;
    }
}