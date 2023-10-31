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
                image.color = new Color(192,192,192);//실
                break;

            case 3:
                image.color = new Color(255, 215, 0);//골
                break;
        }
    }
    public void pick()
    {
        string str = stat.Code;
        //Debug.Log($"{str}");
        Invoke(str,0);
        // 이제 원본 리스트에서 제거해야함
        // 어떻게?
        Ispick = true;
        ResultManager.Instance.close();
    }
    private void S1()//
    {
        Debug.Log("대충 스탯 더해준다는뜻");
    }
}
