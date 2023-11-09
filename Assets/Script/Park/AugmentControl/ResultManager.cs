using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
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


    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();
    public List<SpecialAugment> ProtoList = new List<SpecialAugment>();
    public GameObject Player;

    public PhotonView pv;

    public void startset(GameObject playerObj)
    {
        Player = playerObj;
        MainGameManager.Instance.OnGameEndedEvent += Result;
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
        //PickStatList(MakeAugmentListManager.stat1);//����1 


    }
    private void Start()
    {

    }
    public void startset()
    {
        stat1 = MakeAugmentListManager.Instance.stat1;
        stat2 = MakeAugmentListManager.Instance.stat2;
        stat3 = MakeAugmentListManager.Instance.stat3;
        SpecialAugment1 = MakeAugmentListManager.Instance.SpecialAugment1;
        SpecialAugment2 = MakeAugmentListManager.Instance.SpecialAugment2;
        SpecialAugment3 = MakeAugmentListManager.Instance.SpecialAugment3;
        ProtoList = MakeAugmentListManager.Instance.Prototype;
    }
    public void Result()
    {
        if (MainGameManager.Instance.stageData.isFarmingRoom)
        {
            //CallStatResult();�ְ� ������ �Ʒ��� �׽�Ʈ������ �׹��� �����������������־�� ������
            Debug.Log("�̺κ� ����");//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            CallProtoResult();//�ְ� �׽�Ʈ ����� ����
            //callStatTest();
        }
        else 
        {
            //CallSpecialResult();�̰� ���°� ���� �Ʒ��� ������Ÿ��
            CallProtoResult();//�ְ� �׽�Ʈ ����� ����
            //callStatTest();
        }
    }
    public void callStatTest() 
    {
        PickStatList(stat1);
    }
    public void CallProtoResult()//������Ÿ�Կ� ���� �θ��� ����Ʈ�� ������� �ʱ� ������ ����ִ�
    {
        PickSpecialList(ProtoList);
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
    public void testCallProtoResult()//������Ÿ�Կ� ���� �θ��� ����Ʈ�� ������� �ʱ� ������ ����ִ�
    {
        
        PickSpecialList(SpecialAugment1);
    }
    public void testCallStatResult()
    {
        int tier = MainGameManager.Instance.tier;
        //int tier = 1;
        Debug.Log("��������ؿ�������ؿ�������ؿ�������ؿ��������");
        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ Debug.Log("��������ؿ�������ؿ�������ؿ�������ؿ��������");
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
        pv.RPC("ready",RpcTarget.All);
        //���⿡ ���� ���ӸŴ��� �� 
    }

    [PunRPC]
    public void ready() 
    {
            MainGameManager.Instance.Ready++;
            Debug.Log($"���� ���� �� {MainGameManager.Instance.Ready}");
            MainGameManager.Instance.AllReady();
    }
}
