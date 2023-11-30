using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBullet : UIBase
{
    private Animation bulletAni;
    [SerializeField] private GameObject bullet;

    private void Awake()
    {
        bulletAni = GetComponent<Animation>();
    }

    private void Start()
    {
        // Change bullet's sprite depend on class.
        bullet.GetComponentInChildren<Image>().sprite = GameManager.Instance.clientPlayer.GetComponent<PlayerStatHandler>().indicatorSprite;
    }

    public void PlayAnim(string clipName)
    {
        bulletAni.Play(clipName);
    }
}
