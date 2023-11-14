using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class TestMakeAugmentListManager : MonoBehaviour//���� ����Ʈ�� �������
{
    public static TestMakeAugmentListManager Instance;

    //�̰� �³� �𸣰ڴµ� ������ ��ΰ� ��밡���ϴ� ����ƽ���� �ϳ��� �����ΰ�
    // �÷��� ��Ÿ�� ��ȭ ���� = �ʹ��� ������ ������������� �θ�����
    // 
    public List<IAugment> stat1 = new List<IAugment>();
    public List<IAugment> stat2 = new List<IAugment>();
    public List<IAugment> stat3 = new List<IAugment>();

    public List<SpecialAugment> SpecialAugment1 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment2 = new List<SpecialAugment>();
    public List<SpecialAugment> SpecialAugment3 = new List<SpecialAugment>();

    public List<SpecialAugment> test = new List<SpecialAugment>();
    public List<SpecialAugment> test2 = new List<SpecialAugment>();
    public List<SpecialAugment> Prototype = new List<SpecialAugment>();
    
    private GameObject playerObj;
    int playerType;
    public TestMakeAugmentListManager(GameObject player)
    {
        playerObj = player;

        Debug.Log("��������Ʈ����");
    }
    public void startset(GameObject gameobj)
    {
        playerObj = gameobj;
        makeLisk();
    }

    private void Awake()
    {
        stat1 = new List<IAugment>();
        stat2 = new List<IAugment>();
        stat3 = new List<IAugment>();

        SpecialAugment1 = new List<SpecialAugment>();
        SpecialAugment2 = new List<SpecialAugment>();
        SpecialAugment3 = new List<SpecialAugment>();

        test = new List<SpecialAugment>();
        test2 = new List<SpecialAugment>();
        Prototype = new List<SpecialAugment>();
        Instance = this;
        DontDestroyOnLoad(this);
        StatAugmentSetting(stat1, "stat1");
        StatAugmentSetting(stat2, "stat2");
        StatAugmentSetting(stat3, "stat3");
        //playerType = playerStatHandler.CharacterType;

        SpecialAugmentSetting(test, "Test111"); //@�������������׽�Ʈ�� 
        SpecialAugmentSetting(test2, "Test222"); //@�������������׽�Ʈ�� 
        SpecialAugmentSetting(Prototype, "test_Proto");

        SpecialAugmentSetting(SpecialAugment1, "special1");
        SpecialAugmentSetting(SpecialAugment2, "special2");
        SpecialAugmentSetting(SpecialAugment3, "special3");
    }
    private void Start()
    {
        ResultManager.Instance.startset();
    }
    public void makeLisk()
    {
        //int type = Player.
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum);
        //���������� �̰Ը³�?
        playerType = (int)classNum;
        Debug.Log("�̺κ� �ڵ� �����ؾ���");
        playerType = 1;
        string Ptype = "a";
        switch (playerType)
        {
            case 0:
                Ptype = "Soldier";
                break;

            case 1:
                Ptype = "Shotgun";
                break;

            case 2:
                Ptype = "Sniper";
                break;
        }
        SpecialAugmentSetting(SpecialAugment1, Ptype + "1");
        SpecialAugmentSetting(SpecialAugment2, Ptype + "2");
        SpecialAugmentSetting(SpecialAugment3, Ptype + "3");
    }
    public static void StatAugmentSetting(List<IAugment> list, string str)
    {
        List<Dictionary<string, object>> data = TestCSVReader.Read("CSVReader/" + str);
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
    public static void SpecialAugmentSetting(List<SpecialAugment> list, string str)// ���� ����Ʈ , �ҷ���csv���ϸ� csv������ �ҷ��� ����Ʈ�� �־���
    {
        List<Dictionary<string, object>> data = TestCSVReader.Read("CSVReader/" + str);

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

    public class TestCSVReader// csv ������ �ҷ�����
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
