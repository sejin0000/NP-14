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
    private SetupPopup setupPopup;

    private void OnEnable()
    {
        setupPopup = LobbyManager.Instance.MainLobbyP.SetupPopup.GetComponent<SetupPopup>();
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
        string pattern = @"^[a-zA-Z��-�R0-9]{2,10}$";

        return Regex.IsMatch(nickNameInput.text, pattern);
    }
}