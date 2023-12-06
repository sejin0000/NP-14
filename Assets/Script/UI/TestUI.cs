using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TestUI : UIBase
{
    private GameObject SetupPopup;
   
    public override void Initialize()
    {
        UIManager.Instance.GetUIComponent<UIBase>().Foo();
        UIManager.Instance.GetUIComponent<UIPlayerHUD>().Foo();        
    }

    // Start is called before the first frame update
    void Start()
    {
        SetupPopup = UIManager.Instance.GetUIObject("SetupPopupPrefab");         
    }

    private void Update()
    {
        CheckEscKeyInput();
    }

    
    private void CheckEscKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetupPopup.SetActive(!SetupPopup.activeSelf);
        }
    }
}
