using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.InputSystem;

public class TestAugmentPanel : MonoBehaviourPun
{
    [Header("Buttons")]
    [SerializeField] private Button stat1Button;
    [SerializeField] private Button stat2Button;
    [SerializeField] private Button stat3Button;
    [SerializeField] private Button special1Button;
    [SerializeField] private Button special2Button;
    [SerializeField] private Button special3Button;
    [SerializeField] private Button soldier1Button;
    [SerializeField] private Button soldier2Button;
    [SerializeField] private Button soldier3Button;
    [SerializeField] private Button shotGun1Button;
    [SerializeField] private Button shotGun2Button;
    [SerializeField] private Button shotGun3Button;
    [SerializeField] private Button sniper1Button;
    [SerializeField] private Button sniper2Button;
    [SerializeField] private Button sniper3Button;
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
            buttonDictionary.Add("Soldier1", soldier1Button);
            buttonDictionary.Add("Soldier2", soldier2Button);
            buttonDictionary.Add("Soldier3", soldier3Button);
            buttonDictionary.Add("ShotGun1", shotGun1Button);
            buttonDictionary.Add("ShotGun2", shotGun2Button);
            buttonDictionary.Add("ShotGun3", shotGun3Button);
            buttonDictionary.Add("Sniper1", sniper1Button);
            buttonDictionary.Add("Sniper2", sniper2Button);
            buttonDictionary.Add("Sniper3", sniper3Button);
        }

        augmentButtonPath = "Prefabs/TestScene/TestAugment";
        stat1Button.onClick.AddListener(() => ShowAugmentListOfButton(stat1Button));
        stat2Button.onClick.AddListener(() => ShowAugmentListOfButton(stat2Button));
        stat3Button.onClick.AddListener(() => ShowAugmentListOfButton(stat3Button));
        
        special1Button.onClick.AddListener(() => ShowAugmentListOfButton(special1Button));
        special2Button.onClick.AddListener(() => ShowAugmentListOfButton(special2Button));
        special3Button.onClick.AddListener(() => ShowAugmentListOfButton(special3Button));

        soldier1Button.onClick.AddListener(() => ShowAugmentListOfButton(soldier1Button));
        soldier2Button.onClick.AddListener(() => ShowAugmentListOfButton(soldier2Button));
        soldier3Button.onClick.AddListener(() => ShowAugmentListOfButton(soldier3Button));

        shotGun1Button.onClick.AddListener(() => ShowAugmentListOfButton(shotGun1Button));
        shotGun2Button.onClick.AddListener(() => ShowAugmentListOfButton(shotGun2Button));
        shotGun3Button.onClick.AddListener(() => ShowAugmentListOfButton(shotGun3Button));
        
        sniper1Button.onClick.AddListener(() => ShowAugmentListOfButton(sniper1Button));
        sniper2Button.onClick.AddListener(() => ShowAugmentListOfButton(sniper2Button));
        sniper3Button.onClick.AddListener(() => ShowAugmentListOfButton(sniper3Button));

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
            GetDictionary(key, cnt, DictType.Special);
            GetDictionary(key, cnt, DictType.Soldier);
            GetDictionary(key, cnt, DictType.ShotGun);
            GetDictionary(key, cnt, DictType.Sniper);
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

    private void GetDictionary(string key, int cnt, DictType dictType)
    {
        var typeDict = GetDictionaryByType(dictType);
        if (typeDict.TryGetValue(key, out List<SpecialAugment> specialButtonList))
        {
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
    }

    public enum DictType
    {
        Special,
        Soldier,
        ShotGun,
        Sniper,
    }

    private Dictionary<string, List<SpecialAugment>> GetDictionaryByType(DictType dictType)
    {
        switch (dictType)
        {
            case DictType.Special:
                return TestMakeAugmentListManager.Instance.SpecialDictionary;
            case DictType.Soldier:
                return TestMakeAugmentListManager.Instance.SoldierDictionary;
            case DictType.ShotGun:
                return TestMakeAugmentListManager.Instance.ShotGunDictionary;
            case DictType.Sniper:
                return TestMakeAugmentListManager.Instance.SniperDictionary;
            default:
                return null;
        }
    }
}
