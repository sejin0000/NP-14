using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager1 : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager1 Instance;
    public GameObject Player;

    public event Action OnStageStart;//���� ���۽� �ʱ�ȭ ����� �ϴ°� ����
    public event Action OnStageEnd;//
    public bool IsNormalStage; //�Ϲݽ������� or �̺�Ʈ ��������
    public int tier;      //����Ƽ�� 1 2 3 �ö󰥼��� ����
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
