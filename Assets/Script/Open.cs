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
    void test(List<Istat> origin)// ���� �Ȼ縮���� Ÿ�� = �Ϲݽ���
    {
        int Count = picklist.Length;
        //���⼭ ������������ Ư�� ���������� ����������Ʈ���� �׳� ������
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

    void test2(List<Istat> origin) // ���� ������� Ÿ�� == �÷��̺�ȭ ����
    {
        int Count = picklist.Length;
        //���⼭ ������������ Ư�� ���������� ����������Ʈ���� �׳� ������
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
