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
        //playerStats = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        playerStats.HitEvent += SetValue;
        SetValue();
        //Debug.Log("[PlayerStatHandler]" + "Start Done");
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
