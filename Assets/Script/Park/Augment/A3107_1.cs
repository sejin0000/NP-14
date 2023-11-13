using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A3107_1 : MonoBehaviourPun
{
    public float damage;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) 
        {
            EnemyAI wjr =  collision.GetComponent<EnemyAI>();
            wjr.PV.RPC("DecreaseHP", RpcTarget.All, damage);
        }
    }
    public void DamegeUpdate(float a) 
    {
        damage = a*0.8f;
    }

}
