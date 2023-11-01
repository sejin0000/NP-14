using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CoolTimeController : MonoBehaviour
{
    private TopDownCharacterController controller;
    private void Awake()
    {
        controller = GetComponent<TopDownCharacterController>();
    }
    private void Start()
    {
        controller.OnRollEvent += RollCoolTime;
    }

    private void RollCoolTime()
    {
        float coolTime = controller.playerStatHandler.RollCoolTime.total;
        controller.playerStatHandler.CanRoll = false;
        Invoke("EndRollCoolTime", coolTime);
    }
    private void EndRollCoolTime()
    {
        controller.playerStatHandler.CanRoll = true;
    }
}
