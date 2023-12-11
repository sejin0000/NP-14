using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerSkill : UIBase, ICommonUI
{
    [SerializeField] private Image skillIcon;
    [SerializeField] private Image skillGauge;
    private PlayerStatHandler playerStats;
    private CoolTimeController playerCool;
    private int playerClass;

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
        GameObject player;
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer;
        else
            player = GameManager.Instance.clientPlayer;

        playerCool = GameManager.Instance.clientPlayer.GetComponent<CoolTimeController>();
        playerStats = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>();

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomProperyDefined.CLASS_PROPERTY, out object temp);
        playerClass = (int)temp;

        var playerInput = GameManager.Instance.clientPlayer.GetComponent<PlayerInputController>();
        playerInput.OnSkillEvent += UpdateSkillIcon;

        UpdateSkillIcon();
    }

    public void UpdateSkillIcon()
    {
        //Debug.LogAssertion($"{playerClass}");
        switch (playerClass)
        {
            default:
                break;
            case 0:
                skillIcon.sprite = GameManager.Instance.clientPlayer.GetComponent<Player1Skill>().Icon;
                break;
            case 1:
                skillIcon.sprite = GameManager.Instance.clientPlayer.GetComponent<Player2Skill>().Icon;
                break;
            case 2:
                skillIcon.sprite = GameManager.Instance.clientPlayer.GetComponent<Player3Skill>().Icon;
                break;
        }
    }

    public void UpdateValue()
    {
        float numerator = playerCool.curSkillCool;
        float denominator = 1 / playerStats.SkillCoolTime.total;
        skillGauge.fillAmount = numerator * denominator;
    }
}
