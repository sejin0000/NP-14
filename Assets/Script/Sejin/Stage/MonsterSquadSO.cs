using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MonsterName
{
    Test_Enemy,
    Test_Boss,
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
