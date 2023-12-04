using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NickNameChangePrefab : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickNameInput;    
    [SerializeField] private Button SubmitButton;
    public SetupPopup setupPopup;

    private void OnEnable()
    {
        if (LobbyManager.Instance != null)
            setupPopup = LobbyManager.Instance.MainLobbyP.SetupPopup.GetComponent<SetupPopup>();
        else if (UIManager.Instance != null)
            setupPopup = UIManager.Instance.GetUIComponent<SetupPopup>();

        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
    }

    private void OnSubmitButtonClicked()
    {
        CheckNickname();
    }

    private void CheckNickname()
    {
        var announcePopup = setupPopup._setupAnnouncePopup;
        if (IsValid())
        {
            announcePopup.Initialize(AnnouncementType.NicknameSuccess, nickNameInput.text);
            nickNameInput.text = "";
        }
        else
        {
            announcePopup.Initialize(AnnouncementType.NicknameFailure);
            nickNameInput.text = "";
        }

    }

    private bool IsValid()
    {
        string pattern = @"^[a-zA-Z¤¡-ÆR0-9]{2,10}$";

        return Regex.IsMatch(nickNameInput.text, pattern);
    }
}
