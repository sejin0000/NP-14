using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClassIdentifier : MonoBehaviourPunCallbacks
{    
    public PlayerDataSetting playerData;

    private int viewID;
    public void Initialize()
    {
        viewID = photonView.ViewID;
    }

    public void ClassChangeApply(int classNum)
    {
        Initialize();
        playerData.SetClassType(classNum, this.gameObject);
    }

    [PunRPC]
    public void ApplyClassChange(int classNum, int viewID)
    {        
        PhotonView photonView = PhotonView.Find(viewID);
        playerData.SetClassType(classNum, photonView.gameObject);
    }

    public int GetViewID()
    {
        return viewID;
    }
}
