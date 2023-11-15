using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0208 : MonoBehaviourPun//피해를 입지않은 시간이 길어질수록 강해집니다.
{
    private PlayerStatHandler playerStat;

    public float power;
    private void Awake()
    {
        if (photonView.IsMine)//알맞은 타이밍 //가만히 있는 시간에 비례하여 공업
        {
            playerStat = GetComponent<PlayerStatHandler>();
            playerStat.HitEvent += HitDAHit;
            power = 0;
        }
    }
    private void Update()
    {
            playerStat.ATK.added += (Time.deltaTime) * 0.1f;
            power += Time.deltaTime * 0.1f;
    }

    void HitDAHit()
    {
        playerStat.AtkSpeed.added -= power;
        power = 0;
    }

}
