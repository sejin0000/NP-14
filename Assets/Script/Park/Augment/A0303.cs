using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A0303 : MonoBehaviourPun
{
    //// 분신 소환 오브젝트
    //public GameObject Partner;
    //public GameObject Player;

    //public PlayerSO soldierSO;
    //public PlayerSO shotGunSO;
    //public PlayerSO sniperSO;
     

    //public int viewID;
    //public string PartnerLayerName = "Partner";

    //public GameObject SpawnPartner()
    //{
    //    if (MainGameManager.Instance != null) 
    //    {
    //        Player = MainGameManager.Instance.InstantiatedPlayer;
    //    }
    //    viewID = Player.GetPhotonView().ViewID;
    //    string playerPrefabPath = "Pefabs/Player";
    //    Partner = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
    //    Partner.GetComponent<PlayerStatHandler>().ATK.coefficient *= 0.5f;
    //    Partner.layer = LayerMask.NameToLayer(PartnerLayerName);
    //    return Partner;
    //}

    //public void Initialize(Transform parentTransform)
    //{
    //    GameObject partner = SpawnPartner();
    //    PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum);
    //    SetClassType((int)classNum, partner);
    //    partner.GetPhotonView().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
    //    partner.transform.parent = parentTransform;
    //    partner.transform.localPosition = Vector3.zero;
    //}

    //private void SetClassType(int charType, GameObject playerGo)
    //{
    //    PlayerStatHandler statSO;        
    //    statSO = playerGo.GetComponent<PlayerStatHandler>();
        

    //    switch (charType)
    //    {
    //        case (int)LobbyPanel.CharClass.Soldier:
    //            statSO.CharacterChange(soldierSO);
    //            break;
    //        case (int)LobbyPanel.CharClass.Shotgun:
    //            statSO.CharacterChange(shotGunSO);
    //            break;
    //        case (int)LobbyPanel.CharClass.Sniper:
    //            statSO.CharacterChange(sniperSO);
    //            break;
    //    }
    //}
}
