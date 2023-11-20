using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0208 : MonoBehaviourPun//���ظ� �������� �ð��� ��������� �������ϴ�.
{
    private PlayerStatHandler playerStat;

    public float power;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerStat = GetComponent<PlayerStatHandler>();
            playerStat.HitEvent += HitDAHit;
            power = 0;
        }
    }
    private void Update()
    {
            playerStat.ATK.added += (Time.deltaTime) * 0.3f;
            power += Time.deltaTime * 0.3f;
    }

    void HitDAHit()
    {
        playerStat.AtkSpeed.added -= power;
        power = 0;
    }

}
