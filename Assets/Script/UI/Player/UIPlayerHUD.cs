using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static LobbyPanel;

public class UIPlayerHUD : UIMainGame
{
    [SerializeField] private Image portrait;

    private UIPlayerHP hpGauge;
    private UIPlayerDodge dodgeGauge;
    private UIPlayerSkill skillGauge;
    private UIBulletIndicator bulletIndicator;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer;
        else
            player = GameManager.Instance.clientPlayer;

        hpGauge = GetComponentInChildren<UIPlayerHP>();
        dodgeGauge = GetComponentInChildren<UIPlayerDodge>();
        skillGauge = GetComponentInChildren<UIPlayerSkill>();
        bulletIndicator = GetComponentInChildren<UIBulletIndicator>();

        SetupData();
    }

    void SetupData()
    {
        Debug.Log("Initialize from [PlayerHUD]'s UIPlayerHUD Comp");
        hpGauge.Initialize();
        dodgeGauge.Initialize();
        skillGauge.Initialize();
        bulletIndicator.Initialize();

        string spritePath = "Images/CharClass";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CustomProperyDefined.CLASS_PROPERTY, out object temp);
        
        Sprite playerImage = Resources.Load<Sprite>($"{spritePath}{temp}");
        portrait.sprite = playerImage;
    }

    public void Update()
    {
        dodgeGauge?.UpdateValue();
        skillGauge?.UpdateValue();
    }

    public override void Foo()
    {
        Debug.Log("Foo! " + GetType().Name);
    }
}
