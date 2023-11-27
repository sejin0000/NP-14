using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagerListInfoSO", menuName = "StagerListInfoSO", order = 1)]
public class StagerListInfoSO : ScriptableObject
{
    [Header("�������� ����Ʈ")]
    public List<StageInfoSO> StagerList = new List<StageInfoSO>();
}
