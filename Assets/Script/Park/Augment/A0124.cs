using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A0124 : MonoBehaviour
{
    private GameObject A0124Prefabs;
    // Start is called before the first frame update
    private void Start()
    {
        A0124Prefabs = Resources.Load<GameObject>("A1024");
        Instantiate(A0124Prefabs);
        DarkEnd();
        GameManager.Instance.OnStageStart += DarkStart;
        GameManager.Instance.OnStageEnd += DarkEnd;
    }
    void DarkStart() 
    {
        A0124Prefabs.SetActive(true);
    }
    void DarkEnd()
    {
        A0124Prefabs.SetActive(false);
    }
}
