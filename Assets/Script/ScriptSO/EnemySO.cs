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
    public float atkdelay;        // ���� �ӵ�
    public float bulletLifeTime;  // �Ѿ� ���� �ð�

    public float enemyMoveSpeed;  // �⺻ �̵��ӵ�
    public float enemyChaseSpeed; // �⺻ �����ӵ�


    public float detectionRange; // Ž�� ����
    public float attackRange;    // ���� ����
    public float patrolRange;    // ���� ����

    public int dropGold;       // �ִ� ��ȭ�� ��

    public int unitScale;      // ���� ũ��

    public int actionTime;     // �Ϻ� ���� �ð� ����
}
public enum EnemyType
{
    Melee,                                       // �ٰŸ�
    Ranged,                                      // ���Ÿ�
    Coward,                                      // ������ : ���� ü�� ���ϸ�, ���� �ð� �޾Ƴ�
                                                 //TODO ����
}
