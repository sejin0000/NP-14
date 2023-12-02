using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetupPopup : MonoBehaviour
{
    public enum SetupIndex
    {
        Sound,
        Account,
    }

    [Header("Button")]
    [SerializeField] private Button backButton;

    [Header("Index")]
    [SerializeField] private TextMeshProUGUI setupSubjectText;
    [SerializeField] private Button SoundIndexButton;
    [SerializeField] private Button AccountIndexButton;

    [Header("SetupBox")]
    [SerializeField] private GameObject SetupBox;
    private RectTransform SetupBoxRect;
    private float SetupBoxRectWidth;
    [SerializeField] private SetupIndex setupState;
    [SerializeField] private GameObject SetupBoxScrollContent;

    [Header("SetupAnnouncePopup")]
    private GameObject setupAnnouncePopup;
    public SetupAnnouncePopup _setupAnnouncePopup;

    public SetupIndex SetupState
    {
        get { return setupState; } 
        set
        {
            if (value != setupState)
            {
                setupState = value;
                SetSubjectText(setupState);
            }
        }
    }
    
    private void Awake()
    {
        backButton.onClick.AddListener(OnBackButtonClicked);
        SoundIndexButton.onClick.AddListener(OnSoundIndexButtonClicked);
        AccountIndexButton.onClick.AddListener(OnAccountIndexButtonClicked);

        SetupBoxRect = SetupBoxScrollContent.GetComponent<RectTransform>();
        SetupBoxRectWidth = SetupBoxRect.sizeDelta.x;

        SetAnnouncePopup();
    }

    private void OnEnable()
    {
        SetupState = SetupIndex.Sound;
        ClearSetupBox();
        SetSubjectText(SetupState);
        SetSetupPrefab(PrefabPathes.SOUND_CONTROL_PREFAB_PATH);
    }
    private void SetSubjectText(SetupIndex index)
    {
        switch ((int)index)
        {
            case (int)SetupIndex.Sound:
                setupSubjectText.text = "설정 - 사운드";
                break;
            case (int)SetupIndex.Account:
                setupSubjectText.text = "설정 - 계정";
                break;
        }
    }
    private void SetAnnouncePopup()
    {
        setupAnnouncePopup = Instantiate(Resources.Load<GameObject>(PrefabPathes.SETUP_ANNOUNCE_POPUP), transform, false);
        _setupAnnouncePopup = setupAnnouncePopup.GetComponent<SetupAnnouncePopup>();
        setupAnnouncePopup.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

    private void OnSoundIndexButtonClicked()
    {
        if (SetupState == SetupIndex.Sound) 
        {
            return;
        }
        SetupState = SetupIndex.Sound;
        ClearSetupBox();
        SetSetupPrefab(PrefabPathes.SOUND_CONTROL_PREFAB_PATH);
    }

    private void OnAccountIndexButtonClicked()
    {
        if (SetupState == SetupIndex.Account)
        {
            return;
        }
        SetupState = SetupIndex.Account;
        ClearSetupBox();
        SetSetupPrefab(PrefabPathes.NICKNAME_CHANGE_PREFAB_PATH);
    }

    private void SetSetupPrefab(string path)
    {
        var prefab = Instantiate(Resources.Load<GameObject>(path), SetupBoxScrollContent.transform, false);
        SetupBoxRect.sizeDelta += new Vector2(0, prefab.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ClearSetupBox()
    {
        for (int i = 0; i < SetupBoxScrollContent.transform.childCount; i++) 
        {
            Destroy(SetupBoxScrollContent.transform.GetChild(i).gameObject);
        }
        SetupBoxRect.sizeDelta = new Vector2(SetupBoxRectWidth, 0);
    }
}
