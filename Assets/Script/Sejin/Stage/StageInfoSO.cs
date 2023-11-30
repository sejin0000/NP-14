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
    [Header("�������� Ÿ��")]
    public StageType stageType;
    [Header("�̹� ���������� ���õǴ� ������ ����")]
    public List<MonsterSquadSO> MonsterSquadList = new List<MonsterSquadSO>();
}
