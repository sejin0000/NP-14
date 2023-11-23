using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerHP : UIBase, ICommonUI
{
    private Slider hpGauge;
    private PlayerStatHandler playerStats;

    void ICommonUI.Initialize()
    {
        InitializeData();
    }

    void ICommonUI.Behavior()
    {
        UpdateValue();
        Open();
    }

    public override void Initialize()
    {
        InitializeData();
        UpdateValue();
    }

    void InitializeData()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            playerStats = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        else if (MainGameManager.Instance != null)
            playerStats = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        else
            playerStats = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>();

        hpGauge = GetComponentInChildren<Slider>();

        playerStats.OnChangeCurHPEvent += UpdateValue;
    }

    private void UpdateValue()
    {
        //Debug.Log("[PlayerStatHandler]" + playerStats.ToString());
        hpGauge.minValue = 0;
        hpGauge.maxValue = playerStats.HP.total;
        hpGauge.value = playerStats.CurHP;
        //Debug.Log("[PlayerStatHandler]" + "SetValue Done");
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
