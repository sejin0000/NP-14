using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A1104 : MonoBehaviourPun
{
    private TopDownCharacterController controller;
    private PlayerStatHandler playerStat;
    private CoolTimeController coolTimeController;

    public int layerMask;
    private void Awake()
    {
        if (photonView.IsMine)
        {
            controller = GetComponent<TopDownCharacterController>();
            playerStat = GetComponent<PlayerStatHandler>();
            coolTimeController = GetComponent<CoolTimeController>();

            controller.OnFlashEvent += Flash; // 중요한부분
            layerMask = 1 << LayerMask.NameToLayer("Wall");
        }
    }
    // Update is called once per frame
    void Flash()
    {
        //Vector2 player = transform.position;
        //Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 dir = (mouse - player).normalized * 1.5f;
        //gameObject.transform.position = transform.position + dir;


        Vector2 player = transform.position;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (mouse - player).normalized;
        RaycastHit2D hit = Physics2D.Raycast(player, dir, 1.5f, layerMask);
        Debug.DrawRay(player, dir * 1.5f, Color.red, 3f);
        Vector3 target = dir;
        if (hit)
        {
            Debug.Log("히트");
            target = dir * hit.distance;
        }
        else 
        {
            target = dir * 1.5f;
        }
        gameObject.transform.position = transform.position + target;
    }


}
