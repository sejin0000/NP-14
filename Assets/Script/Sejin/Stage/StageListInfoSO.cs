using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageListInfoSO", menuName = "StageListInfoSO", order = 1)]
public class StageListInfoSO : ScriptableObject
{
    [Header("스테이지 리스트")]
    public List<StageInfoSO> StagerList = new List<StageInfoSO>();
}
