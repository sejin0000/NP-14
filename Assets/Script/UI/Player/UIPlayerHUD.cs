using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayerHUD : UIBase
{
    [SerializeField] private Image portrait;

    private UIPlayerHP hpGauge;
    private UIBulletIndicator bulletIndicator;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer;
        else
            player = MainGameManager.Instance.InstantiatedPlayer;

        hpGauge = GetComponentInChildren<UIPlayerHP>();
        bulletIndicator = GetComponentInChildren<UIBulletIndicator>();

        Initialize();
    }

    public override void Initialize()
    {
        Debug.Log("Initialize from [PlayerHUD]'s UIPlayerHUD Comp");
        hpGauge.Initialize();
        bulletIndicator.Initialize();

        Sprite playerImage;
        string spritePath = "Images/CharClass";

        player.GetComponent<PhotonView>().Owner.CustomProperties.TryGetValue("Char_Class", out object curClassType);
        playerImage = Resources.Load<Sprite>($"{spritePath}{curClassType}");
        portrait.sprite = playerImage;
    }
}
