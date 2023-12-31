using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class A1207 : MonoBehaviourPun
{
    private CollisionController collision;
    private PlayerStatHandler playerStatHandler;
    private List<Transform> target;
    private Transform targetOne;
    private PlayerStatHandler targetPlayerStatHandler;
    private PlayerInput playerinput;

    private void Awake()
    {
        collision = GetComponent<CollisionController>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
        collision.footCollider.isTrigger = true;
        collision.rigidbody.bodyType = RigidbodyType2D.Kinematic;
        if (photonView.IsMine)
        {

            target = new List<Transform>();
     

            GameManager.Instance.OnStageStartEvent += ImDie;
            GameManager.Instance.OnBossStageStartEvent += ImDie;
            GameManager.Instance.PlayerLifeCheckEvent += TargetDieCheck;
            playerinput = GetComponent<PlayerInput>();

            Dictionary<int, Transform> dic = GameManager.Instance.playerInfoDictionary;
            foreach (KeyValuePair<int, Transform> kv in dic)
            {
                if (kv.Key != gameObject.GetPhotonView().ViewID) 
                {
                    target.Add(kv.Value);

                }
            }
            targetOne = target[0];
            targetPlayerStatHandler = targetOne.GetComponent<PlayerStatHandler>();

        }
    }
    private void Update()
    {
        if (photonView.IsMine) 
        {
            transform.position = new Vector2(targetOne.position.x + 0.2f, targetOne.position.y + 0.2f);
        }

    }
    private void TargetDieCheck() 
    {
        if (targetPlayerStatHandler.isDie)
        {
            playerinput.actions.FindAction("Attack").Disable();
        }
        else
        {
            playerinput.actions.FindAction("Attack").Enable();
        }
    }
    public void Change()
    {
        if (targetOne == target[0] && (target.Count >=2))
        {
            targetOne = target[1];
        }
        else if ((target.Count >= 2))
        {
            targetOne = target[0];
        }
        targetPlayerStatHandler= targetOne.GetComponent<PlayerStatHandler>();
        TargetDieCheck();
    }
    // Update is called once per frame
    void ImDie()
    {
        GameManager.Instance.PV.RPC("AddPartyDeathCount", RpcTarget.All);
    }
}
