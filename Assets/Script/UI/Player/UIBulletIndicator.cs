using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class UIBulletIndicator : UIBase
{
    private TMP_Text ammo;
    private PlayerInputController player;
    private PlayerStatHandler playerStat;

    private void Awake()
    {
        ammo = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        //player = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();
        player = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>(); 
        playerStat = player.playerStatHandler;

        playerStat.OnChangeAmmorEvent += ChangeValue;
        ChangeValue();
    }

    private void ChangeValue()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(playerStat.CurAmmo);
        sb.Append("/");
        sb.Append(playerStat.AmmoMax.total);

        ammo.text = sb.ToString();
    }
}
