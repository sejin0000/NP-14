using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemOff : MonoBehaviour
{
    public Button ExitButton;
    private SetupPopup setupPopup;

    private void OnEnable()
    {
        if (LobbyManager.Instance != null)
            setupPopup = LobbyManager.Instance.MainLobbyP.SetupPopup.GetComponent<SetupPopup>();
        else if (UIManager.Instance != null)
            setupPopup = UIManager.Instance.GetUIComponent<SetupPopup>();

        ExitButton.onClick.AddListener(OnExitButtonClicked);
    }
    public void OnExitButtonClicked()
    {
        CheckExit();
    }

    private void CheckExit()
    {
        var checkPopup = setupPopup._setupAnnouncePopup;
        checkPopup.Initialize(AnnouncementType.GameEnd);
    }
}
