using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0211 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    int persent = 2;
    int maxpersent = 10;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();

    }
    private void Start()
    {
        playerStat.HitEvent2 += Endure;
    }

    // Update is called once per frame
    void Endure(float damege)
    {
        int Per = Random.Range(persent, maxpersent);
        if (persent >= Per)
        {
            playerStat.CurHP += damege*0.2f;
        }
    }
}
