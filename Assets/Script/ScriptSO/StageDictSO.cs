using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName = "StageSODict", menuName = "StageSO/StageDict", order = 2)]
public class StageDictSO : ScriptableObject
{
    public List<StageSO> stageDict;
}

