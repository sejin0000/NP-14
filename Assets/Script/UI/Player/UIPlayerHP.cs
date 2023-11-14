using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerHP : UIBase
{
    private Slider hpGauge;
    private PlayerStatHandler playerStats;

    private void Awake()
    {
        hpGauge = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        //Debug.Log("[PlayerStatHandler]" + "Start Done");
    }

    public override void Initialize()
    {
        InitializeData();
        SetValue();
    }

    void InitializeData()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            playerStats = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        else
            playerStats = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();

        playerStats.HitEvent += SetValue;
    }

    private void SetValue()
    {
        //Debug.Log("[PlayerStatHandler]" + playerStats.ToString());
        hpGauge.minValue = 0;
        hpGauge.maxValue = playerStats.HP.total;
        hpGauge.value = playerStats.CurHP;
        //Debug.Log("[PlayerStatHandler]" + "SetValue Done");
    }
}
