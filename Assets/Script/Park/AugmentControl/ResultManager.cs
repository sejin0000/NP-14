using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour//vs코드
{
    public GameObject[] picklist;
    public static ChoiceSlot[] TempSlot = new ChoiceSlot[] { };
    public static ResultManager Instance;
    public StatAugment[] pickStat3;
    public IAugment[] pickSpecial3;
    List<SpecialAugment> tempList = new List<SpecialAugment>();
    bool IsStat;

    void Start()
    {
        Instance = this;// 싱글톤 
        //PickStatList(MakeAugmentListManager.stat1);//스탯1 
        int Count = picklist.Length;
       
    }
    public void testbtnstat() 
    {
        PickStatList(MakeAugmentListManager.stat1);
        Debug.Log($"{MakeAugmentListManager.stat1.Count}");
    }
    public void testbtnstat2()
    {
        PickSpecialList(MakeAugmentListManager.Instance.sniper1);
        Debug.Log($"{MakeAugmentListManager.Instance.sniper1.Count}");
    }
    public void testbtnstat3()
    {
        PickSpecialList(MakeAugmentListManager.Instance.test);
        //Debug.Log($"{MakeAugmentListManager.Instance.test.Count}");
    }
    void PickStatList(List<IAugment> origin)// 고른게 안사리지는 타입 = 일반스탯
    {
        int Count = picklist.Length;
        //여기서 스탯증강인지 특수 증강인지에 따라투리스트할지 그냥 받을지
        List<IAugment> list = origin.ToList();

        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            //Debug.Log(a);
            ChoiceSlot temp = picklist[i].GetComponent<ChoiceSlot>();
            temp.stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        IsStat = true;
        //uiUp();
    }

    void PickSpecialList(List<SpecialAugment> origin) // 고른게 사라지는 타입 == 플레이변화 증강 아직안만듬
    {
        int Count = picklist.Length;
        //여기서 스탯증강인지 특수 증강인지에 따라투리스트할지 그냥 받을지
        List<SpecialAugment> list = origin.ToList();
        tempList=origin;
        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            ChoiceSlot temp = picklist[i].GetComponent<ChoiceSlot>();
            temp.stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        IsStat = false;
        // 현재까지 중복 뽑기시 제거임 픽일때 제거를 해줘야함 
    }
    public void close()
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            if (picklist[i].GetComponent<ChoiceSlot>().Ispick && !IsStat)
            {
                int target= picklist[i].GetComponent<ChoiceSlot>().stat.Code;
                //리스트에서 이름 찾아서 제거
                int index = tempList.FindIndex(x => x.Code.Equals(target));
                tempList.Remove(tempList[index]);
            }
            picklist[i].gameObject.SetActive(false);

        }
    }


}
