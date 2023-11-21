using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class A3104 : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;
    private TopDownMovement topDown;
    private CapsuleCollider2D capsuleColl;
    public float DamageCoeff;
    public bool isRoll;

    private void Awake()
    {
        playerStatHandler = GetComponent<PlayerStatHandler>();
        capsuleColl = GetComponent<CapsuleCollider2D>();
        topDown = GetComponent<TopDownMovement>();
        DamageCoeff = 0.15f;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        isRoll = topDown.isRoll;            
        if (topDown.isRoll)
        {
            if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var collObject = coll.gameObject;
                var collEnemy = collObject.GetComponent<EnemyAI>();
                collEnemy.knockbackDistance = 3f;
            }        
        }                
    }
}
