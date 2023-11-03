using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/EnemySO", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [Header("EnemySO")]
    public string enemyName;   // �̸�
    public EnemyType type;     // �� ����(BT��� �ൿ ��� �ҰŶ� �ʿ������ �����)
    public int atk;            // ���ݷ�
    public int hp;             // ü��
    public float atkSpeed;     // ���� �ӵ�

    public int wanderSpeed;    // �Ϲ�[����] �̵��ӵ�(����)
    public int pursitSpeed;    // ���� �� �̵��ӵ�(����)
    public int detectionRange; // Ž�� ����
    public int attackRange;    // ���� ����
    public int patrolRange;    // ���� ����

    public int actionSpeed;    // �ൿ �ӵ�(���� ��)
    public int dropGold;       // �ִ� ��ȭ�� ��

    public int unitScale;      // ���� ũ��

    public int actionTime;     // �Ϻ� ���� �ð� ����
}
public enum EnemyType
{
    Melee,                                       // �ٰŸ�
    Ranged,                                      // ���Ÿ�
    Coward,                                      // ������ : ���� ü�� ���ϸ�, ���� �ð� �޾Ƴ�
}
