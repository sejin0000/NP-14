using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class UIPlayerMiniHUD : MonoBehaviour
{
    [SerializeField] private List<GameObject> elements;
    private GameObject player;
    private PlayerStatHandler statHandler;
    private TopDownCharacterController playerController;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var element in elements)
        {
            element.SetActive(false);
        }

        player = MainGameManager.Instance.InstantiatedPlayer.gameObject;
        playerController = player.GetComponent<TopDownCharacterController>();
        statHandler = player.GetComponent<PlayerStatHandler>();
        Initialize();
    }

    public void Initialize()
    {
        statHandler.HitEvent += Open<UIPlayerHP>;
        playerController.OnReloadEvent += Open<UIReloadHUD>;
    }

    public void Open<T>() where T : MonoBehaviour
    {
        foreach(var element in elements)
        {
            var temp = element.GetComponent<T>();

            if (temp == null)
                return;

            if (elements.GetType().Name == typeof(T).Name)
                element.SetActive(true);
            else
                element.SetActive(false);
        }
    }
}
