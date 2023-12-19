using Photon.Pun;
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
            controller.OnAttackEvent += StartAtk;
            controller.OnEndAttackEvent += StopAtk;
            stop = true;
        }
    }
    private void Update()
    {
        if (stop && photonView.IsMine) 
        {
            playerStat.ATK.added += (Time.deltaTime) * 0.5f;
            power += Time.deltaTime * 0.5f;
        }
    }
    void StartAtk()
    {
        stop = false;
    }
    void StopAtk()
    {
        if (!stop && photonView.IsMine) 
        {
            playerStat.ATK.added -= power;
            power = 0f;
            stop = true;
        }
    }
}
