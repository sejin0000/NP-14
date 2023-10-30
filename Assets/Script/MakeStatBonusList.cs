using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class MakeStatBonusList : MonoBehaviour
{
    public static List<Istat> stat1 = new List<Istat>();
    public static List<Istat> stat2 = new List<Istat>();
    public static List<Istat> stat3 = new List<Istat>();
    //StatBonus a =new;
    // Start is called before the first frame update
    private void Awake()
    {
        StatSetting(stat1, "stat1");
        //StatSetting(stat2, "stat2");
        //StatSetting(stat3, "stat3");

        Debug.Log($"스탯1리스트 목록 {stat1.Count}");
        Debug.Log($"스탯2리스트 목록 {stat2.Count}");
        Debug.Log($"스탯3리스트 목록 {stat3.Count}");
    }
    void Start()
    {


    }

    void Stat1Setting(List<StatBonus> list,string str) 
    {
        List<Dictionary<string, object>> data = CSVReader.Read(str);

        for (var i = 0; i < data.Count; i++)
        {
            
            StatBonus a = new StatBonus();
            a.Name = (string)data[i]["Name"];
            a.Atk = (int)data[i]["Atk"];
            a.Health = (int)data[i]["Health"];
            a.Speed = (int)data[i]["Speed"];
            a.AtkSpeed = (int)data[i]["AtkSpeed"];
            a.BulletSpread = (int)data[i]["BulletSpread"];
            a.Cooltime = (int)data[i]["Cooltime"];
            a.Critical = (int)data[i]["Critical"];
            a.func = (string)data[i]["Func"];

            list.Add(a);
        }
    }
    void StatSetting(List<Istat> list, string str)
    {
        List<Dictionary<string, object>> data = CSVReader.Read(str);
            for (var i = 0; i < data.Count; i++)
            {
                StatBonus a = new StatBonus();
                a.Name = (string)data[i]["Name"];
                a.Atk = (int)data[i]["Atk"];
                a.Health = (int)data[i]["Health"];
                a.Speed = (int)data[i]["Speed"];
                a.AtkSpeed = (int)data[i]["AtkSpeed"];
                a.BulletSpread = (int)data[i]["BulletSpread"];
                a.Cooltime = (int)data[i]["Cooltime"];
                a.Critical = (int)data[i]["Critical"];
                a.func = (string)data[i]["Func"];
                a.Code = (string)data[i]["Code"];
                a.Rare = (int)data[i]["Rare"];
            if (isHasKey(data[i], "MaxBullet"))
                {
                    a.MaxBullet= (int)data[i]["MaxBullet"];
                }
            list.Add(a);
        }

    }

    //void Stat2Setting(List<StatBonus> list, string str)
   // {
      //  List<Dictionary<string, object>> data = CSVReader.Read(str);

      //  for (var i = 0; i < data.Count; i++)
       // {
       //     StatBonus2 a = new StatBonus2();
       //     a.Name = (string)data[i]["Name"];
       //     a.Atk = (int)data[i]["Atk"];
       //     a.Health = (int)data[i]["Health"];
       //     a.Speed = (int)data[i]["Speed"];
       //     a.AtkSpeed = (int)data[i]["AtkSpeed"];
       //     a.BulletSpread = (int)data[i]["BulletSpread"];
//a.Cooltime = (int)data[i]["Cooltime"];
       //     a.Critical = (int)data[i]["Critical"];
      //      a.func = (string)data[i]["Func"];
      //      //
      //      a.MaxBullet = (int)data[i]["MaxBullet"];
     //       list.Add(a);

      //  }
    //}
    public class CSVReader
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
    public static bool isHasKey<K, V>(Dictionary<K, V> dict, K key)
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
}
 