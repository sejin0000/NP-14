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

    public void Initialize(string sceneName, bool isSelected, TestRoomOptionPopup popup = null, TestLobbyPanel panel = null)
    {
        sceneNameText.text = sceneName;
        IsSelected = isSelected;
        sceneConnectButton.onClick.AddListener(OnSceneConnectButtonClicked);
        if (popup != null)
        {
            sceneConnectButton.onClick.AddListener(() =>
            {
                OnSceneConnectButtonClicked();
                popup.OnSceneConnectButtonClicked(this);
            });
        }
        if (panel != null)
        {
            sceneConnectButton.onClick.AddListener(() =>
            {
                OnSceneConnectButtonClicked();
                panel.OnSceneConnectButtonClicked(this);
            });
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
        var selectedColor = new Color(54f / 255f, 117f / 255f, 117f / 255f); 
        var unSelectedColor = new Color(205f / 255f, 146f / 255f, 146f / 255f); 
        backgroundColor = selectedColor;
        sceneNameText.color = unSelectedColor;
        foreach (var button in allButtons)
        {
            if (button != this)
            {
                button.IsSelected = false;
                button.gameObject.GetComponent<Image>().color = unSelectedColor;
                button.sceneNameText.color = selectedColor;
            }
        }
    }
}
