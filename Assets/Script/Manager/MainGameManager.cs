using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    public static MainGameManager Instance;
    public enum GameStates
    {
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



    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        //
        GameState = GameStates.UIPlaying;
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
            SyncPlayer();
        }
        else
        {
            InstantiatedPlayer.SetActive(true);
        }


        // 임시로 EndingStage 는 3까지
        EndingStage = 3;

        OnStartStateChanged += OnStartStateChangedHandler;
        OnEndStateChanged += OnEndStateChangedHandler;
        OnAugmentListingStateChanged += OnAugmentListingStateChangedHandler;
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
    {;


        if (stageData.isFarmingRoom)
        {
            // 대충 맵 가져다 놓는 메서드

            // 대충 소환하는 메서드
            SpawnMonster();
        }

        // 외부 시작 : 민혁 요청 
        CallStartEvent();

        GameState = GameStates.Playing;        
    }

    private void OnEndStateChangedHandler()
    {
        //
        InstantiatedPlayer.SetActive(false);
        // 스테이지 끝났을 때 결과 패널 같은 거 보여주고,,

        // 게임 엔딩 여부 파악
        if (stageData.currentStage < EndingStage)
        {
            stageData.currentStage += 1;

            // 여기서 랜덤방일 지, 파밍 방일 지, 이벤트 방일 지 결정.
            stageData.isFarmingRoom = true;

            // 다음 상태 진입
            GameState = GameStates.AugmentListing;
            
        }
        else
        {
            // 게임 결과 패널 띄워주고


            // 게임 엔딩 씬으로 ..

        }
    }

    private void OnAugmentListingStateChangedHandler()
    {
        tier = UnityEngine.Random.Range(1, 4);
        CallEndEvent();
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
        characterSetting.viewID = InstantiatedPlayer.GetPhotonView().ViewID;

        // ClassIdentifier 데이터 Init()
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
            if (singleStageInfo.StageName == GetStageName())
            {
                var monsterDatas = singleStageInfo.MonsterDatas;
                var monsterStructList = monsterDatas.monsters;
                foreach (var monsterStruct in monsterStructList)
                {
                    var targetMonster = monsterStruct.monsterType;
                    var monsterCount = monsterStruct.monsterCount;

                    monsterDataList.Add(new MonsterData { monsterNum = monsterCount, monsterType = targetMonster });
                    currentMonsterCount += monsterCount;

                    // 해당 타입의 몬스터를 monsterCount 만큼 반복해서 spawn할 것, 
                    
                }
            }
        }
    }

    private string GetStageName()
    {
        return $"Stage_{stageData.currentArea}_{stageData.currentStage}";
    }
}