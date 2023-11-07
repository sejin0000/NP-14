using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHP : UIBase
{
    private Slider hpGauge;
    private PlayerInputController player;
    private PlayerStatHandler playerStats;

    private void Awake()
    {
        hpGauge = GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        //PlayerDataManager.Instance.OnJoinedPlayer += Initialize;
    }

    private void Initialize()
    {
        //player = PlayerDataManager.Instance.Player.GetComponent<PlayerInputController>();
        playerStats = player.playerStatHandler;
        SetValue();
    }

    private void SetValue()
    {
        hpGauge.minValue = 0;
        hpGauge.maxValue = playerStats.HP.total;
        hpGauge.value = 80f; //playerStats.CurHP;
    }
}
