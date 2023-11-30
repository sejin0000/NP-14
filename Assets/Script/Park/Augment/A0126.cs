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
    public float DamageCoeff;

    private void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        capsuleColl = GetComponent<CapsuleCollider2D>();
        DamageCoeff = 0.15f;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            var collObject = coll.gameObject;
            var collEnemy = collObject.GetComponent<EnemyAI>();
            collEnemy.knockbackDistance = 3f;
        }
    }
}
