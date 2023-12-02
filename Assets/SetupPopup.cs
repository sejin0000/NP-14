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

    [Header("SoundSetupContents")]
    [SerializeField] private GameObject SoundControlPrefab;

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
        SetupState = SetupIndex.Sound;
        backButton.onClick.AddListener(OnBackButtonClicked);
        SetupBoxRect = SetupBoxScrollContent.GetComponent<RectTransform>();
        SetupBoxRectWidth = SetupBoxRect.sizeDelta.x;
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

    private void OnBackButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

    private void OnSoundIndexButtonClicked()
    {
        SetupBoxRect.sizeDelta = new Vector2(SetupBoxRectWidth, 0);
        SetSetupPrefab(PrefabPathes.SOUND_CONTROL_PREFAB_PATH);
    }

    private void SetSetupPrefab(string path)
    {
        var prefab = Resources.Load<GameObject>(path);
        Instantiate(prefab, SetupBoxScrollContent.transform, false);
        SetupBoxRect.sizeDelta += new Vector2(0, prefab.GetComponent<RectTransform>().sizeDelta.y);
    }
}
