using Photon.Realtime;
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
    public List<IAugment> stat1;
    public List<IAugment> stat2;
    public List<IAugment> stat3;


    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();
    public List<SpecialAugment> ProtoList = new List<SpecialAugment>();
    public GameObject Player;

    public void startset(GameObject playerObj)
    {
        Player = playerObj;
    }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        //PickStatList(MakeAugmentListManager.stat1);//스탯1 
        //MainGameManager.Instance.OnGameEndedEvent += Result;

    }
    public void startset()
    {
        Debug.Log("시작@@@@@@@@@@@@@@@@@@@@@@@");
        stat1 = MakeAugmentListManager.Instance.stat1;
        Debug.Log($"스탯1 갯수{stat1.Count}");
        stat2 = MakeAugmentListManager.Instance.stat2;
        Debug.Log($"스탯2 갯수{stat2.Count}");
        stat3 = MakeAugmentListManager.Instance.stat3;
        Debug.Log($"스탯3 갯수{stat3.Count}");
        SpecialAugment1 = MakeAugmentListManager.Instance.SpecialAugment1;
        Debug.Log($"증강1 갯수{SpecialAugment1.Count}");
        SpecialAugment2 = MakeAugmentListManager.Instance.SpecialAugment2;
        Debug.Log($"증강2 갯수{SpecialAugment2.Count}");
        SpecialAugment3 = MakeAugmentListManager.Instance.SpecialAugment3;
        Debug.Log($"증강3 갯수{SpecialAugment3.Count}");
        ProtoList = MakeAugmentListManager.Instance.Prototype;
        Debug.Log($"프로토타입증강 갯수{ProtoList.Count}");
    }
    public void Result()
    {
        if (MainGameManager.Instance.stageData.isFarmingRoom)
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
        int tier = MainGameManager.Instance.tier;
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
    public void testCallProtoResult()//프로토타입용 변수 부르는 리스트가 만들어진 초기 버전만 들어있다
    {
        
        PickSpecialList(SpecialAugment1);
    }
    public void testCallStatResult()
    {
        int tier = MainGameManager.Instance.tier;
        //int tier = 1;
        Debug.Log("여기수정해여기수정해여기수정해여기수정해여기수정해");
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ Debug.Log("여기수정해여기수정해여기수정해여기수정해여기수정해");
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
        int tier = MainGameManager.Instance.tier;
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
