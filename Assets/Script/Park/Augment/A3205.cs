using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A3205 : MonoBehaviourPun//설계시 2204참고
{
    List<PlayerStatHandler> target = new List<PlayerStatHandler>();
    PlayerStatHandler me;
    public GameObject Player;
    private TopDownCharacterController controller;
    // Update is called once per frame
    void TogetherParty()
    {
        if (photonView.IsMine) 
        {
            for (int i = 0; i < target.Count; ++i)
            {
                GameObject shiled = PhotonNetwork.Instantiate("AugmentList/A3205_1", target[i].transform.localPosition, Quaternion.identity);
                int targetID = shiled.GetPhotonView().ViewID;
                int ParentID = target[i].photonView.ViewID;
                shiled.GetComponent<Shield>().Initialized(10,1,1.5f);
                shiled.transform.SetParent(target[i].transform);
                photonView.RPC("TogetherSoDelicious",RpcTarget.Others,ParentID,targetID);
            }
        }

    }
    [PunRPC]
    public void TogetherSoDelicious(int ParentID,int targetID) 
    {
        PhotonView ParentView = PhotonView.Find(ParentID);
        GameObject parent = ParentView.gameObject;
        PhotonView targetView = PhotonView.Find(targetID);
        GameObject target = targetView.gameObject;
        target.transform.SetParent(parent.transform);
    }

    public void Init()
    {
        me = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        controller = me.gameObject.GetComponent<TopDownCharacterController>();
        controller.OnSkillEvent += TogetherParty; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !target.Contains(collision.GetComponent<PlayerStatHandler>()) && (collision.GetComponent<PlayerStatHandler>() != null))
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어입장");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target.Contains(collision.GetComponent<PlayerStatHandler>()))
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어퇴장");
        }
    }
}
