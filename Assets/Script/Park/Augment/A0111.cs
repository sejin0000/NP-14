using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0111 : MonoBehaviourPun//공격을 하지 않은 시간에 비례하여 다음 공격의 공격력이 증가 합니다.

{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private float power;

    bool stop;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            power = 0f;
            controller.OnAttackEvent += StartAtk;//이걸 돈획득 이벤트에검 
            controller.OnEndAttackEvent += StopAtk;
            stop = false;
        }
    }
    private void Update()
    {
        if (stop) 
        {
            playerStat.ATK.added += (Time.deltaTime) * 0.1f;
            power += Time.deltaTime * 0.1f;
        }
    }
    // Update is called once per frame
    void StartAtk()
    {
        stop = false;
    }
    void StopAtk()
    {
        playerStat.ATK.added -= power;//영구 증가면 이부분 주석 처리 하고 파워를 삭제 그런데 그럼 너무사기같음
        stop = true;
    }
}
