using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            GameManager.Instance.CallStageEndEvent();
            gameObject.SetActive(false);
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
}
