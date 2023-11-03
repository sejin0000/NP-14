using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedUnitSO", menuName = "ScriptableObject/RangedUnitSO", order = int.MinValue)]
public class RangedUnitSO : UnitSO
{
    [Header("RangedUnitSO")]
    public int launchVolume;        // 한번의 발사의 발사량
    public int bulletSpread;        // 탄퍼짐
    public int bulletLifeTime;      // 총알 유지시간
    public Sprite BulletSprit;      // 총알 스프라이트
}
