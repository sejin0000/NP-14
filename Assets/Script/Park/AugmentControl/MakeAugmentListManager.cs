using Photon.Realtime;
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
    public  List<IAugment> stat1 = new List<IAugment>();
    public  List<IAugment> stat2 = new List<IAugment>();
    public  List<IAugment> stat3 = new List<IAugment>();

    public  List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public  List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public  List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();

    public List<SpecialAugment> test = new List<SpecialAugment>();
    public List<SpecialAugment> test2 = new List<SpecialAugment>();
    public List<SpecialAugment> Prototype = new List<SpecialAugment>();
    private GameObject playerObj;
    int playerType;
    public MakeAugmentListManager(GameObject player) 
    {
        playerObj = player;
    }

    private void Awake()
    {
        //Instance=this;
        //DontDestroyOnLoad(this);
        StatAugmentSetting(stat1, "stat1");
        StatAugmentSetting(stat2, "stat2");
        StatAugmentSetting(stat3, "stat3");
        //playerType = playerStatHandler.CharacterType;
        makeLisk();
        
        SpecialAugmentSetting(test, "Test111"); //@�������������׽�Ʈ�� 
        SpecialAugmentSetting(test2, "Test222"); //@�������������׽�Ʈ�� 
        SpecialAugmentSetting(Prototype, "Test222");
        SpecialAugmentSetting(Prototype, "Test111");

    }
    public void makeLisk() 
    {
        //int type = Player.
        string Ptype = "a" ;
        switch (playerType) //�̺κ� ��� �� ����
        {
            case 1:
                Ptype = "Sniper";
                break;

            case 2:
                Ptype = "Shotgun";
                break;

            case 3:
                Ptype = "Soldier";
                break;
        }
        SpecialAugmentSetting(SpecialAugment1, Ptype + "1");
        SpecialAugmentSetting(SpecialAugment2, Ptype + "2");
        SpecialAugmentSetting(SpecialAugment3, Ptype + "3");
    }
    void StatAugmentSetting(List<IAugment> list, string str)
    {
        List<Dictionary<string, object>> data = CSVReader.Read("CSVReader/" + str);
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
        List<Dictionary<string, object>> data = CSVReader.Read("CSVReader/" + str);

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
}
