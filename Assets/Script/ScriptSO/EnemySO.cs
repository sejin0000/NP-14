using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/EnemySO", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [Header("EnemySO")]
    public string enemyName;      // �̸�
    public EnemyType type;        // �� ����(BT��� �ൿ ��� �ҰŶ� �ʿ������ �����)
    public float atk;             // ���ݷ�
    public float hp;              // ü��

    public float bulletSpeed;     // źȯ �ӵ�
    public float atkDelay;        // ���� ������
    public float SpecialAttackDelay; // Ư�� ���� ������
    public float patrolDelay;     // ���� ������
    public float chaseTime;       // ���� �����ð�
    public float groggyTiem;      // ���� �ð�
    public float bulletLifeTime;  // �Ѿ� ���� �ð�
    public float breathAttackDelay; // �극�� ���� ����

    public float enemyMoveSpeed;  // �⺻ �̵��ӵ�
    public float enemyChaseSpeed; // �⺻ �����ӵ�


    public float viewAngle;       // Ž�� ����
    public float viewDistance;     // Ž�� ����
    public float attackRange;     // ���� ����
    

    public int dropGold;          // �ִ� ��ȭ�� ��

    public int unitScale;         // ���� ũ��



    //����

    public float bossPatternTime;
}
public enum EnemyType
{
    Melee,                                       // �ٰŸ�
    Ranged,                                      // ���Ÿ�
    Coward,                                      // ������ : ���� ü�� ���ϸ�, ���� �ð� �޾Ƴ�
                                                 //TODO ����
}
