using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToLobbyPrefab : MonoBehaviour
{
    public Button LobbyButton;
    private SetupPopup setupPopup;

    private void OnEnable()
    {
        if (LobbyManager.Instance != null)
            setupPopup = LobbyManager.Instance.MainLobbyP.SetupPopup.GetComponent<SetupPopup>();
        else if (UIManager.Instance != null)
            setupPopup = UIManager.Instance.GetUIComponent<SetupPopup>();

        LobbyButton.onClick.AddListener(OnLobbyButtonClicked);
    }
    public void OnLobbyButtonClicked()
    {
        CheckExit();
    }

    private void CheckExit()
    {
        var checkPopup = setupPopup._setupAnnouncePopup;
        checkPopup.Initialize(AnnouncementType.ToLobby);
    }
}
