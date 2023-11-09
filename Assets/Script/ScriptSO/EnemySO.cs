using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/EnemySO", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [Header("EnemySO")]
    public string enemyName;   // 이름
    public EnemyType type;     // 적 유형(BT기반 행동 제어를 할거라 필요없으면 지우기)
    public float atk;            // 공격력
    public float hp;             // 체력
    public float atkdelay;     // 공격 속도

    public int patrolSpeed;    // 일반[순찰] 이동속도(느림)
    public int chaseSpeed;    // 추적 시 이동속도(빠름)
    public int runSpeed;       // 도주 시 속도(매우 빠름)
    public int detectionRange; // 탐지 범위
    public int attackRange;    // 공격 범위
    public int patrolRange;    // 순찰 범위

    public int dropGold;       // 주는 재화의 양

    public int unitScale;      // 유닛 크기

    public int actionTime;     // 일부 패턴 시간 계산용
}
public enum EnemyType
{
    Melee,                                       // 근거리
    Ranged,                                      // 원거리
    Coward,                                      // 겁쟁이 : 일정 체력 이하면, 일정 시간 달아남
                                                 //TODO 힐러
}
