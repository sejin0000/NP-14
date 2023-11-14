using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0220 : MonoBehaviourPun
{
    public int percent;
    private TopDownCharacterController controller;
    private PlayerStatHandler stats;
    
    private void Awake()
    {
        percent = 0;
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            stats = GetComponent<PlayerStatHandler>();
            controller.OnAttackEvent += Drain;
        }

    }
    // Update is called once per frame
    void Drain()
    {
        int random = Random.Range(0, 100);
        if (percent > random)
        {
            stats.CurHP += stats.ATK.total;
        }
    }

    public void PercentUp(int PerUp) //PercentUp
    {
        percent += PerUp;
    }
}