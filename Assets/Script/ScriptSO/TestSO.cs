using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Test", menuName = "Test", order = 1)]
public class TestSO : ScriptableObject
{
    [Header("PrefabTest")]
    public string aaa;
    public GameObject Prefab;
}
