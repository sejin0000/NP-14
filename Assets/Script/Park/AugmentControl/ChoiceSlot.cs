using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlot : MonoBehaviour//눌러서 골르는 증강 슬롯 열렸을때 해당증강 값을 불러오고 골랐을때 해당 증강을 호출함
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Info;
    public IAugment stat;
    public bool Ispick = false;
    public int listIndex;
    public ResultManager Parent;
    int rare;
    int symbolNum;

    public Sprite tier1;
    public Sprite tier2;
    public Sprite tier3;

    public Sprite symbolStat;
    public Sprite symbolAll;
    public Sprite symbolSniper;
    public Sprite symbolShotgun;
    public Sprite symbolsoldier;

    public Sprite symbolSniperOption;
    public Sprite symbolShotgunOption;
    public Sprite symbolsoldierOption;

    public Image bodyImage;
    public Image symbolImage;
    public Image symbolImageOption;
    public GameObject symbolOptionObj;
    //[Range(1, 3)] int StatType;

    private void OnEnable()// 이름,정보,색 업데이트
    {
        Name.text = stat.Name;
        Ispick = false;
        Info.text = stat.func;
        rare = stat.Rare;
        symbolNum = stat.Code / 1000;
        if (symbolNum == 0) 
        {
            if (stat.Code >= 900) 
            {
                symbolNum = 9;
            }
        }
        bodyImage = gameObject.GetComponent<Image>();
        symbolOptionObj.SetActive(true);

        switch (rare)
        {
            case 1:
                bodyImage.sprite = tier1;
                break;

            case 2:
                bodyImage.sprite = tier2;
                break;

            case 3:
                bodyImage.sprite = tier3;
                break;
        }
        switch (symbolNum)
        {
            case 0:
                symbolImage.sprite = symbolAll;
                symbolOptionObj.SetActive(false);
                break;
            case 1:
                symbolImage.sprite= symbolSniper;
                symbolImageOption.sprite = symbolSniperOption;
                break;

            case 2:
                symbolImage.sprite = symbolsoldier;
                symbolImageOption.sprite = symbolsoldierOption;
                break;

            case 3:
                symbolImage.sprite = symbolShotgun;
                symbolImageOption.sprite = symbolShotgunOption;
                break;
            case 9:
                symbolImage.sprite = symbolStat;
                symbolOptionObj.SetActive(false);
                break;
        }
    }
    public void pick()
    {
        int code = stat.Code;
        Debug.Log($"{code}");
        AugmentManager.Instance.AugmentCall(code);
        //AugmentManager.Instance.Invoke(str, 0);
        Ispick = true;
        ResultManager.Instance.close();
        Parent.SetActiveCheck = false;
    }
   
}
