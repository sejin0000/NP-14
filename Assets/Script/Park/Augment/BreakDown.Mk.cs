using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDownMk : MonoBehaviourPun
{
    public int percent;
    private TopDownCharacterController controller;
    private WeaponSystem weaponSystem;
    private void Awake()
    {
        percent = 0;
        if (photonView.IsMine) 
        {

            weaponSystem = GetComponent<WeaponSystem>();
            controller=GetComponent<TopDownCharacterController>();
            controller.OnAttackEvent += MoreAtk;
        }

    }
    // Update is called once per frame
    void MoreAtk()
    {
        int random = Random.Range(0, 100);
        if (percent > random) 
        {
            weaponSystem.Shooting();
        }
    }

    public void PercentUp(int PerUp) //PercentUp
    {
        percent += PerUp;
    }
}
