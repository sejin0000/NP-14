using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReloadHUD : MonoBehaviour
{
    private Slider slider;
    private GameObject player;
    private CoolTimeController controller;
    private PlayerStatHandler statHandler;

    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();

        if (MainGameManager.Instance != null)
        {
            player = MainGameManager.Instance.InstantiatedPlayer;
            controller = player.GetComponent<CoolTimeController>();
            statHandler = player.GetComponent<PlayerStatHandler>();
            player.GetComponent<TopDownCharacterController>().OnEndRollEvent += Close;
            Initialize();
        }
    }

    void Initialize()
    {
        slider.maxValue = statHandler.ReloadCoolTime.total;
        slider.value = controller.curReloadCool;
    }

    private void OnEnable()
    {
        Initialize();
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = controller.curReloadCool;
    }
}
