using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public GameObject MySpecialList;
    bool SeeNowMyList;
    PlayerInput playerinput;

    public bool readycheck;

    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();
    public List<SpecialAugment> ProtoList = new List<SpecialAugment>();
    public GameObject Player;

    public PhotonView pv;


    public MySpecialListSocket Socketprefab;
    public Transform ViewListContent;


    public bool statChance;
    public bool countDownCheck;
    public float pickTime;
    public TextMeshProUGUI time;

    bool testsetting;
    public bool SetActiveCheck;
    public void startset(GameObject playerObj)
    {
        Player = playerObj;
        IsStat = false;
        SetActiveCheck = false;
        countDownCheck = false;
        //if (MainGameManager.Instance != null) TO DEL사실 죽은 부분 if문 전체를 지워도 된다고 판단됨
        //{
        //    gameManager = MainGameManager.Instance;
        //    gameManager.OnGameEndedEvent += Result;
        //}
        GameManager.Instance.OnRoomEndEvent += CallStatResult;
        GameManager.Instance.OnStageEndEvent += SpecialResult;
        GameManager.Instance.OnBossStageEndEvent += SpecialResult;
        SeeNowMyList = false;
        pv = GetComponent<PhotonView>();
        playerinput = Player.GetComponent<PlayerInput>();
    }
    void Awake()
    {
        if (null == Instance)
        {
            Instance = this;

            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        testsetting = false;

    }
    public void StartSet()
    {
        stat1 = MakeAugmentListManager.Instance.stat1;
        stat2 = MakeAugmentListManager.Instance.stat2;
        stat3 = MakeAugmentListManager.Instance.stat3;

        SpecialAugment1 = MakeAugmentListManager.Instance.SpecialAugment1;
        SpecialAugment2 = MakeAugmentListManager.Instance.SpecialAugment2;
        SpecialAugment3 = MakeAugmentListManager.Instance.SpecialAugment3;

        GameManager.Instance.OnBossStageStartEvent += ReadyCheck;
        GameManager.Instance.OnStageStartEvent += ReadyCheck;
        ProtoList = MakeAugmentListManager.Instance.Prototype;
        Debug.Log("배포전 프로토타입 주석처리");
        statChance = false;
    }
    public void ReadyCheck() 
    {
        readycheck = false;
    }
    private void Update()
    {
        if (countDownCheck) 
        {
            pickTime -= Time.deltaTime;
            time.text = pickTime.ToString("F1");
            if (pickTime <= 0) 
            {
                picklist[0].pick();
            }
        }

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
        int random = Random.Range(1, 12); // 현재 층수에 비례하여 티어 가중치 타겟3도 있었는데 필요없어서 지움 10-나머지 값
        int target1 = 4;
        int target2 = 3;
        int target3 = 2;
        if (tier <= 6 && tier >= 4)
        {
            target1 = 3;
            target2 = 4;
            target3 = 2;
        }
        else if (tier >= 6)
        {
            target1 = 2;
            target2 = 3;
            target3 = 4;
        }
        int type = 0;
        if (random <= target1)
        {
            type = 1;
        }
        else if (random <= target1 + target2)
        {
            type = 2;
        }
        else if (random <= target1 + target2 + target3)
        {
            type = 3;
        }
        else 
        {
            type = 4;
        }
        return type;
    }
    public void CallStatResult() 
    {
        Invoke("CallStatResultWindow",0.5f);
    }
    public void CallStatResultWindow() 
    {
        int tier = RandomTier();
        if (tier <= 3)
        {
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
        else 
        {
            int chance = Random.Range(1, 11);
            statChance = true;
            if (chance > 6)
            {
                PickSpecialList(SpecialAugment2);
            }
            else 
            {
                PickSpecialList(SpecialAugment1);
            }
        }
    }
    public void CallSpecialResult()
    {
        if (GameManager.Instance.curStage < GameManager.Instance.stageListInfo.StagerList.Count -1)
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
            case 4:
                PickSpecialList(SpecialAugment3);
                break;

            default:
                PickSpecialList(SpecialAugment1);
                break;

        }
        }
    }
  
    void PickStatList(List<IAugment> origin)// 고른게 안사리지는 타입 = 일반스탯
    {
        playerinput.actions.FindAction("Attack").Disable();

        if (SetActiveCheck) 
        {
            picklist[0].pick();
        }
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
        SetActiveCheck = true;
    }

    void PickSpecialList(List<SpecialAugment> origin) // 고른게 사라지는 타입 == 플레이변화 증강
    {
        playerinput.actions.FindAction("Attack").Disable();

        if (SetActiveCheck)
        {
            picklist[0].pick();
        }
        int Count = picklist.Length;
        List<SpecialAugment> list = origin.ToList();
        tempList = origin;
        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            picklist[i].stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        if (GameManager.Instance.ClearStageCheck)
        {
            countDownCheck = true;
            time.gameObject.SetActive(true);
            pickTime = 30f;
        }
        SetActiveCheck = true;
        IsStat = false;

    }
    public void close()//목록에서 골랐다면 띄운 ui를 닫아줌
    {
        playerinput.actions.FindAction("Attack").Enable();

        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            if (picklist[i].Ispick && !IsStat)
            {
                int target= picklist[i].stat.Code;
                int index = tempList.FindIndex(x => x.Code.Equals(target));
                //리스트에서 이름 찾아서 제거
                MySpecialListSocket newSocket = Instantiate(Socketprefab);//
                newSocket.transform.SetParent(ViewListContent,false);//월드포지션 유지하면서 스케일이 유지될수가 있음 맞았음 월드포지션펄스하니까해결됨
                newSocket.Init(tempList[index].Name, tempList[index].func, tempList[index].Rare, tempList[index].Code);

                tempList.Remove(tempList[index]);
                if (tempList.Count <= 2) 
                {
                    SpecialAugment AllStat = new SpecialAugment("All Stat",999,"힘쌔고 강한 올스탯", 3);
                    tempList.Add(AllStat);
                }



            }
            picklist[i].gameObject.SetActive(false);
            
        }
        if (!IsStat && !statChance)
        {
            Ready();
        }
        countDownCheck = false;
        time.gameObject.SetActive(false);
        statChance = false;
    }
    public void Ready() 
    {
        if (!readycheck) 
        {
            GameManager.Instance.PV.RPC("EndPlayerCheck",RpcTarget.All);
            readycheck = true;
        }
    }
    public void OnOffGetList()
    {
        if (SeeNowMyList)
        {
            MySpecialList.SetActive(false);
            SeeNowMyList = false;
        }
        else 
        {
            MySpecialList.SetActive(true);
            SeeNowMyList = true;
        }
    }

}
