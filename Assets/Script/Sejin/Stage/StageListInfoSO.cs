using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageListInfoSO", menuName = "StageListInfoSO", order = 1)]
public class StageListInfoSO : ScriptableObject
{
    [Header("�������� ����Ʈ")]
    public List<StageInfoSO> StagerList = new List<StageInfoSO>();
}
