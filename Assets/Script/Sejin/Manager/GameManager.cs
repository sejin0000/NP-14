using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PhotonView PV;

    public event Action OnInitEvent;          //�ʱ⼼��

    public event Action OnStageStartEvent;    //�������� ����
    public event Action OnRoomStartEvent;     //�� ����
    public event Action OnRoomEndEvent;       //�� ����
    public event Action OnStageEndEvent;      //�������� ����


    public event Action OnBossStageSettingEvent; //���� �� ����
    public event Action OnBossStageStartEvent; //���� �� ����
    public event Action OnBossStageEndEvent;   //���� �� ����



    public event Action OnGameClearEvent;     //���� Ŭ����
    public event Action OnGameOverEvent;      //���� ����




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
        Debug.Log("�ʱ�ȭ");
        OnInitEvent?.Invoke();
    }
    public void CallStageStartEvent()
    {
        Debug.Log("�������� ����");
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
        Debug.Log("�� ����");
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
        Debug.Log("�� ����");
        PV.RPC("PunCallRoomEndEvent", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void PunCallRoomEndEvent()
    {
        OnRoomEndEvent?.Invoke();
    }

    public void CallStageEndEvent()
    {
        Debug.Log("�������� ����");
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
        Debug.Log($"���� ���� : {EndPlayer} �ʿ� ���� : {PhotonNetwork.CurrentRoom.PlayerCount}");
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
        Debug.Log("���� �������� ����");
        OnBossStageSettingEvent?.Invoke();
    }

    public void CallBossStageStartEvent()
    {
        Debug.Log("���� �������� ����");
        OnBossStageStartEvent?.Invoke();
    }

    public void CallBossStageEndEvent()
    {
        Debug.Log("���� �������� ����");
        OnBossStageEndEvent?.Invoke();
    }

    public void CallGameClearEvent()
    {
        Debug.Log("���� Ŭ����");
        FF.FadeOut(2);
    }
    public void NextGameClearEvent()
    {
        OnGameClearEvent?.Invoke();
    }

    public void CallGameOverEvent()
    {
        Debug.Log("���� ����");
        FF.FadeOut(3);
    }
    public void NextGameOverEvent()
    {
        OnGameOverEvent?.Invoke();
    }


}
