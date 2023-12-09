using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MonsterName
{
    Boss_Dragon,
    MiniDragon_Ranged_Enemy,
    MiniDragon_Melee_Enemy,
    Boss_Turtle,
    MiniTurtle_Ranged_Enemy,
    MiniTurtle_Melee_Enemy,
}
[CreateAssetMenu(fileName = "MonsterSquadSO", menuName = "MonsterSquadSO", order = 1)]

[System.Serializable]
public class MonsterSquadSO : ScriptableObject
{
    [Header("몬스터 종류")]
    public List<MonsterName> MonsterList = new List<MonsterName>();
    [Header("생성할 몬스터 수")]
    public int MonsterNum;
}
