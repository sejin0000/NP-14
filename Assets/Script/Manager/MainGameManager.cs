using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
    public GameObject InstantiatedPlayer;
    private bool isPlayerInstantiated;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;
    public bool isDie;
    [HideInInspector]
    public List<int> PartyViewIDList;
    public int PartyDeathCount;

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
        ; }
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
    public event Action OnGameClearedEvent;
    public event Action OnGameOverEvent;
    public event Action OnOverCheckEvent;

    [HideInInspector]
    public event Action OnUIPlayingStateChanged;
    public event Action OnStartStateChanged;
    public event Action OnPlayingStateChanged;
    public event Action OnEndStateChanged;
    public event Action OnAugmentListingStateChanged;

    [HideInInspector]
    public int tier;
    public int Ready;
    public bool IsCleared;
    public bool IsOvered;



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
            //이아래 2줄 박민혁이넣음 게임시작시 증강뽑는거에 플레이어값 전달임
            PlayerResultController MakeSetting = InstantiatedPlayer.GetComponent<PlayerResultController>();
            MakeSetting.MakeManager();
            SyncPlayer();
        }
        else
        {
            InstantiatedPlayer.SetActive(true);
        }

        //네브 생성 임시-우민규
        Nav = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy/NavMesh2D"));


        // 임시로 EndingStage 는 3까지
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

    private void OnStartStateChangedHandler()
    {        
        InstantiatedPlayer.SetActive(true);

        if (stageData.isFarmingRoom)
        {
            // 대충 맵 가져다 놓는 메서드

            // 대충 소환하는 메서드
            SpawnMonster();
        }

        // 외부 시작 : 민혁 요청 게임 스테이지 의 시작 
        CallStartEvent();

        GameState = GameStates.Playing;
    }

    private void OnEndStateChangedHandler()
    {
        // 게임 오버라면...
        if (stageData.currentStage < EndingStage && PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            OnGameOverEvent();
            Debug.Log("MainGameManger : OverPanel");
            return;
        }
        //InstantiatedPlayer.SetActive(false);
        // 스테이지 끝났을 때 결과 패널 같은 거 보여주고,,

        // 게임 엔딩 여부 파악
        if (stageData.currentStage < EndingStage)
        {
            // 다음 스테이지에서 플레이어 부활 시,
            //PartyDeathCount = 0;
            stageData.currentStage += 1;
            
            // 여기서 랜덤방일 지, 파밍 방일 지, 이벤트 방일 지 결정.
            stageData.isFarmingRoom = true;

            // 다음 상태 진입
            GameState = GameStates.AugmentListing;

        }
        else
        {
            // 게임 결과 패널 띄워주고
            OnGameClearedEvent();
            Debug.Log("MainGameManger : ClearedPanel");

            // 게임 엔딩 씬으로 ..

        }
    }

    private void OnAugmentListingStateChangedHandler()
    {
        tier = UnityEngine.Random.Range(1, 4);
        Ready = 0;
        CallEndEvent();
        //GameState = GameStates.UIPlaying;
    }
    
    public void AllReady() 
    {
        if (Ready == PhotonNetwork.CurrentRoom.MaxPlayers) 
        {
            photonView.RPC("uiscene", RpcTarget.All);
        }
    }
    [PunRPC]
    public void uiscene() 
    {
        InstantiatedPlayer.SetActive(false);
        GameState = GameStates.UIPlaying;
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
        int viewID = InstantiatedPlayer.GetPhotonView().ViewID;
        characterSetting.viewID = viewID;
        PartyViewIDList.Add(viewID);
        photonView.RPC("SendViewID", RpcTarget.Others, viewID);
        // isDie
        var playerStatHandler = InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        isDie = playerStatHandler.isDie;
        PartyDeathCount = 0;
        playerStatHandler.OnDieEvent += DiedAfter;


        // ClassIdentifier 데이터 Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;
    }

    [PunRPC]
    private void SendViewID(int viewID)
    {
        PartyViewIDList.Add(viewID);
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
            Debug.Log("매니저의 스테이지 이름: "+GetStageName());
            Debug.Log("SO의 스테이지 이름: "+singleStageInfo.StageName);
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

                    // 해당 타입의 몬스터를 monsterCount 만큼 반복해서 spawn할 것, 
                }
                // SpawnPoint 인스턴스화

                

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
                        enemySpawn.Spawn();
                    }
                }
            }
        }
    }

    public void DiedAfter()
    {
        PartyDeathCount++;
        OnOverCheckEvent?.Invoke();
    }

    public void OverCheck()
    {
        if (PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            GameState = GameStates.End;
        }
    }
    private string GetStageName()
    {
        return $"Stage_{stageData.currentArea}_{stageData.currentStage}";
    }
}