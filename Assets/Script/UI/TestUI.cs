using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestUI : UIBase
{
    public override void Initialize()
    {
        UIManager.Instance.GetUIComponent<UIBase>().Foo();
        UIManager.Instance.GetUIComponent<UIPlayerHUD>().Foo();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
