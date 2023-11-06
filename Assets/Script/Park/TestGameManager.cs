using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager1 Instance;
    public GameObject Player;

    public event Action OnStageStart;//게임 시작시 초기화 해줘야 하는게 있음
    public event Action OnStageEnd;//
    public bool IsNormalStage; //일반스테이지 or 이벤트 스테이지
    public int tier;      //증강티어 1 2 3 올라갈수록 좋음
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
