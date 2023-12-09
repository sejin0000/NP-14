using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PhotonView PV;

    public event Action OnInitEvent;          //초기세팅

    public event Action OnStageSettingEvent;  //스테이지 세팅
    public event Action OnStageStartEvent;    //스테이지 시작
    public event Action OnRoomStartEvent;     //룸 시작
    public event Action OnRoomEndEvent;       //룸 종료
    public event Action OnStageEndEvent;      //스테이지 종료


    public event Action OnBossStageSettingEvent; //보스 룸 시작
    public event Action OnBossStageStartEvent; //보스 룸 시작
    public event Action OnBossStageEndEvent;   //보스 룸 종료

    public event Action OnGameClearEvent;     //게임 클리어
    public event Action OnGameOverEvent;      //게임 오버
    public event Action PlayerLifeCheckEvent; //플레이어 죽음

    public event Action ChangeGoldEvent;
    public bool ClearStageCheck;//박민혁 추가 스테이지 클리어시 빈방 비울때 콜여부

    public StageListInfoSO stageListInfo;
    public int curStage = 0;


    public GameObject _mapGenerator;
    public MapGenerator MG;

    public GameObject _fadeInfadeOutPanel;
    private FadeInFadeOutPanel FF;

    public GameObject _mansterSpawner;
    private MonsterSpawner MS;

    public Setting PS;

    public static GameManager Instance;


    public GameObject clientPlayer;
    public Dictionary<int, Transform> playerInfoDictionary;

    public int PartyDeathCount;
    public int TeamGold;
    public bool isTransitionPlayed;

    private bool firstStart;


    private void Awake()
    {
        firstStart = true;
        Debug.Log("GameManager - Awake()");

        if (Instance == null)
        {
            Instance = this;
        }
        ClearStageCheck = false;

        PV = GetComponent<PhotonView>();

        playerInfoDictionary = new Dictionary<int, Transform>();

        PS = GetComponent<Setting>();

        OnInitEvent += PS.InstantiatePlayer;

        CallInitEvent();

        MG = _mapGenerator.GetComponent<MapGenerator>();
        FF = _fadeInfadeOutPanel.GetComponent<FadeInFadeOutPanel>();
        MS = _mansterSpawner.GetComponent<MonsterSpawner>();

        OnStageSettingEvent += MG.MapMake;
        MG.roomNodeInfo = MG.GetComponent<RoomNodeInfo>();
        OnStageSettingEvent += MG.roomNodeInfo.CloseDoor;
        OnBossStageSettingEvent += MG.BossMapMake;
        if (PhotonNetwork.IsMasterClient)
        {
            OnStageSettingEvent += MG.NavMeshBakeRunTime;
            OnStageStartEvent += MS.MonsterSpawn;
            OnBossStageStartEvent += MS.BossSpawn;
        }
        OnStageStartEvent += MG.roomNodeInfo.OpenDoor;

    }
    private void Start()
    {
        AudioManager.Instance.AddComponent<AudioManagerTest>().Initialize();
        if (firstStart)
        {
            PlayerResultController MakeSetting = clientPlayer.GetComponent<PlayerResultController>();
            MakeSetting.MakeManager();
            TeamGold = 0;
        }

        if (stageListInfo.StagerList[curStage].stageType == StageType.normalStage)
        {
            CallStageSettingEvent();
        }
        else if (stageListInfo.StagerList[curStage].stageType == StageType.bossStage)
        {
            CallBossStageSettingEvent();
        }
    }

    public void CallInitEvent()
    {
        Debug.Log("초기화");
        OnInitEvent?.Invoke();
    }

    public void CallStageSettingEvent()
    {
        Debug.Log("파밍 스테이지 세팅");
        PartyDeathCount = 0;
        ClearStageCheck = false;
        OnStageSettingEvent?.Invoke();
        FF.FadeIn();
        StartCoroutine(WaitTransition());
    }

    public void CallStageStartEvent()
    {
        Debug.Log("스테이지 시작");
        OnStageStartEvent?.Invoke();
        if (PhotonNetwork.IsMasterClient)
        {
            PV.RPC("PunReadyCheck", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void PunReadyCheck()
    {
        FF.FadeIn();
    }


    public void CallRoomStartEvent()
    {
        Debug.Log("룸 시작");
        PV.RPC("PunCallRoomStartEvent",RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void PunCallRoomStartEvent()
    {
        OnRoomStartEvent?.Invoke();
    }
    [PunRPC]
    public void CallRoomEndEvent()
    {
        Debug.Log("룸 종료");
        if (!ClearStageCheck)
        {
            PV.RPC("PunCallRoomEndEvent", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void PunCallRoomEndEvent()
    {
        OnRoomEndEvent?.Invoke();
    }

    public void CallStageEndEvent()
    {
        Debug.Log("스테이지 종료");
        PV.RPC("PunCallStageEndEvent",RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void PunCallStageEndEvent()
    {
        FF.FadeOut(1);
    }

    public void NextStageEndEvent()
    {
        Debug.Log("OnStageEndEvent");
        ClearStageCheck = true;
        OnStageEndEvent?.Invoke();
        
        //PV.RPC("EndPlayerCheck", RpcTarget.AllBuffered);
    }

    int EndPlayer = 0;
    [PunRPC]
    public void EndPlayerCheck()
    {
        EndPlayer++;
        //if (EndPlayer == 1)//?????
        //{
        //    StageClear();
        //    EndPlayer = 0;
        //}
        Debug.Log($"현재 레디 : {EndPlayer} 필요 레디 : {PhotonNetwork.CurrentRoom.PlayerCount}");
        if (EndPlayer == PhotonNetwork.CurrentRoom.PlayerCount) 
        {
            StageClear();
            EndPlayer = 0;
        }
    }

    public void StageClear()
    {
        if (stageListInfo.StagerList.Count > curStage + 1)
        {
            curStage++;
            Start();
        }
        else
        {
            CallGameClearEvent();
        }
    }

    public void CallBossStageSettingEvent()
    {
        Debug.Log("보스 스테이지 세팅");
        OnBossStageSettingEvent?.Invoke();
        FF.FadeIn();
        StartCoroutine(WaitTransition());
    }

    public void CallBossStageStartEvent()
    {
        Debug.Log("보스 스테이지 시작");
        OnBossStageStartEvent?.Invoke();
    }

    public void CallBossStageEndEvent()
    {
        Debug.Log("보스 스테이지 종료");
        OnBossStageEndEvent?.Invoke();
    }

    public void CallGameClearEvent()
    {
        Debug.Log("게임 클리어");
        FF.FadeOut(2);
    }
    public void NextGameClearEvent()
    {
        OnGameClearEvent?.Invoke();
    }

    public void CallGameOverEvent()//맵지워지는 시간 벌기
    {
        Debug.Log("게임 오버");
        FF.FadeOut(3);
    }
    public void NextGameOverEvent()
    {
        OnGameOverEvent?.Invoke();
    }

    public void PlayerDie()
    {
        PV.RPC("AddPartyDeathCount", RpcTarget.All);
        Debug.Log("현재 죽은수 PartyDeath : " + PartyDeathCount.ToString());
    }

    [PunRPC]
    public void AddPartyDeathCount()
    {
        PartyDeathCount++;
        CallPlayerLifeCheckEvent();
        if (PartyDeathCount == PhotonNetwork.CurrentRoom.PlayerCount) 
        {
            CallGameOverEvent();
        }
    }
    [PunRPC]
    public void RemovePartyDeathCount()
    {
        CallPlayerLifeCheckEvent();
        PartyDeathCount--;
        Debug.Log("현재 죽은수 PartyDeath : " + PartyDeathCount.ToString());
    }
    public void CallPlayerLifeCheckEvent()
    {
        PlayerLifeCheckEvent?.Invoke();
    }
    [PunRPC]
    public void ChangeGold(int i)
    {
        CallChangeGoldEvent();
        TeamGold += i;
    }
    public void CallChangeGoldEvent()
    {
        ChangeGoldEvent?.Invoke();
    }

    private IEnumerator WaitTransition()
    {
        if (!isTransitionPlayed)
        {
            Debug.Log("GameManager [WaitTransition] - Waiting");
            yield return new WaitForSeconds(1f);
            StartCoroutine(WaitTransition());
        }
        else
        {
            Debug.Log("GameManager[WaitTransition] - end");
            isTransitionPlayed = false;
            if (stageListInfo.StagerList[curStage].stageType == StageType.normalStage)
            {
                CallStageStartEvent();
            }
            else if (stageListInfo.StagerList[curStage].stageType == StageType.bossStage)
            {
                CallBossStageStartEvent();
            }
            yield return null;
        }
    }
}
