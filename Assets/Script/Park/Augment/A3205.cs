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
                photonView.RPC("TogetherSoDelicious",RpcTarget.All,ParentID,targetID);
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
        target.transform.localScale = parent.transform.localScale;
    }

    public void Init()
    {
        me = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        controller = me.gameObject.GetComponent<TopDownCharacterController>();
        controller.OnSkillEvent += TogetherParty; 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerStatHandler targetP = collision.GetComponent<PlayerStatHandler>();
        if (targetP!=null)
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerStatHandler targetP = collision.GetComponent<PlayerStatHandler>();
        if (targetP != null)
        {
            target.Remove(targetP);
            Debug.Log("플레이어퇴장");
        }
    }
}
