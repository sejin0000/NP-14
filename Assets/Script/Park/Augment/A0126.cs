using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A0126 : MonoBehaviourPun
{
    private PlayerStatHandler playerStatHandler;
    private CapsuleCollider2D capsuleColl;

    private void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        capsuleColl = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("���Ϳ� �浹��");
            coll.transform.parent.GetComponent<PhotonView>().RPC("DecreaseHP", RpcTarget.All, GetComponent<PlayerStatHandler>().ATK.total);
        }
    }
}
