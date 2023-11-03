using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = int.MinValue)]
public class PlayerSO : RangedUnitSO
{
    [Header("PlayerSO")]
    public float reloadCoolTime;       // ���� ��Ÿ��
    public int skillCoolTime;        // ��ų ��Ÿ��
    public int rollCoolTime;         // ������ ��Ÿ��
    public int ammoMax;              // ��ź��
    public int characterClass;       // ����
    public int critical;             // ũ��Ƽ��
    public SpriteLibraryAsset playerSprite;// �÷��̾� ��������Ʈ
    public SpriteLibraryAsset weaponSprite;// ���� ��������Ʈ
}

enum characterClass
{
    rifle,       // ������
    shotgun,     // ����
    sniper       // ��������
}
