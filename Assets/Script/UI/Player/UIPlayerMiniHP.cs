using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerMiniHP : UIBase//, ICommonUI
{
    private Slider hpGauge;
    private PlayerStatHandler playerStats;

    public override void Initialize()
    {
        InitializeData();
        UpdateValue();
        Open();
    }

    void InitializeData()
    {
        var player = transform.parent.GetComponent<UIPlayerMiniHUD>().Player;

        hpGauge = GetComponentInChildren<Slider>();
        playerStats = player.GetComponent<PlayerStatHandler>();

        playerStats.OnChangeCurHPEvent += UpdateValue;
    }

    private void UpdateValue()
    { 
        hpGauge.minValue = 0;
        hpGauge.maxValue = playerStats.HP.total;
        hpGauge.value = playerStats.CurHP;
    }
}
