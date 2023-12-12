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
            playerStat.EnemyHitEvent += HitPlusDamege; // 중요한부분
        }
    }
    // Update is called once per frame
    void HitPlusDamege()
    {
        playerStat.ATK.added += 0.5f; // 중요한 부분2
    }
}
