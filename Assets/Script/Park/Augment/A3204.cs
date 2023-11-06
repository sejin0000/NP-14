using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3204 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    GameObject Prefabs;
    GameObject nullcheck;

    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        controller.OnEndRollEvent += Makeshield;
        Prefabs = Resources.Load<GameObject>("AugmentList/A3204_1");
        nullcheck = null;
    }

    // Update is called once per frame
    void Makeshield()
    {
        if (nullcheck == null)
        {
            GameObject shield = Instantiate(Prefabs, transform);
            A3204_1 a3204_1 = shield.GetComponent<A3204_1>();
            a3204_1.Init(playerStat);
            nullcheck = shield;
        }
        else
        {
            Debug.Log("실드 재생");
        }
    }
}
