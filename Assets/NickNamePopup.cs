using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NickNamePopup : MonoBehaviour
{
    public enum NickNameState
    {
        Asking,
        Checking,
        Denying,
        Welcome,
    }

    [SerializeField] private TextMeshProUGUI annoucnementText;

    [Header("NickName")]    
    [SerializeField] private GameObject setNickNameBox;
    [SerializeField] private TMP_InputField nickNameInput;
    [SerializeField] private Button submitButton;
    public string Nickname;

    [Header("Check")]
    [SerializeField] private Button allowButton;
    [SerializeField] private Button cancelButton;

    [Header("State")]
    [SerializeField] private NickNameState state;
    public NickNameState State
    {
        get { return state; }
        set
        {
            if (value != state)
            {
                state = value;
                SetAnnouncement(Nickname);
            }
        }
    }

    private void Awake()
    {
        submitButton.onClick.AddListener(OnSubmitButtonClicked);
    }
    public void Initialize()
    {        
        State = NickNameState.Asking;
        Nickname = "";        
    }

    private void SetAnnouncement(string nickname)
    {
        switch ((int)state)
        {
            case (int)NickNameState.Asking:
                annoucnementText.text = "Welcome. \n ´Ð³×ÀÓÀ» ¼³Á¤ÇØÁÖ¼¼¿ä";
                break;
            case (int)NickNameState.Checking:
                annoucnementText.text = "ÀÌ ´Ð³×ÀÓÀ¸·Î °áÁ¤ÇÏ½Ã°Ú½À´Ï±î?";
                break;
            case (int)NickNameState.Denying:
                annoucnementText.text = "´Ð³×ÀÓÀ» ´Ù½Ã ÀÔ·ÂÇØÁÖ¼¼¿ä.\n 2 ~ 10±ÛÀÚ / Æ¯¼ö¹®ÀÚ X";
                break;
            case (int)NickNameState.Welcome:
                annoucnementText.text = $"È¯¿µÇÕ´Ï´Ù.{nickname} ´Ô.";
                break;
            default:
                break;
        }        
    }

    private void CheckNickname()
    {
        if (IsValid())
        {
            State = NickNameState.Checking;
            setNickNameBox.SetActive(false);
            SetActiveButtons(true);
            ClearButtonListner();
            allowButton.onClick.AddListener(OnAllowButtonClickedInChecking);
            cancelButton.onClick.AddListener(OnCancelButtonClickedInChecking);
        }
        else
        {
            State = NickNameState.Denying;
            nickNameInput.text = "";
            setNickNameBox.SetActive(false);
            SetActiveButtons(true);            
            ClearButtonListner();
            allowButton.onClick.AddListener(OnAllowButtonClickedInDenying);
        }
    }

    private bool IsValid()
    {
        string pattern = @"^[a-zA-Z¤¡-ÆR0-9]{2,10}$";

        return Regex.IsMatch(nickNameInput.text, pattern);
    }

    private void OnSubmitButtonClicked()
    {
        CheckNickname();
    }

    private void OnAllowButtonClickedInChecking()
    {
        Nickname = nickNameInput.text;        

        State = NickNameState.Welcome;
        ClearButtonListner();
        cancelButton.gameObject.SetActive(false);
        allowButton.onClick.AddListener(OnAllowButtonClickedInWelcome);
    }

    private void OnAllowButtonClickedInDenying()
    {
        State = NickNameState.Asking;
        ClearButtonListner();
        SetActiveButtons(false);
        setNickNameBox.SetActive(true);
    }
    private void OnCancelButtonClickedInChecking()
    {
        State = NickNameState.Asking;
        ClearButtonListner();
        SetActiveButtons(false);
        setNickNameBox.SetActive(true);
    }

    private void ClearButtonListner()
    {
        allowButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    private void OnAllowButtonClickedInWelcome()
    {
        this.gameObject.SetActive(false);
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.LocalPlayer.NickName = Nickname;
        }
    }

    private void SetActiveButtons(bool isSet)
    {
        allowButton.gameObject.SetActive(isSet);
        cancelButton.gameObject.SetActive(isSet);
    }
}
