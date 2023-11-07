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
        //PlayerDataManager.Instance.OnJoinedPlayer += Initialize;
    }

    private void Initialize()
    {
        //player = PlayerDataManager.Instance.Player.GetComponent<PlayerInputController>();
        playerStat = player.playerStatHandler;

        player.OnAttackEvent += ChangeValue;
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
