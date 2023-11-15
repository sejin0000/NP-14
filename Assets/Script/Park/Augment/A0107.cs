using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class A0107 : MonoBehaviourPun
{
    //private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    //private CoolTimeController coolTimeController;

    public float power;
    public float oldpower;
    bool Ismove;
    float powerTime = 0f;
    float magnification = 0.1f;
    private void Awake()
    {
        if (photonView.IsMine)//알맞은 타이밍 //가만히 있는 시간에 비례하여 공업
        {
            //controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();

            playerStat.MoveStartEvent += MoveStartEvent;
            playerStat.MoveEndEvent += MoveEndEvent;
            power = 0;
            oldpower = 0;
            Ismove=true;
        }
    }
    private void Update()
    {
        if (!Ismove) 
        {
            playerStat.ATK.added += (Time.deltaTime) * 0.1f;
            power += Time.deltaTime * 0.1f;
            powerTime += Time.deltaTime;
            if (powerTime >= 30f)
            {
                magnification = 0.2f;
            }
        }

    }

    // Update is called once per frame
    void MoveStartEvent()
    {
        playerStat.AtkSpeed.added -= power; // 중요한 부분2
        power = 0;
        Ismove = true;
    }
    void MoveEndEvent() 
    {
        Ismove=false;
        magnification = 0.1f;
        powerTime = 0f;
    }
}
