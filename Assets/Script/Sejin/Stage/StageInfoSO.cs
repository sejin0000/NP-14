using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "StageInfoSO", menuName = "StageInfoSO", order = 1)]
public class StageInfoSO : ScriptableObject
{
    [Header("�̹� ���������� ���õǴ� ������ ����")]
    public List<MonsterSquadSO> MonsterSquadList = new List<MonsterSquadSO>();
}
