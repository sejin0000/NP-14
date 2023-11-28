using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    normalStage,
    bossStage,
}


[System.Serializable]
[CreateAssetMenu(fileName = "StageInfoSO", menuName = "StageInfoSO", order = 1)]
public class StageInfoSO : ScriptableObject
{
    [Header("스테이지 타입")]
    public StageType stageType;
    [Header("이번 스테이지에 세팅되는 스쿼드 종류")]
    public List<MonsterSquadSO> MonsterSquadList = new List<MonsterSquadSO>();
}
