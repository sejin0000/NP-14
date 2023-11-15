using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A1105 : MonoBehaviourPun
{
    //private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    //private CoolTimeController coolTimeController;
    public WeaponSystem WeaponSystem;
    public GameObject player;
    int autoTime = 5;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            WeaponSystem=GetComponent<WeaponSystem>();
            AutoChangeStart();
        }
    }
    // Update is called once per frame
    void AutoChangeStart() 
    {
        StartCoroutine("AutoChange");
    }
    IEnumerator AutoChange() 
    {
        while (true)
        {
            //if(gameObject.SetActive() == true)
            WeaponSystem.isDamage = !WeaponSystem.isDamage;
            yield return autoTime;
        }
    }
}
