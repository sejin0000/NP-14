using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss_TurtleSO", menuName = "ScriptableObject/Boss_TurtleSO", order = int.MinValue)]
public class Boss_Turtle_SO : EnemySO
{
    public float rollingCooltime;
    public float thornTornadoCoolTime;
    public float missileCoolTime;
    public float rollingDamge;
    public int endRollCount;
}
