using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MySpecialListSocket : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Func;
    public GameObject symbolOptionObj;

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

    public void Init(string name, string func, int rare, int Code)
    {
        Name.text = name;
        Func.text = func;
        int symbolNum = Code / 1000;
        bodyImage = gameObject.GetComponent<Image>();
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
                symbolImage.sprite = symbolSniper;
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

}
