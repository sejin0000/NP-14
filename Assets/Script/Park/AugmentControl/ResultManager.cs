using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour//vs코드
{
    public ChoiceSlot[] picklist;
    public static ResultManager Instance;
    List<SpecialAugment> tempList = new List<SpecialAugment>();
    bool IsStat;
    public List<IAugment> stat1 = new List<IAugment>();
    public List<IAugment> stat2 = new List<IAugment>();
    public List<IAugment> stat3 = new List<IAugment>();

    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();
    public List<SpecialAugment> ProtoList = new List<SpecialAugment>();
    public GameObject Player;
    public MakeAugmentListManager ListManager;

    public ResultManager(GameObject playerObj)
    {
        Player = playerObj;
    }
    void Start()
    {
        if (null == Instance)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        MakeAugmentListManager listManager = new MakeAugmentListManager(Player);


        stat1 = ListManager.stat1;
        stat2 = ListManager.stat2;
        stat3 = ListManager.stat3;
        SpecialAugment1 = ListManager.SpecialAugment1;
        SpecialAugment2 = ListManager.SpecialAugment2;
        SpecialAugment3 = ListManager.SpecialAugment3;
        ProtoList = ListManager.Prototype;

        //PickStatList(MakeAugmentListManager.stat1);//스탯1 
        GameManager1.Instance.OnStageEnd += Result;//@@@@@@@@@@@@@@@@@@@@@@@@@여기넣기

    }
    public void Result()
    {
        if (GameManager1.Instance.IsNormalStage)
        {
            CallStatResult();
        }
        else 
        {
            //CallSpecialResult();이거 쓰는게 정상 아래가 프로토타입
            CallProtoResult();
        }
    }
    public void CallProtoResult()//프로토타입용 변수 부르는 리스트가 만들어진 초기 버전만 들어있다
    {
        PickSpecialList(SpecialAugment1);
    }
    public void CallStatResult() 
    {
        int tier = GameManager1.Instance.tier;
            switch (tier) 
            {
                case 1:
                    PickStatList(stat1);
                    break;

                case 2:
                    PickStatList(stat2);
                    break;

                case 3:
                    PickStatList(stat3);
                    break;

            }
    }
    public void CallSpecialResult()//
    {
        int tier = GameManager1.Instance.tier;
        switch (tier)
        {
            case 1:
                PickSpecialList(SpecialAugment1);
                break;

            case 2:
                PickSpecialList(SpecialAugment2);
                break;

            case 3:
                PickSpecialList(SpecialAugment3);
                break;

        }
    }
    public void testbtnstat3()
    {
        PickSpecialList(MakeAugmentListManager.Instance.test);
        //Debug.Log($"{MakeAugmentListManager.Instance.test.Count}");
    }
    public void testbtnstat4()
    {
        PickSpecialList(MakeAugmentListManager.Instance.test2);
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
            picklist[i].stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        IsStat = true;// 이걸로 리스트에서 제거인지 그대로인지 구별함
    }

    void PickSpecialList(List<SpecialAugment> origin) // 고른게 사라지는 타입 == 플레이변화 증강
    {
        int Count = picklist.Length;
        List<SpecialAugment> list = origin.ToList();
        tempList=origin;
        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            picklist[i].stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        IsStat = false;
    }
    public void close()//목록에서 골랐다면 띄운 ui를 닫아줌
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            if (picklist[i].Ispick && !IsStat)
            {
                int target= picklist[i].stat.Code;
                //리스트에서 이름 찾아서 제거
                int index = tempList.FindIndex(x => x.Code.Equals(target));
                tempList.Remove(tempList[index]);
            }
            picklist[i].gameObject.SetActive(false);

        }
    }


}
