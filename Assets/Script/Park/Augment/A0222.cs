using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0222 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();

    }
    private void Start()
    {
        controller.OnSkillEvent += RollingHeal;
    }

    // Update is called once per frame
    void RollingHeal()
    {
        playerStat.CurHP += playerStat.HP.total*0.1f;
    }
}
