using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class MakeAugmentListManager : MonoBehaviour//���� ����Ʈ�� �������
{
    public static MakeAugmentListManager Instance;

    //�̰� �³� �𸣰ڴµ� ������ ��ΰ� ��밡���ϴ� ����ƽ���� �ϳ��� �����ΰ�
    // �÷��� ��Ÿ�� ��ȭ ���� = �ʹ��� ������ ������������� �θ�����
    // 
    public static List<IAugment> stat1 = new List<IAugment>();
    public static List<IAugment> stat2 = new List<IAugment>();
    public static List<IAugment> stat3 = new List<IAugment>();

    public List<SpecialAugment> sniper1 = new List<SpecialAugment>();
    public List<SpecialAugment> sniper2 = new List<SpecialAugment>();
    public List<SpecialAugment> sniper3 = new List<SpecialAugment>();

    public List<SpecialAugment> Soldier1 = new List<SpecialAugment>();
    public List<SpecialAugment> Soldier2 = new List<SpecialAugment>();
    public List<SpecialAugment> Soldier3 = new List<SpecialAugment>();

    public List<SpecialAugment> Shotgun1 = new List<SpecialAugment>();
    public List<SpecialAugment> Shotgun2 = new List<SpecialAugment>();
    public List<SpecialAugment> Shotgun3 = new List<SpecialAugment>();

    public List<SpecialAugment> test = new List<SpecialAugment>();
    public List<SpecialAugment> test2 = new List<SpecialAugment>();
    //StatBonus a =new;
    // Start is called before the first frame update
    private void Awake()
    {
        Instance=this;
        StatAugmentSetting(stat1, "stat1");
        StatAugmentSetting(stat2, "stat2");
        StatAugmentSetting(stat3, "stat3");


        SpecialAugmentSetting(sniper1, "Sniper1");
        SpecialAugmentSetting(sniper1, "All1");


        SpecialAugmentSetting(test, "Test111"); //@�������������׽�Ʈ�� 
        SpecialAugmentSetting(test2, "Test222"); //@�������������׽�Ʈ�� 

        SpecialAugmentSetting(sniper2, "Sniper2");
        SpecialAugmentSetting(sniper2, "All2");

        //SpecialAugmentSetting(sniper3, "Sniper3");
        //SpecialAugmentSetting(sniper3, "All3");

        //SpecialAugmentSetting(Shotgun1, "Shotgun1");
        //SpecialAugmentSetting(Shotgun1, "All1");

        //SpecialAugmentSetting(Shotgun2, "Shotgun2");
        //SpecialAugmentSetting(Shotgun2, "All2");

        //SpecialAugmentSetting(Shotgun3, "Shotgun3");
        //SpecialAugmentSetting(Shotgun3, "All3");

        //SpecialAugmentSetting(Soldier1, "Soldier1");
        //SpecialAugmentSetting(Soldier1, "All1");

        //SpecialAugmentSetting(Soldier2, "Soldier2");
        //SpecialAugmentSetting(Soldier2, "All2");

        //SpecialAugmentSetting(Soldier3, "Soldier3");
        //SpecialAugmentSetting(Soldier3, "All3");
        //for (int i = 0; i < sniper1.Count; ++i) 
        //{
        //Debug.Log($"{ sniper1[i].Name}");
        //}
        //Debug.Log($"����1����Ʈ ���� {stat1.Count}");
        //Debug.Log($"����1����Ʈ ���� {stat2.Count}");
        //Debug.Log($"����1����Ʈ ���� {stat3.Count}");
        //Debug.Log($"����1����Ʈ ����  {sniper1.Count}");
        //Debug.Log($"����2����Ʈ ���� {sniper2.Count}");
        //Debug.Log($"����3����Ʈ ���� {sniper3.Count}");
        //Debug.Log($"����1����Ʈ ���� {Shotgun1.Count}");
        //Debug.Log($"����2����Ʈ ���� {Shotgun2.Count}");
        //Debug.Log($"����3����Ʈ ���� {Shotgun3.Count}");
        //Debug.Log($"����1����Ʈ ���� {Soldier1.Count}");
        //Debug.Log($"����2����Ʈ ���� {Soldier2.Count}");
        //Debug.Log($"����3����Ʈ ���� {Soldier3.Count}");




    }
    void Start()
    {


    }

    
    //ó���� �������̽� ���� ����Ʈ Ƽ�� 1 = ��������Ƽ��1 + ���� ���� Ƽ�� 1 �� �ϱ� ���� ��ĥ�� ���� �������̽��� ��ġ�� ������
    //���� Ŭ������ ����ϰ� �Ǿ� ���� �������̽��� ���� �� �ʿ䰡 ������ ��ġ�°� ������ �� �𸣰���
    //����Ʈ Ƽ�� 1 = ��������Ƽ��1 + ���� ���� Ƽ�� 1 �̹Ƿ� ����Ʈ�� ���鶧 �ٸ� ������ ������ ���� �ʵ��� ����
    //���ǼȻ� ó�� ���� ������ punrpc�� ȣ��Ʈ�� , ���� ���� ��������Ʈ�� ������ ���� ����������? �ϴ� �����̵�
    // ������ ����Ʈ���� �̾Ƽ� ���� ��Ű�°͸� punrpc�� ����ȭ �ϸ� ���������� �ͱ� ���� -- ��Ȯ���� ���� 
    void StatAugmentSetting(List<IAugment> list, string str)// ���� ����Ʈ , �ҷ���csv���ϸ� csv������ �ҷ��� ����Ʈ�� �־���
    {//�ֵ� ���� ���� �������� �ٲ㼭 �� ��������� �ð�����Ʒ��� ������ ��
        List<Dictionary<string, object>> data = CSVReader.Read(str);
            for (var i = 0; i < data.Count; i++)
            {
                StatAugment a = new StatAugment();
                a.Name = (string)data[i]["Name"];
                a.func = (string)data[i]["Func"];
                a.Code = (int)data[i]["Code"];
                a.Rare = (int)data[i]["Rare"];
            list.Add(a);
        }

    }
    //�ִ� �׷��� �׳� ����������� ����
    void SpecialAugmentSetting(List<SpecialAugment> list,string str)// ���� ����Ʈ , �ҷ���csv���ϸ� csv������ �ҷ��� ����Ʈ�� �־���
    {
        List<Dictionary<string, object>> data = CSVReader.Read(str);

        for (var i = 0; i < data.Count; i++)
        {
            SpecialAugment a = new SpecialAugment();
            a.Name = (string)data[i]["Name"];
            a.func = (string)data[i]["Func"];
            a.Code = (int)data[i]["Code"];
            a.Rare = (int)data[i]["Rare"];
            //Debug.Log($"�̸�{a.Name}��ȣ {i}");
            list.Add(a);
        }

    }

    public class CSVReader// csv ������ �ҷ�����
    {
        static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
        static char[] TRIM_CHARS = { '\"' };

        public static List<Dictionary<string, object>> Read(string file)
        {
            var list = new List<Dictionary<string, object>>();
            TextAsset data = Resources.Load(file) as TextAsset;

            var lines = Regex.Split(data.text, LINE_SPLIT_RE);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], SPLIT_RE);
            for (var i = 1; i < lines.Length; i++)
            {

                var values = Regex.Split(lines[i], SPLIT_RE);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                    object finalvalue = value;
                    int n;
                    float f;
                    if (int.TryParse(value, out n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add(entry);
            }
            return list;
        }
    }
    public static bool isHasKey<K, V>(Dictionary<K, V> dict, K key)//��ųʸ� Ű�� üũ�� �ε� �ֵ� �ʿ�������Ű��� ;;;
    {
        foreach (KeyValuePair<K, V> kvp in dict)
        {
            if (kvp.Key.Equals(key))
            {
                return true;
            }
        }
        return false;
    }
    //public List<List<SpecialAugment>> MakeSoldier1List()
    //{
    //List<List<SpecialAugment>> returnList = new List<List<SpecialAugment>>();
    //  returnList.Add(SpecialAugmentSetting("Soldier1"));
    //   returnList.Add(SpecialAugmentSetting("Soldier2"));
    //    returnList.Add(SpecialAugmentSetting("Soldier3"));
    //    return returnList;
    //}
}
