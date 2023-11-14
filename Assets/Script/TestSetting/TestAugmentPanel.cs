using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestAugmentPanel : MonoBehaviourPun
{
    [Header("Buttons")]
    [SerializeField] private Button stat1Button;
    [SerializeField] private Button stat2Button;
    [SerializeField] private Button stat3Button;
    [SerializeField] private Button special1Button;
    [SerializeField] private Button special2Button;
    [SerializeField] private Button special3Button;
    [SerializeField] private Button closePanelButton;

    public GameObject AugmentScrollViewContent;
    private Dictionary<string, Button> buttonDictionary;

    private void Awake()
    {
        if (buttonDictionary == null) 
        {
            buttonDictionary.Add("stat1", stat1Button);
            buttonDictionary.Add("stat2", stat2Button);
            buttonDictionary.Add("stat3", stat3Button);
            buttonDictionary.Add("special1", special1Button);
            buttonDictionary.Add("special2", special2Button);
            buttonDictionary.Add("special3", special3Button);        
        }

        stat1Button.onClick.AddListener(ShowAugmentListOfButton);
    }

    public void ShowAugmentListOfButton()
    {

    }
}
