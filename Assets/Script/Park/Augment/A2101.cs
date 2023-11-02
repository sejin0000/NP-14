using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A2101 : MonoBehaviour
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
        playerStat = GetComponent<PlayerStatHandler>();
    }
    private void Start()
    {
        controller.OnSkillEvent += AtkSpeedUp;
    }

    // Update is called once per frame
    void AtkSpeedUp()
    {
        playerStat.AtkSpeed.added += 0.01f;
    }
}
