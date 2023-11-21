using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3104 : MonoBehaviour
{
    private PlayerStatHandler playerStatHandler;
    private TopDownMovement topDown;
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
        if (coll.gameObject.layer == LayerMask.NameToLayer("Enemy") 
            && topDown.isRoll == true)
        {
            Debug.Log("몬스터와 충돌중");
            var collObject = coll.gameObject;
            var collEnemy = collObject.GetComponent<EnemyAI>();
            collEnemy.knockbackDistance = 3f;
        }
    }
}
