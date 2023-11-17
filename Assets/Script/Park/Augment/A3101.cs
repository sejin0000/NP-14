using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3101 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    public float heal=5f;
    public float healTime = 5f;
    float time = 0f;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();

            playerStat.HitEvent += restartTime; // 중요한부분
        }
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= healTime) 
        {
            StayHeal();
            time = 0f;
        }
    }
    // Update is called once per frame
    void StayHeal()
    {
        if (photonView.IsMine) 
        {
            Debug.Log($"3101힐전 체력 {playerStat.CurHP}");
            playerStat.HPadd(heal);
            Debug.Log($"3101힐후 체력 {playerStat.CurHP}");
        }
    }
    void restartTime() 
    {
        time = 0;
    }
}
