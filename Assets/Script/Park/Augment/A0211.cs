using Photon.Pun;
using UnityEngine;

public class A0211 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    int persent = 2;
    int maxpersent = 10;
    private void Awake()
    {
        if (photonView.IsMine) 
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            playerStat.HitEvent2 += Endure;
        }
    }
    void Endure(float damege)
    {
        int Per = Random.Range(persent, maxpersent);
        if (persent >= Per)
        {
            playerStat.HPadd(damege * 1.2f);
        }
    }
}
