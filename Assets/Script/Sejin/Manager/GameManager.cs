using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PhotonView PV;

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


    public GameObject _mapGenerator;
    public MapGenerator MG;
    public GameObject _fadeInfadeOutPanel;
    private FadeInFadeOutPanel FF;

    public static GameManager Instance;


    public StageDictSO StageInfo;
    private int curStage;

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

        OnInitEvent += GetComponent<PlayerSetting>().InstantiatePlayer;

        MG = _mapGenerator.GetComponent<MapGenerator>();
        FF = _fadeInfadeOutPanel.GetComponent<FadeInFadeOutPanel>();


        OnStageStartEvent += MG.MapMake;
        CallInitEvent();
    }
    


    private void Start()
    {
        CallStageStartEvent();
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
        OnRoomStartEvent?.Invoke();
    }
    public void CallRoomEndEvent()
    {
        Debug.Log("�� ����");
        OnRoomEndEvent?.Invoke();
    }
    public void CallStageEndEvent()
    {
        Debug.Log("�������� ����");
        FF.FadeOut(1);
    }
    public void NextStageEndEvent()
    {
        OnStageEndEvent?.Invoke();
        PV.RPC("EndPlayerCheck", RpcTarget.AllBuffered);
    }

    int EndPlayer = 0;
    [PunRPC]
    public void EndPlayerCheck()
    {
        EndPlayer++;
        if (EndPlayer == 2)
        {
            StageClear();
            EndPlayer = 0;
        }
    }

    public void StageClear()
    {
        CallStageStartEvent();
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
