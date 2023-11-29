using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static MainGameManager;

public class ResultManager : MonoBehaviour//vs코드
{
    public ChoiceSlot[] picklist;
    public static ResultManager Instance;
    List<SpecialAugment> tempList = new List<SpecialAugment>();
    private bool IsStat;
    public List<IAugment> stat1;
    public List<IAugment> stat2;
    public List<IAugment> stat3;

    //public MainGameManager gameManager;
    //public TestGameManager gameManager;
    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();
    public List<SpecialAugment> ProtoList = new List<SpecialAugment>();
    public GameObject Player;

    public PhotonView pv;

    bool testsetting;

    public void startset(GameObject playerObj)
    {
        Player = playerObj;
        IsStat = false; 
        //if (MainGameManager.Instance != null) TO DEL사실 죽은 부분 if문 전체를 지워도 된다고 판단됨
        //{
        //    gameManager = MainGameManager.Instance;
        //    gameManager.OnGameEndedEvent += Result;
        //}
        GameManager.Instance.OnRoomEndEvent += CallStatResult;
        GameManager.Instance.OnStageEndEvent += SpecialResult;

        pv = GetComponent<PhotonView>();
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
        testsetting = false;
        //PickStatList(MakeAugmentListManager.stat1);//스탯1
        //if (MainGameManager.Instance != null) TO DEL 아래도 테스트 게임 매니저라고 판단됨
        //{
        //    gameManager = MainGameManager.Instance;
        //}
        //if (TestGameManager.Instance != null)
        //{
        //    gameManager = TestGameManager.Instance;
        //}

    }
    public void StartSet()
    {
        stat1 = MakeAugmentListManager.Instance.stat1;
        Debug.Log($"스탯1 개수{stat1.Count}");
        stat2 = MakeAugmentListManager.Instance.stat2;
        Debug.Log($"스탯2 개수{stat2.Count}");
        stat3 = MakeAugmentListManager.Instance.stat3;
        Debug.Log($"스탯3 개수{stat3.Count}");
        SpecialAugment1 = MakeAugmentListManager.Instance.SpecialAugment1;
        Debug.Log($"증강1 개수{SpecialAugment1.Count}");
        SpecialAugment2 = MakeAugmentListManager.Instance.SpecialAugment2;
        Debug.Log($"증강2 개수{SpecialAugment2.Count}");
        SpecialAugment3 = MakeAugmentListManager.Instance.SpecialAugment3;
        Debug.Log($"증강3 개수{SpecialAugment3.Count}");
        ProtoList = MakeAugmentListManager.Instance.Prototype;
        Debug.Log("배포전 프로토타입 주석처리");
    }
    public void SpecialResult()
    {
        if (!testsetting)//증강 테스트용 어웨이크 테스트 true시 프로토타입리절트만가져옴
        {
            CallSpecialResult();
        }
        else 
        {
            CallProtoResult();//애가 테스트 스페셜 증강
        }
    }
    public void CallProtoResult()//프로토타입용 변수 부르는 리스트가 만들어진 초기 버전만 들어있다
    {
        PickSpecialList(ProtoList);
    }
    private int RandomTier() 
    {
        int tier = GameManager.Instance.curStage;
        int random = Random.Range(1, 11); // 현재 층수에 비례하여 티어 가중치 타겟3도 있었는데 필요없어서 지움 10-나머지 값
        int target1 = 5;
        int target2 = 3;
        if (tier <= 6 && tier >= 4)
        {
            target1 = 3;
            target2 = 5;
        }
        else if (tier >= 6)
        {
            target1 = 2;
            target2 = 3;
        }
        int type = 0;
        Debug.Log($"랜덤 수 : {random}");
        if (random <= target1)
        {
            type = 1;
            Debug.Log($"대상 타겟 가중치 {target1}대상 티어 1티어 ");
        }
        else if (random <= target1 + target2)
        {
            type = 2;
            Debug.Log($"대상 타겟 가중치 {target1 + target2}대상 티어  2티어 ");
        }
        else
        {
            type = 3;
            Debug.Log($"대상 티어 3티어 ");
        }
        return type;
    }
    public void CallStatResult() 
    {

        int tier = RandomTier();

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
    public void CallSpecialResult()
    {
        int tier = RandomTier();
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
        Debug.Log($"이즈스탯 노말 트루여야함 {IsStat}");
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
        Debug.Log($"이즈스탯 스페셜 펄스여야함{IsStat}");
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
        Debug.Log($"이즈스탯{IsStat}");
        //pv.RPC("ready",RpcTarget.All);
        //여기에 메인 게임매니저 콜 
        if (IsStat)
        {
            Debug.Log("스탯이라 안들어옴");
        }
        else 
        {
            Debug.Log("레디 들어옴");
            ready();
        }

    }
    public void ready() 
    {
        Debug.Log("레디 들어옴");
        GameManager.Instance.PV.RPC("EndPlayerCheck",RpcTarget.All);
            //gameManager.Ready++;
        
            //gameManager.AllReady();
    }
}
