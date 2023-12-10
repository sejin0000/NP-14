using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class UIBulletIndicator : UIBase
{
    [SerializeField] private TMP_Text ammo;
    [SerializeField] private List<GameObject> bullets;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletParents;

    private PlayerInputController player;
    private PlayerStatHandler playerStat;

    private float ammoMax;
    private float currentAmmo;
    private float spriteWidth;
    [SerializeField] private float spriteSpace = 10f;

    public override void Initialize()
    {
        InitializeData();
        InitializeBullets();
        ChangeValue();
    }

    public void InitializeData()
    {
        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            player = TestGameManagerDohyun.Instance.InstantiatedPlayer.GetComponent<PlayerInputController>();
        else
            player = GameManager.Instance.clientPlayer.GetComponent<PlayerInputController>();

        playerStat = player.playerStatHandler;
        ammoMax = player.playerStatHandler.AmmoMax.total;

        //subscribe event
        player.OnEndReloadEvent += ReloadBullets;
        playerStat.OnChangeAmmorEvent += ChangeValue;
        player.OnAttackEvent += ShootBullet;
    }

    private void ChangeValue()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(playerStat.CurAmmo);
        sb.Append("/");
        sb.Append(playerStat.AmmoMax.total);

        ammo.text = sb.ToString();
    }

    public void InitializeBullets()
    {
        bullets = new List<GameObject>((int)ammoMax);
        for (int i = 0; i < ammoMax; ++i)
        {
            GameObject temp = Instantiate(bulletPrefab, bulletParents.transform);
            bullets.Add(temp);

            spriteWidth = bullets[0].GetComponent<RectTransform>().rect.width;

            if (i > 0)
            {
                Vector3 pos = bullets[i - 1].GetComponent<RectTransform>().anchoredPosition;
                pos.x += ((spriteWidth) + spriteSpace);
                bullets[i].GetComponent<RectTransform>().anchoredPosition = pos;
            }
        }
    }

    public void ResizeBullets()
    {
        int prevCount = bullets.Count;
        int newCount = (int)ammoMax;
        
        // 30��25�߷� �پ����� �� 24~29index�� ������Ʈ ����
        // 30��32�߷� �þ ���̽��� �ݺ����� �ɸ��� ����
        for (int i=newCount; i<prevCount; ++i)
            Destroy(bullets[i]);

        // �þ��ŭ źâ �߰�
        for (int i=prevCount; i < newCount; ++i)
        {
            GameObject temp = Instantiate(bulletPrefab, bulletParents.transform);
            bullets.Add(temp);

            if (i > 0)
            {
                Vector3 pos = bullets[i - 1].GetComponent<RectTransform>().anchoredPosition;
                pos.x += ((spriteWidth) + spriteSpace);
                bullets[i].GetComponent<RectTransform>().anchoredPosition = pos;
            }     
        }
    }

    public void UpdateAmmoMax()
    {
        if (ammoMax != playerStat.AmmoMax.total)
        {
            ammoMax = playerStat.AmmoMax.total;
            ResizeBullets();
            Debug.Log("[UIBulletIndicator] ammoMax: " + ammoMax);
        }
    }

    public void ShootBullet()
    {
        currentAmmo = playerStat.CurAmmo;
        //Debug.Log("[UIBulletIndicator] currentAmmo: " + currentAmmo);
        int index_L = (int)(ammoMax - currentAmmo);
        int index_R = (int)currentAmmo;
        //Debug.Log("[UIBulletIndicator] index_R: " + index_R);

        if (index_R >= 0)
        {
            //Debug.Log("[UIBulletIndicator] index: " + index_R);
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
    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
    }
}
