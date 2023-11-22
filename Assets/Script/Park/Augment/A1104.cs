using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A1104 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();

            controller.OnFlashEvent += Flash; // 중요한부분
        }
    }
    // Update is called once per frame
    void Flash()
    {
        Vector2 player = transform.position;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (mouse - player).normalized * 1.5f;
        this.gameObject.transform.position = transform.position + dir;
    }


}
