using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHUD : UIBase
{
    UIPlayerHP hpGauge;
    UIBulletIndicator bulletIndicator;

    // Start is called before the first frame update
    void Start()
    {
        hpGauge = GetComponentInChildren<UIPlayerHP>();
        bulletIndicator = GetComponentInChildren<UIBulletIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
