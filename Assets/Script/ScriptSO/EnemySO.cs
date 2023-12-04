using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/EnemySO", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [Header("EnemySO")]
    public string enemyName;      // 이름
    public EnemyType type;        // 적 유형(BT기반 행동 제어를 할거라 필요없으면 지우기)
    public float atk;             // 공격력
    public float hp;              // 체력

    public float bulletSpeed;     // 탄환 속도
    public float atkDelay;        // 공격 딜레이
    public float SpecialAttackDelay; // 특수 공격 딜레이
    public float patrolDelay;     // 순찰 딜레이
    public float chaseTime;       // 추적 유지시간
    public float groggyTiem;      // 기절 시간
    public float bulletLifeTime;  // 총알 유지 시간
    public float breathAttackDelay; // 브레스 공격 판정

    public float enemyMoveSpeed;  // 기본 이동속도
    public float enemyChaseSpeed; // 기본 추적속도


    public float viewAngle;       // 탐지 범위
    public float viewDistance;     // 탐지 길이
    public float attackRange;     // 공격 범위
    

    public int dropGold;          // 주는 재화의 양

    public int unitScale;         // 유닛 크기



    //보스

    public float bossPatternTime;
}
public enum EnemyType
{
    Melee,                                       // 근거리
    Ranged,                                      // 원거리
    Coward,                                      // 겁쟁이 : 일정 체력 이하면, 일정 시간 달아남
                                                 //TODO 힐러
}
