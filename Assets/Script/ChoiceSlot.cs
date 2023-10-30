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
    public Istat stat;
    int rare;
    

    [Range(1, 3)] int StatType;

    private void OnEnable()
    {
        Name.text = stat.Name;

        Info.text = stat.func;
        rare = stat.Rare;
        Image image = GetComponent<Image>();
        switch (rare)
        {
            case 1:
                image.color = new Color(image.color.r, image.color.g, image.color.b);
                break;

            case 2:
                break;

            case 3:
                break;
        }
    }

    void exit() 
    {
        gameObject.SetActive(false);
    }
    public void pick()
    {
        string str = stat.Code;
        Debug.Log(str);
        Invoke(str,0);

        Open.Instance.close();
    }
    private void S1()
    {
        Debug.Log("대충 스탯 더해준다는뜻");
    }
    void wjrdyd() 
    { 
        //+= stat.Atk
        //+= stat.Health;
        //+= stat.Speed;
        //+= stat.AtkSpeed
        //+= stat.BulletSpread;
        //+= stat.Cooltime;
        //+= stat.Critical;

        //if (stat is StatBonus) 
        //{
        //    StatBonus a = stat as StatBonus;
        //    += a.MaxBullet;
        //}
    }
    void TextUpdate(int i)
    {

           // Name[i].text = stat.Name;
           // Info[i].text = stat.func;
    }

}
