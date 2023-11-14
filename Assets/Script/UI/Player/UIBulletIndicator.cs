using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class UIBulletIndicator : UIBase, ICommonUI
{
    private TMP_Text ammo;
    private PlayerInputController player;
    private PlayerStatHandler playerStat;

    void ICommonUI.Initialize()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();
        else
            player = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();

        playerStat = player.playerStatHandler;
        
        ammo = GetComponent<TMP_Text>();

        playerStat.OnChangeAmmorEvent += ChangeValue;
    }

    void ICommonUI.Behavior()
    {
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

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
