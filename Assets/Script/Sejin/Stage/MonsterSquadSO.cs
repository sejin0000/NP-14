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
    [Header("���� ����")]
    public List<MonsterName> MonsterList = new List<MonsterName>();
    [Header("������ ���� ��")]
    public int MonsterNum;
}
