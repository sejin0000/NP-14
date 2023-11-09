using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySO", menuName = "ScriptableObject/EnemySO", order = int.MinValue)]
public class EnemySO : ScriptableObject
{
    [Header("EnemySO")]
    public string enemyName;   // �̸�
    public EnemyType type;     // �� ����(BT��� �ൿ ��� �ҰŶ� �ʿ������ �����)
    public float atk;            // ���ݷ�
    public float hp;             // ü��
    public float atkdelay;     // ���� �ӵ�

    public int patrolSpeed;    // �Ϲ�[����] �̵��ӵ�(����)
    public int chaseSpeed;    // ���� �� �̵��ӵ�(����)
    public int runSpeed;       // ���� �� �ӵ�(�ſ� ����)
    public int detectionRange; // Ž�� ����
    public int attackRange;    // ���� ����
    public int patrolRange;    // ���� ����

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
