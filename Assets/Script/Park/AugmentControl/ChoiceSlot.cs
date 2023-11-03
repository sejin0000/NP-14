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
    
    int rare;

    //[Range(1, 3)] int StatType;

    private void OnEnable()// 이름,정보,색 업데이트
    {
        Name.text = stat.Name;
        Ispick = false;
        Info.text = stat.func;
        rare = stat.Rare;
        Image image = gameObject.GetComponent<Image>();
        //Debug.Log($"{rare}");
        switch (rare)//증강 티어에 따라 색넣어주는데 색 뭔가 이상함 나중에 머터리얼 넣어줘야할듯
        {
            case 1:
                image.color = new Color(205 / 255f, 127 / 255f, 50 / 255f);//브 
                break;

            case 2:
                image.color = new Color(192 / 255f, 192 / 255f, 192 / 255f);//실
                break;

            case 3:
                image.color = new Color(255 / 255f, 215 / 255f, 0 / 255f);//골
                break;
        }
    }
    public void pick()
    {
        string str = "A" + stat.Code.ToString();
        Debug.Log($"{str}");
        AugmentManager.Instance.Invoke(str, 0);
        Ispick = true;
        ResultManager.Instance.close();
    }
    public void pick2()//테스트용 증강 나중에 없애야함
    {
        string str = "A" + "0124";//뒷숫자컨
        Invoke(str, 0);
    }

   
}
