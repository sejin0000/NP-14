using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Rendering.FilterWindow;

public class UIPlayerMiniHUD : UIBase
{
    [SerializeField] private List<UIBase> elements;
    private GameObject player;
    private PlayerStatHandler statHandler;
    private TopDownCharacterController playerController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("[test] start.");
        if ((SceneManager.GetActiveScene().name == "Test_DoHyun") || (SceneManager.GetActiveScene().name == "MainGameScene"))
        {
            Debug.Log("[test] CheckCondition");
            InitializeData();
        }
        else
        {
            Debug.Log("[test] Fail.");
            return;
        }
    }

    public void InitializeData()
    {
        Debug.Log("[test] InitializeData");
        foreach (var element in elements)
        {
            element.Initialize();
            element.Close();
        }

        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.gameObject;
        else
            player = MainGameManager.Instance.InstantiatedPlayer.gameObject;

        playerController = player.GetComponent<TopDownCharacterController>();
        statHandler = player.GetComponent<PlayerStatHandler>();

        InitializeEvent();
    }

    public void InitializeEvent()
    {
        Debug.Log("[test] Initialize");
        statHandler.HitEvent += Open<UIPlayerHP>;
        playerController.OnReloadEvent += Open<UIReloadHUD>;
    }

    public void Open<T>() where T : UIBase
    {
        foreach (var element in elements)
        {
            Debug.Log("[test] " + element.GetType().Name + "==" + typeof(T).Name);
            if(element.GetType().Name == typeof(T).Name)
                element.Open();
            else
                element.Close();
        }
    }
}
