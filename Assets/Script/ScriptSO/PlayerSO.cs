using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = int.MinValue)]
public class PlayerSO : RangedUnitSO
{
    [Header("PlayerSO")]
    public int reloadCoolTime;      // ���� ��Ÿ��
    public int skillCoolTilm;       // ��ų ��Ÿ��
    public int rollCoolTime;        // ������ ��Ÿ��
    public int AmmoMax;             // ��ź��
    public int characterClass;      // ����
    public int critical;            // ũ��Ƽ��
}

enum characterClass
{
    rifle,       // ������
    shotgun,     // ����
    sniper       // ��������
}
