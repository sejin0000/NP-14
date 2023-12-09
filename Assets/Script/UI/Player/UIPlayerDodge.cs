using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerDodge : UIBase
{
    [SerializeField] private Slider dodgeGauge;
    [SerializeField] private TMP_Text dodgeText;
    private CoolTimeController playerCool;
    private PlayerStatHandler playerStat;

    public override void Initialize()
    {
        InitializeData();
        UpdateValue();
        Open();
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
        dodgeGauge.value = playerStat.RollCoolTime.total - playerCool.curRollCool;
        dodgeText.text = (playerStat.CurRollStack.ToString() + "/" + playerStat.MaxRollStack.ToString());
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
