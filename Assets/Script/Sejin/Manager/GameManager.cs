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
        Debug.Log("�ʱ�ȭ");
        OnInitEvent?.Invoke();
    }
    public void CallStageStartEvent()
    {
        Debug.Log("�������� ����");
        OnStageStartEvent?.Invoke();
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
        OnStageEndEvent?.Invoke();
    }
    public void CallGameClearEvent()
    {
        Debug.Log("���� Ŭ����");
        OnGameClearEvent?.Invoke();
    }
    public void CallGameOverEvent()
    {
        Debug.Log("���� ����");
        OnGameOverEvent?.Invoke();
    }
}
