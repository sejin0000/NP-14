using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSOObj : MonoBehaviour
{
    public TestSO testSO;

    private void Start()
    {
        GameObject go = testSO.Prefab;
        Instantiate(go);
    }
}
