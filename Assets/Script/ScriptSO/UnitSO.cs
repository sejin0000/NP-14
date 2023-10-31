using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitSO", menuName = "ScriptableObject/UnitSO", order = int.MinValue)]
public class UnitSO : ScriptableObject
{
    [Header("UnitSO")]
    public int atk;         // 공격력
    public int hp;          // 체력
    public int unitSpeed;   // 이동 속도
    public int atkSpeed;    // 공격 속도
    public int unitScale;   // 유닛 크기
}
