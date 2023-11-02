using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedUnitSO", menuName = "ScriptableObject/RangedUnitSO", order = int.MinValue)]
public class RangedUnitSO : UnitSO
{
    [Header("RangedUnitSO")]
    public int launchVolume;        // 한번의 발사의 발사량
    public int BulletSpread;        // 탄퍼짐
}
