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
    FadeInFadeOutPanel FF;

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

        MapGenerator MG = _mapGenerator.GetComponent<MapGenerator>();
        FF = _fadeInfadeOutPanel.GetComponent<FadeInFadeOutPanel>();


        OnStageStartEvent += MG.MapMake;
        OnStageStartEvent += FF.FadeIn;
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
        FF.FadeOut(1);
    }
    public void NextStageEndEvent()
    {
        OnStageEndEvent?.Invoke();
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
