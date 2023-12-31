using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPlayerMiniHUD : UIBase
{
    [SerializeField] private List<GameObject> elements;
    [SerializeField] private GameObject player;
    private PlayerStatHandler statHandler;
    private TopDownCharacterController playerController;

    public GameObject Player
    {
        get { return player; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if ((SceneManager.GetActiveScene().name == "Test_DoHyun") || (SceneManager.GetActiveScene().name == "MainGameScene"))
            InitializeData();
        else
            return;
    }

    public void InitializeData()
    {

        foreach (var element in elements)
        {
            var temp = element.GetComponent<UIBase>();
            if (temp != null)
            {
                Debug.Log("[CheckInterface] Init " + element.GetType());
                temp.Initialize();
            }
        }

        playerController = player.GetComponent<TopDownCharacterController>();
        statHandler = player.GetComponent<PlayerStatHandler>();

        OpenChild<UIPlayerMiniHP>();
        InitializeEvent();
    }

    public void InitializeEvent()
    {
        statHandler.HitEvent += OpenChild<UIPlayerMiniHP>;
        playerController.OnReloadEvent += OpenChild<UIReloadHUD>;
        playerController.OnEndReloadEvent += OpenChild<UIPlayerMiniHP>;
    }

    public void OpenChild<T>() where T : UIBase
    {
        var new_elements = player.GetComponentInChildren<UIPlayerMiniHUD>().elements;
        foreach (var element in new_elements)
        {
            //Debug.Log("[CheckInterface] " + element.GetType().Name + "/" + element.GetComponentInChildren<T>().name);
            T temp = element.GetComponent<T>();
            if (temp == null)
                element.SetActive(false);
            else
                temp.Open();
        }
    }
}
