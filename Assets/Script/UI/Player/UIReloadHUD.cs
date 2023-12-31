using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIReloadHUD : UIBase
{
    private Slider slider;
    private GameObject player;
    private CoolTimeController controller;
    private PlayerStatHandler statHandler;

    public override void Initialize()
    {
        InitializeData();
        UpdateData();
        Close();
    }

    public void InitializeData()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.gameObject;
        else
            player = GameManager.Instance.clientPlayer.gameObject;

        controller = player.GetComponent<CoolTimeController>();
        statHandler = player.GetComponent<PlayerStatHandler>();
        //player.GetComponent<TopDownCharacterController>().OnEndReloadEvent += Close;
        slider = GetComponentInChildren<Slider>();
    }

    public void UpdateData()
    {
        slider.maxValue = statHandler.ReloadCoolTime.total;
        slider.value = controller.curReloadCool;
    }

    private void OnEnable()
    {
        if(player != null)
            UpdateData();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = controller.curReloadCool;
    }
}
