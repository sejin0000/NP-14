using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0111 : MonoBehaviourPun//������ ���� ���� �ð��� ����Ͽ� ���� ������ ���ݷ��� ���� �մϴ�.

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
            controller.OnAttackEvent += StartAtk;//�̰� ��ȹ�� �̺�Ʈ���� 
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
        playerStat.ATK.added -= power;//���� ������ �̺κ� �ּ� ó�� �ϰ� �Ŀ��� ���� �׷��� �׷� �ʹ���ⰰ��
        stop = true;
    }
}
