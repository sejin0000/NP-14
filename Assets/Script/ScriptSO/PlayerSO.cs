using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = int.MinValue)]
public class PlayerSO : RangedUnitSO
{
    [Header("PlayerSO")]
    public int reloadCoolTime;      // 장전 쿨타입
    public int skillCoolTilm;       // 스킬 쿨타입
    public int rollCoolTime;        // 구르기 쿨타입
    public int AmmoMax;             // 장탄수
    public int characterClass;      // 직업
    public int critical;            // 크리티컬
}

enum characterClass
{
    rifle,       // 라이플
    shotgun,     // 샷건
    sniper       // 스나이퍼
}
