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
    private void Awake()
    {
        if (photonView.IsMine)//�˸��� Ÿ�̹� //������ �ִ� �ð��� ����Ͽ� ����
        {
            //controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();

            playerStat.MoveStartEvent += MoveStartEvent;
            playerStat.MoveEndEvent += MoveEndEvent;
            power = 0;
            oldpower = 0;
            powerTime = 0f;
            Ismove =false;
        }
    }
    private void Update()
    {
        if (!Ismove && photonView.IsMine) 
        {
            playerStat.ATK.added += (Time.deltaTime) * 0.1f;
            power += Time.deltaTime * 0.1f;
            powerTime += Time.deltaTime;

        }

    }

    // Update is called once per frame
    void MoveStartEvent()
    {
        playerStat.ATK.added -= power; // �߿��� �κ�2
        power = 0;
        Ismove = true;
    }
    void MoveEndEvent() 
    {
        Ismove=false;
        powerTime = 0f;
    }
}
