using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour//vs�ڵ�
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
        Instance = this;// �̱��� 
        PickStatList(MakeAugmentListManager.stat1);//����1 
        int Count = picklist.Length;
       
    }
    public void testbtnstat() 
    {
        PickStatList(MakeAugmentListManager.stat1);
        Debug.Log($"{MakeAugmentListManager.stat1.Count}");
    }
    public void testbtnstat2()
    {
        PickSpecialList(MakeAugmentListManager.sniper1);
        Debug.Log($"{MakeAugmentListManager.sniper1.Count}");
    }
    void PickStatList(List<IAugment> origin)// ���� �Ȼ縮���� Ÿ�� = �Ϲݽ���
    {
        int Count = picklist.Length;
        //���⼭ ������������ Ư�� ���������� ����������Ʈ���� �׳� ������
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

    void PickSpecialList(List<SpecialAugment> origin) // ���� ������� Ÿ�� == �÷��̺�ȭ ���� �����ȸ���
    {
        int Count = picklist.Length;
        //���⼭ ������������ Ư�� ���������� ����������Ʈ���� �׳� ������
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
        // ������� �ߺ� �̱�� ������ ���϶� ���Ÿ� ������� 
    }
    public void close()
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            if (picklist[i].GetComponent<ChoiceSlot>().Ispick && !IsStat)
            {
                int target= picklist[i].GetComponent<ChoiceSlot>().stat.Code;
                //����Ʈ���� �̸� ã�Ƽ� ����
                int index = tempList.FindIndex(x => x.Code.Equals(target));
                tempList.Remove(tempList[index]);
            }
            picklist[i].gameObject.SetActive(false);

        }
    }
    void uiUp() 
    {
        int Count = picklist.Length;
        for (int i = 0; i < Count; ++i)
        {
            picklist[i].GetComponent<ChoiceSlot>().Name.text = pickSpecial3[i].Name;
            picklist[i].GetComponent<ChoiceSlot>().Info.text = pickSpecial3[i].func;
            int rare = pickSpecial3[i].Rare;
            Image image = picklist[i].GetComponent<Image>();
            switch (rare)
            {
                case 1:
                    image.color = new Color(205, 127, 50);//��
                    break;

                case 2:
                    image.color = new Color(192, 192, 192);//��
                    break;

                case 3:
                    image.color = new Color(255, 215, 0);//��
                    break;
            }
        }
    }


}
