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
    [SerializeField] private Button SoundIndexButton;
    [SerializeField] private Button AccountIndexButton;

    [Header("SetupBox")]
    [SerializeField] private GameObject SetupBox;
    [SerializeField] private SetupIndex setupState;
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
    [SerializeField] private TextMeshProUGUI setupSubjectText;
    
    private void Awake()
    {
        SetupState = SetupIndex.Sound;
        backButton.onClick.AddListener(OnBackButtonClicked);
    }
    private void SetSubjectText(SetupIndex index)
    {
        switch ((int)index)
        {
            case (int)SetupIndex.Sound:
                setupSubjectText.text = "���� - ����";
                break;
            case (int)SetupIndex.Account:
                setupSubjectText.text = "���� - ����";
                break;
        }
    }

    private void OnBackButtonClicked()
    {
        this.gameObject.SetActive(false);
    }
}
