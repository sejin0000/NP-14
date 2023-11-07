using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour//vs�ڵ�
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

        //PickStatList(MakeAugmentListManager.stat1);//����1 
        GameManager1.Instance.OnStageEnd += Result;//@@@@@@@@@@@@@@@@@@@@@@@@@����ֱ�

    }
    public void Result()
    {
        if (GameManager1.Instance.IsNormalStage)
        {
            CallStatResult();
        }
        else 
        {
            //CallSpecialResult();�̰� ���°� ���� �Ʒ��� ������Ÿ��
            CallProtoResult();
        }
    }
    public void CallProtoResult()//������Ÿ�Կ� ���� �θ��� ����Ʈ�� ������� �ʱ� ������ ����ִ�
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
    }


}
