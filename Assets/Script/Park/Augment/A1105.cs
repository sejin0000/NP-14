using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class A1105 : MonoBehaviourPun
{  
    public WeaponSystem WeaponSystem;
    private WaitForSeconds autoTime = new WaitForSeconds(5f);

    private void Awake()
    {
        if (photonView.IsMine)
        {
            WeaponSystem=GetComponent<WeaponSystem>();
        }
    }
    private void OnEnable()
    {
        if (photonView.IsMine) 
        {
            AutoChangeStart();
        }
    }
    // Update is called once per frame
    void AutoChangeStart() 
    {
        if (photonView.IsMine) 
        {
            StopCoroutine("AutoChange");
            StartCoroutine("AutoChange");
        }
    }
    IEnumerator AutoChange() 
    {
        while (true)
        {
            WeaponSystem.isDamage = !WeaponSystem.isDamage;
            var targets = WeaponSystem.targets;
            if (WeaponSystem.isDamage) 
            {
                if (targets.ContainsKey("Enemy"))
                    continue;
                else
                {
                    targets["Enemy"] = (int)BulletTarget.Enemy;
                    targets.Remove("Player");
                    Debug.Log("A1105 - 공격모드 실행");
                }
            }
            else
            {
                if (targets.ContainsKey("Player"))
                    continue;
                else
                {
                    targets["Player"] = (int)BulletTarget.Player;
                    targets.Remove("Enemy");
                    Debug.Log("A1105 - 힐모드 실행");
                }
            }
            yield return autoTime;
        }
    }
}
