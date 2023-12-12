using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1101 : MonoBehaviourPun
{
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerStat = GetComponent<PlayerStatHandler>();
            playerStat.EnemyHitEvent += HitPlusDamege; // �߿��Ѻκ�
        }
    }
    // Update is called once per frame
    void HitPlusDamege()
    {
        playerStat.ATK.added += 0.5f; // �߿��� �κ�2
    }
}
