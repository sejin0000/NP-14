using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public float total
    {
        get {if ((basic + added) * coefficient <= 0) { return 0.1f; }
        else { return (basic + added) * coefficient; } } }  //  �� ���� ��
    public float basic       { get; private set; }                               //�⺻ ���� ��
    public float added       { get; set; } = 0;                                  //�߰� ���� ��
    public float coefficient { get; set; } = 1;                                  //���� ��� ��

    public Stats(float basic) 
    {
        this.basic = basic;
    }    
}
