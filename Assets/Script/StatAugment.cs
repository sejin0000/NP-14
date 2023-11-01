using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAugment
{//���� ��� �̸� �ڵ� ���� ���
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
    //public float Atk { get; set; }
    //public float Health { get; set; }
    //public float Speed { get; set; }
    //public float AtkSpeed { get; set; }
    //public float BulletSpread { get; set; }
    //public float Cooltime { get; set; }
    //public float Critical { get; set; }
    //public float? MaxBullet { get; set; }
}
public class StatAugment : IAugment
{// ������ �ܼ� �տ��� �̱⶧���� ���Ȱ��� ��� ������ �ܼ� ���� �Լ��� ó���ϱ� ���� ��� ���� 
    public string Name { get; set; } = "";
    public float Atk { get; set; } = 0;
    public float Health { get; set; } = 0;
    public float Speed { get; set; } = 0;
    public float AtkSpeed { get; set; } = 0;
    public float BulletSpread { get; set; } = 0;
    public float Cooltime { get; set; } = 0;
    public float Critical { get; set; } = 0;
    public float? MaxBullet { get; set; }
    public int Code { get; set; }

    public string func { get; set; } = "test";
    public int Rare { get; set; }
}
public class SpecialAugment : IAugment
{ // ������ ȿ���� �ڵ�� ����� �̱� ������ �ʼ� ��� 4������ �ʿ� 
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
}