using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceSlot : MonoBehaviour
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
        switch (rare)
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
    public void pick2()//내가 테스트 할려고 뒤숫자 바꾸면서 하는거 
    {
        string str = "A" + "0124";//뒷숫자컨
        Invoke(str, 0);
    }

   
}
