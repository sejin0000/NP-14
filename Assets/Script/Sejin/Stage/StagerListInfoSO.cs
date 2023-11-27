using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StagerListInfoSO", menuName = "StagerListInfoSO", order = 1)]
public class StagerListInfoSO : ScriptableObject
{
    [Header("스테이지 리스트")]
    public List<StageInfoSO> StagerList = new List<StageInfoSO>();
}
