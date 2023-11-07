using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakDownMk : MonoBehaviour
{
    int percent = 0;
    private TopDownCharacterController controller;

    private CoolTimeController coolTimeController;
    private WeaponSystem weaponSystem;
    private void Awake()
    {
        percent = 0;
        weaponSystem=GetComponent<WeaponSystem>();
    }
    private void Start()
    {
        controller.OnAttackEvent += MoreAtk;
        
    }

    // Update is called once per frame
    void MoreAtk()
    {
        int random = Random.Range(0, 100);
        if (percent > random) 
        {
            weaponSystem.BS();
        }
    }

    public void PercentUp(int PerUp) //PercentUp
    {
        percent += PerUp;
    }
}
