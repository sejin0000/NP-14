using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public GameObject InstantiatedPlayer;
    private bool isPlayerInstantiated;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;

    [Header("GameData")]
    public int currentMonsterCount;
    public List<MonsterData> monsterDataList;
    [Space(10f)]
    public StageData stageData;
    public int EndingStage;
    [Space(10f)]
    private GameStates gameState;
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
        ;}
    }
    public bool IsStateEnded;
    [Space(10f)]
    public StageDictSO stageInfo;


    [Serializable]
    public struct MonsterData
    {
        public int monsterNum;
        public string monsterType;
    }

    [Serializable]
    public struct StageData
    {
        public int currentArea;
        public int currentStage;
        public bool isFarmingRoom;
        public bool isEventRoom;
        public bool isBossRoom;
    }

    [Header("UI")]
    public GameObject StageInfoUI;

    [Header("Enemy")]
    public GameObject Nav;

    [HideInInspector]
    public event Action OnGameStartedEvent;
    public event Action OnGameEndedEvent;

    [HideInInspector]
    public event Action OnUIPlayingStateChanged;
    public event Action OnStartStateChanged;
    public event Action OnPlayingStateChanged;
    public event Action OnEndStateChanged;
    public event Action OnAugmentListingStateChanged;

    [HideInInspector]
    public int tier;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        //
        GameState = GameStates.Init;
        IsStateEnded = false;

        stageData = new StageData
        {
            currentArea = 1,
            currentStage = 1,
            isFarmingRoom = true,
        };

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
                GameState = GameStates.End;
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
                break;
            case GameStates.Start:
                OnStartStateChanged?.Invoke();                
                break;
            case GameStates.Playing:                
                OnPlayingStateChanged?.Invoke();                
                break;
            case GameStates.End:
                OnEndStateChanged?.Invoke();                
                break;
            case GameStates.AugmentListing:                
                OnAugmentListingStateChanged?.Invoke();                
                break;
        }
    }

    private void OnStartStateChangedHandler()
    {
        Debug.Log("Start state ����");
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

    private void OnEndStateChangedHandler()
    {
        //
        InstantiatedPlayer.SetActive(false);
        // �������� ������ �� ��� �г� ���� �� �����ְ�,,

        // ���� ���� ���� �ľ�
        if (stageData.currentStage < EndingStage)
        {
            stageData.currentStage += 1;

            // ���⼭ �������� ��, �Ĺ� ���� ��, �̺�Ʈ ���� �� ����.
            stageData.isFarmingRoom = true;

            // ���� ���� ����
            GameState = GameStates.AugmentListing;
            
        }
        else
        {
            // ���� ��� �г� ����ְ�


            // ���� ���� ������ ..

        }
    }

    private void OnAugmentListingStateChangedHandler()
    {
        tier = UnityEngine.Random.Range(1, 4);
        CallEndEvent();
        //GameState = GameStates.UIPlaying;
    }

    #endregion


    private void SpawnPlayer()
    {
        // PlayerCharacterSetting 
        string characterSettingPath = "Prefabs/CharacterData/PlayerCharacterSetting";
        GameObject characterSettingGO = Instantiate(Resources.Load<GameObject>(characterSettingPath));
        characterSetting = characterSettingGO.GetComponent<PlayerDataSetting>();

        // PhotonNetwork.Instantiate()
        string playerPrefabPath = "Pefabs/Player";
        InstantiatedPlayer = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
        characterSetting.ownerPlayer = InstantiatedPlayer;
        characterSetting.viewID = InstantiatedPlayer.GetPhotonView().ViewID;

        // ClassIdentifier ������ Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;
    }

    private void SyncPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            characterSetting.SetClassType((int)classNum, InstantiatedPlayer);
            int viewID = characterSetting.viewID;
            InstantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        }
    }

    private void SpawnMonster()
    {
        foreach (StageSO singleStageInfo in stageInfo.stageDict)
        {
            Debug.Log("�Ŵ����� �������� �̸�: "+GetStageName());
            Debug.Log("SO�� �������� �̸�: "+singleStageInfo.StageName);
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

                    // �ش� Ÿ���� ���͸� monsterCount ��ŭ �ݺ��ؼ� spawn�� ��, 
                }
                // SpawnPoint �ν��Ͻ�ȭ
                GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/SpawnPoint"));
                go.transform.position = new Vector3(5, 5, 0);

                for (int i = 0; i < currentMonsterCount; i++) 
                {
                    Debug.Log(currentMonsterCount);
                    EnemySpawn enemySpawn = go.GetComponent<EnemySpawn>();
                    enemySpawn.Spawn("����1");
                }
            }
        }
    }

    private string GetStageName()
    {
        return $"Stage_{stageData.currentArea}_{stageData.currentStage}";
    }
}