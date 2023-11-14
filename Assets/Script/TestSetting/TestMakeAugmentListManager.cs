using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class TestMakeAugmentListManager : MonoBehaviour//증강 리스트를 만들어줌
{
    public static TestMakeAugmentListManager Instance;

    //이게 맞나 모르겠는데 스탯은 모두가 사용가능하니 스태틱으로 하나만 만들어두고
    // 플레이 스타일 변화 증강 = 너무김 앞으로 스페셜증강으로 부르겠음
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

        Debug.Log("증강리스트생성");
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

        SpecialAugmentSetting(test, "Test111"); //@만든증강적용테스트용 
        SpecialAugmentSetting(test2, "Test222"); //@만든증강적용테스트용 
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
        //구별값으로 이게맞나?
        playerType = (int)classNum;
        Debug.Log("이부분 코드 수정해야함");
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
    //애는 그래서 그냥 스페셜증강만 받음
    public static void SpecialAugmentSetting(List<SpecialAugment> list, string str)// 넣을 리스트 , 불러올csv파일명 csv파일을 불러와 리스트에 넣어줌
    {
        List<Dictionary<string, object>> data = TestCSVReader.Read("CSVReader/" + str);

        for (var i = 0; i < data.Count; i++)
        {
            SpecialAugment a = new SpecialAugment();
            a.Name = (string)data[i]["Name"];
            a.func = (string)data[i]["Func"];
            a.Code = (int)data[i]["Code"];
            a.Rare = (int)data[i]["Rare"];
            //Debug.Log($"이름{a.Name}번호 {i}");
            list.Add(a);
        }

    }

    public class TestCSVReader// csv 파일을 불러와줌
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
