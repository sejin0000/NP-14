using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "UnitSO", menuName = "ScriptableObject/UnitSO", order = int.MinValue)]
public class UnitSO : ScriptableObject
{
    [Header("UnitSO")]
    public int atk;         // ���ݷ�
    public int hp;          // ü��
    public float unitSpeed;   // �̵� �ӵ�
    public float atkSpeed;    // ���� �ӵ�
    public int unitScale;   // ���� ũ��

    //Ž�� ����, ���� ����
    //��Ʈ�� ����
}
