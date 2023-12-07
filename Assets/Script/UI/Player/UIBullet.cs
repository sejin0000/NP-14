using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBullet : UIBase
{
    private Animation bulletAni;
    [SerializeField] private GameObject bullet;

    private Sprite bulletSprite;

    private void Awake()
    {
        bulletAni = GetComponent<Animation>();
    }

    private void Start()
    {
        // Change bullet's sprite depend on class.
        bulletSprite = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>().indicatorSprite;
        bullet.GetComponent<Image>().sprite = bulletSprite;
    }

    public void PlayAnim(string clipName)
    {
        bulletAni.Play(clipName);
    }
}
