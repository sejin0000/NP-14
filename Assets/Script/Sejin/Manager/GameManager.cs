using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PhotonView PV;

    public event Action OnInitEvent;          //초기세팅

    public event Action OnStageStartEvent;    //스테이지 시작
    public event Action OnRoomStartEvent;     //룸 시작
    public event Action OnRoomEndEvent;       //룸 종료
    public event Action OnStageEndEvent;      //스테이지 종료


    public event Action OnBossStageSettingEvent; //보스 룸 시작
    public event Action OnBossStageStartEvent; //보스 룸 시작
    public event Action OnBossStageEndEvent;   //보스 룸 종료



    public event Action OnGameClearEvent;     //게임 클리어
    public event Action OnGameOverEvent;      //게임 오버




    public StagerListInfoSO stageListInfo;
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



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        PV = GetComponent<PhotonView>();

        playerInfoDictionary = new Dictionary<int, Transform>();

        PS = GetComponent<Setting>();

        OnInitEvent += PS.InstantiatePlayer;

        MG = _mapGenerator.GetComponent<MapGenerator>();
        FF = _fadeInfadeOutPanel.GetComponent<FadeInFadeOutPanel>();
        MS = _mansterSpawner.GetComponent<MonsterSpawner>();

        OnStageStartEvent += MG.MapMake;
        OnStageStartEvent += MG.roomNodeInfo.CloseDoor;
        if (PhotonNetwork.IsMasterClient)
        {
            OnStageStartEvent += MG.NavMeshBakeRunTime;
            OnStageStartEvent += MS.MonsterSpawn;           
        }
        OnStageStartEvent += MG.roomNodeInfo.OpenDoor;

        CallInitEvent();
        PlayerResultController MakeSetting = clientPlayer.GetComponent<PlayerResultController>();
        MakeSetting.MakeManager();
    }
    


    private void Start()
    {
        if (stageListInfo.StagerList[curStage].stageType == StageType.normalStage)
        {
            CallStageStartEvent();
        }
        else if(stageListInfo.StagerList[curStage].stageType == StageType.bossStage)
        {

        }
    }


    public void CallInitEvent()
    {
        Debug.Log("초기화");
        OnInitEvent?.Invoke();
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
        PV.RPC("PunCallRoomEndEvent", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void PunCallRoomEndEvent()
    {
        OnRoomEndEvent?.Invoke();
    }

    public void CallStageEndEvent()
    {
        Debug.Log("스테이지 종료");
        FF.FadeOut(1);
    }
    public void NextStageEndEvent()
    {
        Debug.Log("OnStageEndEvent");
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
        if (stageListInfo.StagerList.Count >= curStage)
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

    public void CallGameOverEvent()
    {
        Debug.Log("게임 오버");
        FF.FadeOut(3);
    }
    public void NextGameOverEvent()
    {
        OnGameOverEvent?.Invoke();
    }


}
