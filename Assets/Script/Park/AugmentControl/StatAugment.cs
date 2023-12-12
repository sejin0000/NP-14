using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IAugment
{//���� ��� �̸� �ڵ� ���� ���
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
}
public class StatAugment : IAugment
{// ������ �ܼ� �տ��� �̱⶧���� ���Ȱ��� ��� ������ �ܼ� ���� �Լ��� ó���ϱ� ���� ��� ���� 
    public string Name { get; set; } = "";
    public int Code { get; set; }
    public string func { get; set; } = "";
    public int Rare { get; set; }
}
public class SpecialAugment : IAugment
{ // ������ ȿ���� �ڵ�� ����� �̱� ������ �ʼ� ��� 4������ �ʿ� 
    public string Name { get; set; }
    public int Code { get; set; }
    public string func { get; set; }
    public int Rare { get; set; }
    public SpecialAugment(string name, int code, string func, int rare)
    {
        Name = name;
        Code = code;
        this.func = func;
        Rare = rare;
    }
}