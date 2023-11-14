using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    private string augmentButtonPath;

    private void Awake()
    {
        if (buttonDictionary == null) 
        {
            buttonDictionary = new Dictionary<string, Button>();
            buttonDictionary.Add("Stat1", stat1Button);
            buttonDictionary.Add("Stat2", stat2Button);
            buttonDictionary.Add("Stat3", stat3Button);
            buttonDictionary.Add("Special1", special1Button);
            buttonDictionary.Add("Special2", special2Button);
            buttonDictionary.Add("Special3", special3Button);        
        }

        augmentButtonPath = "Prefabs/TestScene/TestAugment";
        stat1Button.onClick.AddListener(() => ShowAugmentListOfButton(stat1Button));
        stat2Button.onClick.AddListener(() => ShowAugmentListOfButton(stat2Button));
        stat3Button.onClick.AddListener(() => ShowAugmentListOfButton(stat3Button));
        
        special1Button.onClick.AddListener(() => ShowAugmentListOfButton(special1Button));
        special2Button.onClick.AddListener(() => ShowAugmentListOfButton(special2Button));
        special3Button.onClick.AddListener(() => ShowAugmentListOfButton(special3Button));

        closePanelButton.onClick.AddListener(CloseAugmentPanel);
    }

    public void ShowAugmentListOfButton(Button clickedButton)
    {
        // scroll view size init
        int cnt = 0;
        AugmentScrollViewContent.GetComponent<RectTransform>().sizeDelta = new Vector2(410, 110);

        // 기존 목록 삭제
        for (int i = 0; i < AugmentScrollViewContent.transform.childCount; i++) 
        {
            Destroy(AugmentScrollViewContent.transform.GetChild(i).gameObject);
        }

        // 새 목록 추가
        var key = buttonDictionary.FirstOrDefault(x => x.Value == clickedButton).Key;
        TestMakeAugmentListManager.Instance.StatDictionary.TryGetValue(key, out List<IAugment> buttonList);
        if (buttonList == null)
        { 
            TestMakeAugmentListManager.Instance.SpecialDictionary.TryGetValue(key, out List<SpecialAugment> specialButtonList);
            foreach (var augment in specialButtonList)
            {
                GameObject sampleButton = Instantiate(Resources.Load<GameObject>(augmentButtonPath));
                sampleButton.GetComponent<TestAugmentBtn>().Initialize(augment.Name, augment.Code);
                sampleButton.transform.SetParent(AugmentScrollViewContent.transform, false);
                if (cnt > 1)
                {
                    cnt = 0;
                    AugmentScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 110);
                }
                cnt += 1;
            }
        }
        else
        {
            foreach (var augment in buttonList)
            {
                GameObject sampleButton = Instantiate(Resources.Load<GameObject>(augmentButtonPath), AugmentScrollViewContent.transform, false);
                sampleButton.GetComponent<TestAugmentBtn>().Initialize(augment.Name, augment.Code);
                sampleButton.transform.SetParent(AugmentScrollViewContent.transform, false);
                if (cnt > 1)
                {
                    cnt = 0;
                    AugmentScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 110);
                }
                cnt += 1;
            }
        }

    }

    public void CloseAugmentPanel()
    {
        this.gameObject.SetActive(false);
    }
}
