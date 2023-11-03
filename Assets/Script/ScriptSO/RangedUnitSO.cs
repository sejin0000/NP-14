using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedUnitSO", menuName = "ScriptableObject/RangedUnitSO", order = int.MinValue)]
public class RangedUnitSO : UnitSO
{
    [Header("RangedUnitSO")]
    public int launchVolume;        // �ѹ��� �߻��� �߻緮
    public int bulletSpread;        // ź����
    public int bulletLifeTime;      // �Ѿ� �����ð�
    public Sprite BulletSprit;      // �Ѿ� ��������Ʈ
}
