using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class MakeAugmentListManager : MonoBehaviour//증강 리스트를 만들어줌
{
    public static MakeAugmentListManager Instance;

    //이게 맞나 모르겠는데 스탯은 모두가 사용가능하니 스태틱으로 하나만 만들어두고
    // 플레이 스타일 변화 증강 = 너무김 앞으로 스페셜증강으로 부르겠음
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


        SpecialAugmentSetting(test, "Test111"); //@만든증강적용테스트용 
        SpecialAugmentSetting(test2, "Test222"); //@만든증강적용테스트용 

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
        //Debug.Log($"스탯1리스트 갯수 {stat1.Count}");
        //Debug.Log($"스탯1리스트 갯수 {stat2.Count}");
        //Debug.Log($"스탯1리스트 갯수 {stat3.Count}");
        //Debug.Log($"스나1리스트 갯수  {sniper1.Count}");
        //Debug.Log($"스나2리스트 갯수 {sniper2.Count}");
        //Debug.Log($"스나3리스트 갯수 {sniper3.Count}");
        //Debug.Log($"샷건1리스트 갯수 {Shotgun1.Count}");
        //Debug.Log($"샷건2리스트 갯수 {Shotgun2.Count}");
        //Debug.Log($"샷건3리스트 갯수 {Shotgun3.Count}");
        //Debug.Log($"솔져1리스트 갯수 {Soldier1.Count}");
        //Debug.Log($"솔져2리스트 갯수 {Soldier2.Count}");
        //Debug.Log($"솔져3리스트 갯수 {Soldier3.Count}");




    }
    void Start()
    {


    }

    
    //처음엔 인터페이스 뽑을 리스트 티어 1 = 직업증강티어1 + 공용 증강 티어 1 로 하기 위해 합칠때 같은 인터페이스로 합치려 했으나
    //같은 클래스를 사용하게 되어 굳이 인터페이스로 값을 할 필요가 없어짐 고치는게 좋을지 잘 모르겠음
    //리스트 티어 1 = 직업증강티어1 + 공용 증강 티어 1 이므로 리스트를 만들때 다른 직업의 증강이 들어가지 않도록 주의
    //뇌피셜상 처음 스탯 증강은 punrpc로 호스트가 , 각자 뽑을 증강리스트는 개인이 만들어도 되지않을까? 하는 생각이듬
    // 이유는 리스트에서 뽑아서 적용 시키는것만 punrpc로 동기화 하면 되지않을까 싶기 때문 -- 정확하지 않음 
    void StatAugmentSetting(List<IAugment> list, string str)// 넣을 리스트 , 불러올csv파일명 csv파일을 불러와 리스트에 넣어줌
    {//애도 지금 스탯 적용방법을 바꿔서 좀 쓸모없어짐 시간나면아래라 통합할 것
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
    //애는 그래서 그냥 스페셜증강만 받음
    void SpecialAugmentSetting(List<SpecialAugment> list,string str)// 넣을 리스트 , 불러올csv파일명 csv파일을 불러와 리스트에 넣어줌
    {
        List<Dictionary<string, object>> data = CSVReader.Read(str);

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

    public class CSVReader// csv 파일을 불러와줌
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
    public static bool isHasKey<K, V>(Dictionary<K, V> dict, K key)//딕셔너리 키값 체크용 인데 애도 필요없어질거같음 ;;;
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
