using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneConnectButton : MonoBehaviour
{
    public TextMeshProUGUI sceneNameText;
    public Button sceneConnectButton;
    public bool IsSelected;

    public List<SceneConnectButton> allButtons;

    public void Initialize(string sceneName, bool isSelected, TestRoomOptionPopup popup = null, TestPanel panel = null)
    {
        sceneNameText.text = sceneName;
        IsSelected = isSelected;
        sceneConnectButton.onClick.AddListener(OnSceneConnectButtonClicked);
        if (popup != null) 
        {
            sceneConnectButton.onClick.AddListener(() => popup.OnSceneConnectButtonClicked(this));
        }
        if (panel != null)
        {
            sceneConnectButton.onClick.AddListener(() => panel.OnSceneConnectButtonClicked(this));
        }
    }

    public void InitializeButtonList(List<SceneConnectButton> buttons)
    {
        allButtons = buttons;
    }

    public void OnSceneConnectButtonClicked()
    {
        IsSelected = true;
        var backgroundColor = this.gameObject.GetComponent<Image>().color;
        var selectedColor = new Color(54, 117, 117);
        var unSelectedColor = new Color(205, 146, 146);
        backgroundColor = selectedColor;
        sceneNameText.color = unSelectedColor;
        foreach (var button in allButtons)
        {
            if (button != this)
            {
                button.IsSelected = false;
                button.gameObject.GetComponent<Image>().color = unSelectedColor;
                button.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = selectedColor;
            }
        }
    }
}
