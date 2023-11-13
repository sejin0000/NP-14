using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIReloadHUD : MonoBehaviour
{
    private Slider slider;
    private GameObject player;
    private CoolTimeController controller;
    private PlayerStatHandler statHandler;

    private void Awake()
    {
        slider = GetComponentInChildren<Slider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Test_ DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.gameObject;
        else
            player = MainGameManager.Instance.InstantiatedPlayer.gameObject;

        controller = player.GetComponent<CoolTimeController>();
        statHandler = player.GetComponent<PlayerStatHandler>();
        player.GetComponent<TopDownCharacterController>().OnEndReloadEvent += Close;

        Initialize();
    }

    void Initialize()
    {
        slider.maxValue = statHandler.ReloadCoolTime.total;
        slider.value = controller.curReloadCool;
    }

    private void OnEnable()
    {
        if(player != null)
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
