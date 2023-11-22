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
    public GameObject InstantiatedPlayer; // 클라이언트가 소환한 플레이어
    private bool isPlayerInstantiated;    // 클라이언트가 플레이어를 소환했는지 여부
    public Dictionary<int, Transform> playerInfoDictionary;   // 각 플레이어의 viewID를 키로 하고 Trasform을 value로 하는 딕셔너리

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting; // 플레이어의 정보
    public bool isDie; // 플레이어 죽음 여부
    public int Gold;  // 골드

    [HideInInspector]
    public List<int> PartyViewIDList; // 파티의 viewID 리스트
    public int PartyDeathCount; // 파티의 죽음 카운트


    [Header("GameData")]
    public int currentMonsterCount; // 현재 남은 몬스터수
    public List<MonsterData> monsterDataList; // SO에서 읽어온 몬스터 데이터 리스트
    [Space(10f)]
    public StageData stageData; // 스테이지 SO의 데이터
    public int EndingStage; // 엔드 스테이지
    [Space(10f)]
    private GameStates gameState; // 게임 스테이트
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
    public bool IsStateEnded; // 스테이트가 끝났는지 여부
    [Space(10f)]
    public StageDictSO stageInfo; // 스테이지 SO 정보


    [Serializable]
    public struct MonsterData // 몬스터 데이터
    {
        public int monsterNum;
        public string monsterType;
    }

    [Serializable]
    public struct StageData // 스테이지의 데이터
    {
        public int currentArea;
        public int currentStage;
        public bool isFarmingRoom;
        public bool isEventRoom;
        public bool isBossRoom;
    }

    [Header("UI")]
    public GameObject StageInfoUI; //스테이지 UI

    [Header("Enemy")]
    public GameObject Nav; // nav

    [HideInInspector]
    public event Action OnGameStartedEvent; // 게임 시작시 이벤트 (증강쪽)
    public event Action OnGameEndedEvent; // 게임 끝날시 이벤트 (증강쪽)
    public event Action OnGameClearedEvent; // 게임 클리어시 이벤트
    public event Action OnPlayerDieEvent; // 플레이어 사망시 이벤트
    public event Action OnGameOverEvent; // 게임 오버시 이벤트
    public event Action OnOverCheckEvent; // 다른 플레이어 사망시 현재 생존자를 세는 이벤트

    [HideInInspector]
    public event Action OnUIPlayingStateChanged; // UI쪽 플레이시 이벤트
    public event Action OnStartStateChanged; // 스타트 스테이트시 이벤트
    public event Action OnPlayingStateChanged; // 플레잉 스테이트시 이벤트
    public event Action OnEndStateChanged; // 끝날 시 이벤트
    public event Action OnAugmentListingStateChanged; // 보상 스테이트시 이벤트
     
    [HideInInspector]
    public int tier;  // 증강의 티어
    public int Ready; // 다른 사람들이 증강을 선택했는지 여부를 알기 위해



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
                Debug.Log("몬스터 다 죽임");
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

    private void OnStartStateChangedHandler()  // 스타트 스테이트로 시작될 때
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

    private void OnEndStateChangedHandler()  // 엔드 스테이트로 시작될 때
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

    private void OnAugmentListingStateChangedHandler() // 증강 선택 스테이트 시작시
    {
        tier = UnityEngine.Random.Range(1, 4);
        Ready = 0;
        CallEndEvent();
        //GameState = GameStates.UIPlaying;
    }
    
    public void AllReady()  // 증강 선택 여부 확인
    {
        if (Ready == PhotonNetwork.CurrentRoom.MaxPlayers) 
        {
            photonView.RPC("uiscene", RpcTarget.All);
        }
    }
    [PunRPC]
    public void uiscene()  // UI 플레이시
    {
        InstantiatedPlayer.SetActive(false);
        GameState = GameStates.UIPlaying;
    }
    #endregion


    private void SpawnPlayer() // 플레이어 소환
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
        //playerStatHandler.OnDieEvent += DiedAfter; 부활로 인해 
        // 플레이어 데이터 추가
        playerInfoDictionary.Add(viewID, InstantiatedPlayer.transform);
        GameObject sendingPlayer = InstantiatedPlayer;
        photonView.RPC("SendPlayerInfo", RpcTarget.Others, viewID);

        // ClassIdentifier 데이터 Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;
    }
    [PunRPC]
    public void SendPlayerInfo(int viewID) // playerInfoDictionary 동기화 및 채우기
    {
        GameObject clientPlayer = PhotonView.Find(viewID).gameObject;
        playerInfoDictionary.Add(viewID, clientPlayer.transform);
        Debug.Log($"{playerInfoDictionary.Count}개가 딕셔너리에 등록됨");
        int cnt = 0;
        foreach (var key in playerInfoDictionary.Keys)
        {
            cnt += 1;
            Debug.Log($"{playerInfoDictionary.Count}개의 키 중 {cnt}번째 == {key}");
        }
    }

    [PunRPC]
    private void SendViewID(int viewID)
    {
        PartyViewIDList.Add(viewID);
    }

    private void SyncPlayer() // 플레이어 동기화
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            characterSetting.SetClassType((int)classNum, InstantiatedPlayer);
            int viewID = characterSetting.viewID;
            InstantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        }
    }

    private void SpawnMonster() // 몬스터 소환
    {
        foreach (StageSO singleStageInfo in stageInfo.stageDict)
        {
            Debug.Log("매니저의 스테이지 이름: "+GetStageName());
            Debug.Log("SO의 스테이지 이름: " + singleStageInfo.StageName);
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

    public void DiedAfter() // 사망이후
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