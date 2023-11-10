using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class A3107_1 : MonoBehaviourPun
{
    public float damege;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("EnemyBullet")) 
        {
            EnemyAI wjr =  collision.GetComponent<EnemyAI>();
            wjr.PV.RPC("DecreaseHP", RpcTarget.All, damege);
        }
    }
    public void DamegeUpdate(float a) 
    {
        damege = a*0.8f;
    }

}
