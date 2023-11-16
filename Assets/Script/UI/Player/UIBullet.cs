using Microsoft.Unity.VisualStudio.Editor;
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

    public override void Initialize()
    {
        // Change bullet's sprite depend on class.
        // bullet.sprite =
    }

    public void PlayAnim(string clipName)
    {
        bulletAni.Play(clipName);
    }
}
