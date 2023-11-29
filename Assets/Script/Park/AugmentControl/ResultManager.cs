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

public class ResultManager : MonoBehaviour//vs�ڵ�
{
    public ChoiceSlot[] picklist;
    public static ResultManager Instance;
    List<SpecialAugment> tempList = new List<SpecialAugment>();
    bool IsStat;
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
        //if (MainGameManager.Instance != null) TO DEL��� ���� �κ� if�� ��ü�� ������ �ȴٰ� �Ǵܵ�
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
        //PickStatList(MakeAugmentListManager.stat1);//����1
        //if (MainGameManager.Instance != null) TO DEL �Ʒ��� �׽�Ʈ ���� �Ŵ������ �Ǵܵ�
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
        Debug.Log($"����1 ����{stat1.Count}");
        stat2 = MakeAugmentListManager.Instance.stat2;
        Debug.Log($"����2 ����{stat2.Count}");
        stat3 = MakeAugmentListManager.Instance.stat3;
        Debug.Log($"����3 ����{stat3.Count}");
        SpecialAugment1 = MakeAugmentListManager.Instance.SpecialAugment1;
        Debug.Log($"����1 ����{SpecialAugment1.Count}");
        SpecialAugment2 = MakeAugmentListManager.Instance.SpecialAugment2;
        Debug.Log($"����2 ����{SpecialAugment2.Count}");
        SpecialAugment3 = MakeAugmentListManager.Instance.SpecialAugment3;
        Debug.Log($"����3 ����{SpecialAugment3.Count}");
        ProtoList = MakeAugmentListManager.Instance.Prototype;
        Debug.Log("������ ������Ÿ�� �ּ�ó��");
    }
    public void SpecialResult()
    {
        if (!testsetting)//���� �׽�Ʈ�� �����ũ �׽�Ʈ true�� ������Ÿ�Ը���Ʈ��������
        {
            CallSpecialResult();
        }
        else 
        {
            CallProtoResult();//�ְ� �׽�Ʈ ����� ����
        }
    }
    public void CallProtoResult()//������Ÿ�Կ� ���� �θ��� ����Ʈ�� ������� �ʱ� ������ ����ִ�
    {
        PickSpecialList(ProtoList);
    }
    private int RandomTier() 
    {
        int tier = GameManager.Instance.curStage;
        int random = Random.Range(1, 11); // ���� ������ ����Ͽ� Ƽ�� ����ġ Ÿ��3�� �־��µ� �ʿ��� ���� 10-������ ��
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
        Debug.Log($"���� �� : {random}");
        if (random <= target1)
        {
            type = 1;
            Debug.Log($"��� Ÿ�� ����ġ {target1}��� Ƽ�� 1Ƽ�� ");
        }
        else if (random <= target1 + target2)
        {
            type = 2;
            Debug.Log($"��� Ÿ�� ����ġ {target1 + target2}��� Ƽ��  2Ƽ�� ");
        }
        else
        {
            type = 3;
            Debug.Log($"��� Ƽ�� 3Ƽ�� ");
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
  
    void PickStatList(List<IAugment> origin)// ���� �Ȼ縮���� Ÿ�� = �Ϲݽ���
    {
        int Count = picklist.Length;
        //���⼭ ������������ Ư�� ���������� ����������Ʈ���� �׳� ������
        List<IAugment> list = origin.ToList();

        for (int i = 0; i < Count; ++i)
        {
            int a = Random.Range(0, list.Count);
            picklist[i].stat = list[a];
            picklist[i].gameObject.SetActive(true);
            list.RemoveAt(a);
        }
        IsStat = true;// �̰ɷ� ����Ʈ���� �������� �״������ ������
    }

    void PickSpecialList(List<SpecialAugment> origin) // ���� ������� Ÿ�� == �÷��̺�ȭ ����
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
    public void close()//��Ͽ��� ����ٸ� ��� ui�� �ݾ���
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            if (picklist[i].Ispick && !IsStat)
            {
                int target= picklist[i].stat.Code;
                //����Ʈ���� �̸� ã�Ƽ� ����
                int index = tempList.FindIndex(x => x.Code.Equals(target));
                tempList.Remove(tempList[index]);
            }
            picklist[i].gameObject.SetActive(false);
            
        }
        //pv.RPC("ready",RpcTarget.All);
        //���⿡ ���� ���ӸŴ��� �� 
        if (!IsStat) 
        {
            ready();
        }

    }
    public void ready() 
    {
        Debug.Log("���� ����");
        GameManager.Instance.PV.RPC("EndPlayerCheck",RpcTarget.All);
            //gameManager.Ready++;
        
            //gameManager.AllReady();
    }
}
