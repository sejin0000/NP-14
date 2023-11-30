using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomFindPanel : MonoBehaviour
{
    public Button BackButton;

    public void OnBackButtonClicked()
    {
        LobbyManager.Instance.SetPanel(PanelType.MainLobbyPanel);
    }
}
