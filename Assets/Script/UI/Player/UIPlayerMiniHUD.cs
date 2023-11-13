using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPlayerMiniHUD : UIBase
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

        if(SceneManager.GetActiveScene().name == "Test_ DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.gameObject;
        else
            player = MainGameManager.Instance.InstantiatedPlayer.gameObject;

        playerController = player.GetComponent<TopDownCharacterController>();
        statHandler = player.GetComponent<PlayerStatHandler>();
        Initialize();
    }

    public override void Initialize()
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
                element.SetActive(false);
            else
                element.SetActive(true);
        }
    }
}
