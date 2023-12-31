using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class A0303 : MonoBehaviourPun
{
    // 분신 소환 오브젝트
    public GameObject Partner;
    public GameObject Player;

    public PlayerSO soldierSO;
    public PlayerSO shotGunSO;
    public PlayerSO sniperSO;


    public int viewID;
    public string PartnerLayerName = "Partner";

    public GameObject SpawnPartner()
    {
        if (photonView.IsMine)
        { 
            if (MainGameManager.Instance != null)
            {
                Player = MainGameManager.Instance.InstantiatedPlayer;
            }
            if (TestGameManager.Instance != null) 
            {
                Player = TestGameManager.Instance.InstantiatedPlayer;
            }
            viewID = Player.GetPhotonView().ViewID;
            string playerPrefabPath = "Pefabs/Player";
            Partner = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
            Partner.GetComponent<PlayerStatHandler>().ATK.coefficient *= 0.5f;
            Partner.layer = LayerMask.NameToLayer(PartnerLayerName);
            Partner.GetComponent<PlayerInput>().actions.FindAction("Move2").Disable();
            Partner.GetComponent<PlayerInput>().actions.FindAction("Move").Disable();            
            return Partner;
        }
        return null;
    }

    public GameObject Initialize(Transform parentTransform)
    {
        GameObject partner = SpawnPartner();
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum);
        SetClassType((int)classNum, partner);
        partner.GetPhotonView().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        partner.transform.parent = parentTransform;
        partner.transform.localPosition = Vector3.zero;        
        partner.AddComponent<PartnerMovement>();
        return partner;
    }

    private void SetClassType(int charType, GameObject playerGo)
    {
        PlayerStatHandler statSO;
        statSO = playerGo.GetComponent<PlayerStatHandler>();


        switch (charType)
        {
            case (int)CharClass.Soldier:
                statSO.CharacterChange(soldierSO);
                break;
            case (int)CharClass.Shotgun:
                statSO.CharacterChange(shotGunSO);
                break;
            case (int)CharClass.Sniper:
                statSO.CharacterChange(sniperSO);
                break;
        }
    }
}
