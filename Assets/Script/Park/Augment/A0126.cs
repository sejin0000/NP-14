using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

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
            Debug.Log("몬스터와 충돌중");
            var collObject = coll.transform.parent;
            var collEnemy = collObject.GetComponent<EnemyAI>();
            collObject.GetComponent<PhotonView>().RPC("DecreaseHP", RpcTarget.All, GetComponent<PlayerStatHandler>().ATK.total);
            collEnemy.knockbackStartPosition = collObject.transform.position;
            Vector2 knockbackVector = ((gameObject.transform.position - collObject.transform.position).normalized * 3f);
            collEnemy.knockbackTargetPosition = collEnemy.knockbackStartPosition - knockbackVector;
            collEnemy.knockbackStartTime = Time.time;
            collEnemy.isKnockback = true;
        }
    }
}
