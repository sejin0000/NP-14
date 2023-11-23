using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnInitEvent;          //초기세팅

    public event Action OnStageStartEvent;    //스테이지 시작
    public event Action OnRoomStartEvent;     //룸 시작
    public event Action OnRoomEndEvent;       //룸 종료
    public event Action OnStageEndEvent;      //스테이지 종료

    public event Action OnGameClearEvent;     //게임 클리어
    public event Action OnGameOverEvent;      //게임 오버


    public GameObject _mapGenerator;
    public GameObject _fadeInfadeOutPanel;

    public static GameManager Instance;


    public GameObject clientPlayer;
    public Dictionary<int, Transform> playerInfoDictionary;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        playerInfoDictionary = new Dictionary<int, Transform>();

        OnInitEvent += GetComponent<PlayerSetting>().InstantiatePlayer;
        OnStageStartEvent += _mapGenerator.GetComponent<MapGenerator>().MapMake;

        CallInitEvent();
    }
    


    private void Start()
    {
        OnStageStartEvent += _fadeInfadeOutPanel.GetComponent<FadeInFadeOutPanel>().FadeIn;

        CallStageStartEvent();
    }

    private void Update()
    {

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
    }
    public void CallRoomStartEvent()
    {
        Debug.Log("룸 시작");
        OnRoomStartEvent?.Invoke();
    }
    public void CallRoomEndEvent()
    {
        Debug.Log("룸 종료");
        OnRoomEndEvent?.Invoke();
    }
    public void CallStageEndEvent()
    {
        Debug.Log("스테이지 종료");
        OnStageEndEvent?.Invoke();
    }
    public void CallGameClearEvent()
    {
        Debug.Log("게임 클리어");
        OnGameClearEvent?.Invoke();
    }
    public void CallGameOverEvent()
    {
        Debug.Log("게임 오버");
        OnGameOverEvent?.Invoke();
    }
}
