using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance;
    public GameObject Player;

    public event Action OnStageStart;
    public event Action OnStageEnd;

    void Awake()
    {
        Instance = this;
    }
    private void GameStart()
    {
        OnStageStart.Invoke(); 
    }
    private void GameEnd()
    {
        OnStageEnd.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
