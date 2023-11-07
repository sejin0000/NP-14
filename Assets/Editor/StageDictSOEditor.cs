using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageDictSO))]
public class StageDictSOEditor : Editor
{
    private SerializedProperty stageDict;
    private Dictionary<string, StageSO> stageDictionary;

    private void OnEnable()
    {
        stageDict = serializedObject.FindProperty("stageDict");
        UpdateDictionary();
    }

    private void UpdateDictionary()
    {
        stageDictionary = new Dictionary<string, StageSO>();

        for (int i = 0; i < stageDict.arraySize; i++)
        {
            SerializedProperty element = stageDict.GetArrayElementAtIndex(i);
            StageSO stageSO = element.objectReferenceValue as StageSO;

            if (stageSO != null && !stageDictionary.ContainsKey(stageSO.StageName))
            {
                stageDictionary.Add(stageSO.StageName, stageSO);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(stageDict, true);
        if (EditorGUI.EndChangeCheck())
        {
            UpdateDictionary();
        }

        EditorGUILayout.LabelField("Stage Dictionary:");

        foreach (var kvp in stageDictionary)
        {
            EditorGUILayout.ObjectField(kvp.Key, kvp.Value, typeof(StageSO), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
