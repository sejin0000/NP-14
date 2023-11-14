using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIReloadHUD : UIBase, ICommonUI
{
    private Slider slider;
    private GameObject player;
    private CoolTimeController controller;
    private PlayerStatHandler statHandler;

    void ICommonUI.Initialize()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.gameObject;
        else
            player = MainGameManager.Instance.InstantiatedPlayer.gameObject;

        controller = player.GetComponent<CoolTimeController>();
        statHandler = player.GetComponent<PlayerStatHandler>();
        //player.GetComponent<TopDownCharacterController>().OnEndReloadEvent += Close;
        slider = GetComponentInChildren<Slider>();

        Debug.Log("[[CheckInterface] Done Initialize");
    }

    void ICommonUI.Behavior()
    {
        UpdateData();
        Close();
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

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
