using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class UIBulletIndicator : UIBase, ICommonUI
{
    [SerializeField] private TMP_Text ammo;
    [SerializeField] private GameObject[] bullets;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletParents;

    private PlayerInputController player;
    private PlayerStatHandler playerStat;

    private float ammoMax;
    private float currentAmmo;
    [SerializeField] private float spriteWidth;
    [SerializeField] private float spriteSpace;
    

    void ICommonUI.Initialize()
    {
        InitializeData();
    }

    void ICommonUI.Behavior()
    {
        ChangeValue();
    }

    public void InitializeData()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();
        else
            player = MainGameManager.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();

        playerStat = player.playerStatHandler;
        ammoMax = player.playerStatHandler.AmmoMax.total;
        
        player.OnEndReloadEvent += ReloadBullets;
        playerStat.OnChangeAmmorEvent += ChangeValue;
        player.OnAttackEvent += ShootBullet;

    }

    public override void Initialize()
    {
        InitializeData();
        InitializeBullets();
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

    public void InitializeBullets()
    {
        Debug.Log("[InitializeBullets] AKAKAKAKAAKAKAK");
        bullets = new GameObject[(int)ammoMax];
        for (int i = 0; i < ammoMax; ++i)
        {
            GameObject temp = Instantiate(bulletPrefab, bulletParents.transform);
            bullets[i] = temp;

            if (i > 0)
            {
                Vector3 pos = bullets[i - 1].transform.position;
                pos.x += ((spriteWidth) + spriteSpace);
                bullets[i].transform.position = pos;
            }
        }
    }

    public void UpdateAmmoMax()
    {
        if (ammoMax != playerStat.AmmoMax.total)
        {
            ammoMax = playerStat.AmmoMax.total;
            InitializeBullets();
        }
    }

    public void ShootBullet()
    {
        currentAmmo = playerStat.CurAmmo;
        int index_L = (int)(ammoMax - currentAmmo);
        int index_R = (int)currentAmmo-1;

        if (index_R >= 0)
        {
            Debug.Log("[UIBulletIndicator] index: " + index_R);
            bullets[index_R].GetComponent<UIBullet>().PlayAnim("Shooting");
        }
    }

    public void ReloadBullets()
    {
        UpdateAmmoMax();
        for(int i=0; i<ammoMax; ++i)
        {
            bullets[i].SetActive(true);
            bullets[i].GetComponent<UIBullet>().PlayAnim("Idle");
        }
    }
}
