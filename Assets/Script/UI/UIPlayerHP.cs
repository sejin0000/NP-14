using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
        playerStats = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        Initialize();
    }

    private void Initialize()
    {
        SetValue();
    }

    private void SetValue()
    {
        hpGauge.minValue = 0;
        hpGauge.maxValue = playerStats.HP.total;
        hpGauge.value = playerStats.CurHP;
    }
}
