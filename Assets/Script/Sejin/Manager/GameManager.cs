using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event Action OnInitEvent;          //�ʱ⼼��

    public event Action OnStageStartEvent;    //�������� ����
    public event Action OnRoomStartEvent;     //�� ����
    public event Action OnRoomEndEvent;       //�� ����
    public event Action OnStageEndEvent;      //�������� ����

    public event Action OnGameClearEvent;     //���� Ŭ����
    public event Action OnGameOverEvent;      //���� ����


    public GameObject _mapGenerator;


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

        OnInitEvent = GetComponent<PlayerSetting>().InstantiatePlayer;
        OnStageStartEvent = _mapGenerator.GetComponent<MapGenerator>().MapMake;
        CallInitEvent();
    }
    


    private void Start()
    {
        CallStageStartEvent();
    }

    private void Update()
    {

    }

    public void CallInitEvent()
    {
        OnInitEvent?.Invoke();
    }
    public void CallStageStartEvent()
    {
        OnStageStartEvent?.Invoke();
    }
    public void CallRoomStartEvent()
    {
        OnRoomStartEvent?.Invoke();
    }
    public void CallRoomEndEvent()
    {
        OnRoomEndEvent?.Invoke();
    }
    public void CallStageEndEvent()
    {
        OnStageEndEvent?.Invoke();
    }
    public void CallGameClearEvent()
    {
        OnGameClearEvent?.Invoke();
    }
    public void CallGameOverEvent()
    {
        OnGameOverEvent?.Invoke();
    }
}
