using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Open : MonoBehaviour
{
    public GameObject[] picklist;
    public static ChoiceSlot[] TempSlot = new ChoiceSlot[] { };

    public static Open Instance;

    void Start()
    {
        Instance = this;
        test(MakeStatBonusList.stat1);
    }
    void test(List<Istat> origin)// 고른게 안사리지는 타입 = 일반스탯
    {
        int Count = picklist.Length;
        //여기서 스탯증강인지 특수 증강인지에 따라투리스트할지 그냥 받을지
        List<Istat> list = origin.ToList();
        int rare = 1;

        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            Debug.Log(a);
            ChoiceSlot temp = picklist[i].GetComponent<ChoiceSlot>();
            temp.stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
    }

    void test2(List<Istat> origin) // 고른게 사라지는 타입 == 플레이변화 증강
    {
        int Count = picklist.Length;
        //여기서 스탯증강인지 특수 증강인지에 따라투리스트할지 그냥 받을지
        List<Istat> list = origin.ToList();

        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            Debug.Log(a);
            ChoiceSlot temp = picklist[i].GetComponent<ChoiceSlot>();
            temp.stat = list[a];
            picklist[i].gameObject.SetActive(true);

            list.RemoveAt(a);
        }
    }
    public void close()
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            picklist[i].gameObject.SetActive(false);

        }
    }


}
