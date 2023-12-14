using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviourPun
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GameManager.Instance.CallStageEndEvent();
            photonView.RPC("PortalOff", RpcTarget.All);
            if(GameManager.Instance.MG.roomNodeInfo.porTal == null)
            {
                GameManager.Instance.MG.roomNodeInfo.porTal = gameObject;
            }
        }
    }

    public void portalSetting(float x,float y)
    {
        gameObject.transform.position = new Vector2(x, y);
    }

    [PunRPC]
    public void PortalOff()
    {
        gameObject.SetActive(false);
    }
}
