using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerDodge : UIBase, ICommonUI
{
    [SerializeField] private Slider dodgeGauge;
    private CoolTimeController playerCool;
    private PlayerStatHandler playerStat;

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
        {
            playerCool = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<CoolTimeController>();
            playerStat = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        }
        else
        {
            playerCool = GameManager.Instance.clientPlayer.GetComponent<CoolTimeController>();
            playerStat = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>();
        }
    }

    public void UpdateValue()
    {
        dodgeGauge.minValue = 0;
        dodgeGauge.maxValue = playerStat.RollCoolTime.total;

        //if (playerStat.CanRoll)
        //    dodgeGauge.value = playerStat.RollCoolTime.total;
        //else
        
        dodgeGauge.value = playerStat.RollCoolTime.total - playerCool.curRollCool;

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
