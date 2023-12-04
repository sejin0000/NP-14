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
    public static string ANNOUNCEMENT_NICKNAME_SUCCESS = "�г����� ����Ǿ����ϴ�.";
    public static string ANNOUNCEMENT_NICKNAME_FAILURE = "�г����� 2~10 ������ �ѱ� / ���� / ������ �������θ� �����մϴ�.";


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
    }

    private void SetAnnounceDict()
    {
        AnnounceDict = new Dictionary<AnnouncementType, string>
        {
            { AnnouncementType.NicknameSuccess, ANNOUNCEMENT_NICKNAME_SUCCESS },
            { AnnouncementType.NicknameFailure, ANNOUNCEMENT_NICKNAME_FAILURE }
        };
    }

    private void SetButtonDict()
    {
        ButtonDict = new Dictionary<AnnouncementType, int>
        {
            { AnnouncementType.NicknameSuccess, 1 },
            { AnnouncementType.NicknameFailure, 1 }
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
                AllowButtonText.text = "Ȯ��";
                areaControl.spacing = 0;
                break;
            case 2:
                AllowButton.gameObject.SetActive(true);
                DenyButton.gameObject.SetActive(true);
                AllowButtonText.text = "����";
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
        }
    }
}
