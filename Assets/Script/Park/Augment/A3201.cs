using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3201 : MonoBehaviour
{

    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTime;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();
        coolTime = GetComponent<CoolTimeController>();
    }
    private void Start()
    {
        controller.OnRollEvent += Reloading;
    }

    // Update is called once per frame
    void Reloading()
    {
        Debug.Log($"총알수체크장전전{playerStat.CurAmmo}");
        playerStat.AmmoMax.added += 3;
        coolTime.curReloadCool = 0f;
        controller.CallReloadEvent();
        Invoke("reloadcontrol", 3);
        Debug.Log($"총알수체크장전후{playerStat.CurAmmo}");
        Debug.Log($"최대총알체크{playerStat.AmmoMax.total}");
    }
    void reloadcontrol() 
    {
        playerStat.AmmoMax.added -= 3;
    }
}
