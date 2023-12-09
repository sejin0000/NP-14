using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitSO", menuName = "ScriptableObject/UnitSO", order = int.MinValue)]
public class UnitSO : ScriptableObject
{
    [Header("UnitSO")]
    public int atk;         // 공격력
    public int hp;          // 체력
    public float unitSpeed;   // 이동 속도
    public float atkSpeed;    // 공격 속도
    public int unitScale;   // 유닛 크기

    //탐지 범위, 공격 범위
    //패트롤 범위
}
