using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;

public enum AnnouncementType
{
    NicknameSuccess,
    NicknameFailure,
    GameEnd,
    ToLobby,
}
public class SetupAnnouncePopup : MonoBehaviour
{
    [Header("Announcement")]
    [SerializeField] private TextMeshProUGUI announcementText;

    [Header("Button")]
    [SerializeField] private GameObject ButtonArea;
    private HorizontalLayoutGroup areaControl;
    [SerializeField] private Button AllowButton;
    [SerializeField] TextMeshProUGUI AllowButtonText;
    [SerializeField] private Button DenyButton;
    [SerializeField] TextMeshProUGUI DenyButtonText;

    [Header("PopupType")]
    [SerializeField] AnnouncementType PopupType;

    private Dictionary<AnnouncementType, string> AnnounceDict;
    private Dictionary<AnnouncementType, int> ButtonDict;

    [HideInInspector]
    public static string ANNOUNCEMENT_NICKNAME_SUCCESS = "닉네임이 변경되었습니다.";
    public static string ANNOUNCEMENT_NICKNAME_FAILURE = "닉네임은 2~10 글자의 한글 / 영어 / 숫자의 조합으로만 가능합니다.";
    public static string ANNOUNCEMENT_GAME_END = "게임을 종료하시겠습니까?";
    public static string ANNOUNCEMENT_TO_LOBBY = "당신이 떠나면, 팀원들도 같이 로비로 떠나게 됩니다. 로비로 떠나시겠습니까?";


    public void Initialize(AnnouncementType announcementType, string nickname = "")
    {
        this.gameObject.SetActive(true);

        areaControl = ButtonArea.GetComponent<HorizontalLayoutGroup>();
        PopupType = announcementType;

        SetAnnounceDict();
        SetButtonDict();

        announcementText.text = AnnounceDict[announcementType];
        SetButton(ButtonDict[announcementType]);
        AllowButton.onClick.AddListener(() => AllowButtonClicked(announcementType, nickname));
        DenyButton.onClick.AddListener(() => DenyButtonClicked(announcementType));
    }

    private void SetAnnounceDict()
    {
        AnnounceDict = new Dictionary<AnnouncementType, string>
        {
            { AnnouncementType.NicknameSuccess, ANNOUNCEMENT_NICKNAME_SUCCESS },
            { AnnouncementType.NicknameFailure, ANNOUNCEMENT_NICKNAME_FAILURE },
            { AnnouncementType.GameEnd, ANNOUNCEMENT_GAME_END },
            { AnnouncementType.ToLobby, ANNOUNCEMENT_TO_LOBBY }
        };
    }

    private void SetButtonDict()
    {
        ButtonDict = new Dictionary<AnnouncementType, int>
        {
            { AnnouncementType.NicknameSuccess, 1 },
            { AnnouncementType.NicknameFailure, 1 },
            { AnnouncementType.GameEnd, 2 },
            { AnnouncementType.ToLobby, 2 }
        };
    }

    private void SetButton(int numOfButtons)
    {
        switch(numOfButtons) 
        {
            case 0:
                break;
            case 1:
                AllowButton.gameObject.SetActive(true);
                DenyButton.gameObject.SetActive(false);
                AllowButtonText.text = "확인";
                areaControl.spacing = 0;
                break;
            case 2:
                AllowButton.gameObject.SetActive(true);
                DenyButton.gameObject.SetActive(true);
                AllowButtonText.text = "결정";
                areaControl.spacing = 135;
                break;
        }
    }

    private void AllowButtonClicked(AnnouncementType announcementType, string nickname = "")
    {
        switch((int)announcementType)
        {
            case (int)AnnouncementType.NicknameSuccess:
                PhotonNetwork.LocalPlayer.NickName = nickname;
                this.gameObject.SetActive(false);
                break;
            case (int)AnnouncementType.NicknameFailure:
                this.gameObject.SetActive(false);
                break;
            case (int)AnnouncementType.GameEnd:
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
                this.gameObject.SetActive(false);
                break;
            case (int)AnnouncementType.ToLobby:
                PhotonNetwork.AutomaticallySyncScene = false;
                PhotonNetwork.LoadLevel("LobbyScene");
                break;
        }
    }

    private void DenyButtonClicked(AnnouncementType announcementType)
    {
        switch ((int)announcementType)
        {
            case (int)AnnouncementType.NicknameSuccess:
            case (int)AnnouncementType.NicknameFailure:
            case (int)AnnouncementType.GameEnd:
            case (int)AnnouncementType.ToLobby:
                this.gameObject.SetActive(false);
                break;            
        }
    }
}
