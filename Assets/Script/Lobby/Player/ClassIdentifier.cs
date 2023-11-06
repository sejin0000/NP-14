using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ClassIdentifier : MonoBehaviourPunCallbacks
{
    public PlayerInfo playerInfo;
    private PlayerSO soldierSO;
    private PlayerSO shotGunSO;
    private PlayerSO sniperSO;

    private int viewID;
    public void Initialize()
    {
        soldierSO = playerInfo.soldierSO;
        shotGunSO = playerInfo.shotGunSO;
        sniperSO = playerInfo.sniperSO;

        viewID = photonView.ViewID;
    }

    public void ClassChangeApply(int classNum)
    {
        Initialize();
        playerInfo.SetClassType(classNum, this.gameObject);
    }

    [PunRPC]
    public void ApplyClassChange(int classNum, int viewID)
    {        
        PhotonView photonView = PhotonView.Find(viewID);
        playerInfo.SetClassType(classNum, photonView.gameObject);
    }

    public int GetViewID()
    {
        return viewID;
    }
}
