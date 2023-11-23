using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct MonsterData
{
    public string monsterType;
    public int monsterCount;
}
public enum StageTypr
{
    Noemal,
    Boss
}


[Serializable]
public class StageMonsterData
{
    public StageTypr stageTypr;
    public List<MonsterData> monsters;
}

[CreateAssetMenu(fileName = "SingleStage", menuName = "StageSO/SingleStage", order = 1)]
public class StageSO : ScriptableObject
{
    [Header("StageInfo")]
    [SerializeField] private string stageName;
    [SerializeField] private int areaNum;
    [SerializeField] private int stageNum;

    public int AreaNum
    {
        get { return areaNum; }
        set
        {
            areaNum = value;
            UpdateStageName();
        }
    }

    public int StageNum
    {
        get { return stageNum; }
        set
        {
            stageNum = value;
            UpdateStageName();
        }
    }

    public string StageName
    {
        get { return stageName; }
        private set { stageName = value; }
    }

    private void OnValidate()
    {
        UpdateStageName();
    }

    private void UpdateStageName()
    {
        stageName = $"Stage_{areaNum}_{stageNum}";
    }

    [Header("MonsterInfo")]
    public StageMonsterData MonsterDatas;
}
