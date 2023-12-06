using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerSkill : UIBase, ICommonUI
{
    [SerializeField] private Image skillGauge;
    private PlayerStatHandler playerStats;
    private CoolTimeController playerCool;

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
        else
            playerStats = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>();

        playerCool = GameManager.Instance.clientPlayer.GetComponent<CoolTimeController>();
        playerStats = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>();
    }

    public void UpdateValue()
    {
        float numerator = playerCool.curSkillCool;
        float denominator = 1 / playerStats.SkillCoolTime.total;
        skillGauge.fillAmount = numerator * denominator;
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
