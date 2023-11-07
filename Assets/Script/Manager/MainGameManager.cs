using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviourPunCallbacks
{
    [Header("ClientPlayer")]
    public GameObject InstantiatedPlayer;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;

    private void Start()
    {
        SpawnPlayer();
        SyncPlayer();
        PlayerResultController MakeSetting = InstantiatedPlayer.GetComponent<PlayerResultController>();//@@@@@@@@@추가
        MakeSetting.MakeManager();//@@@@@@@@@@@@@@@@@추가
    }
    private void SpawnPlayer()
    {
        // PlayerCharacterSetting 
        string characterSettingPath = "Prefabs/CharacterData/PlayerCharacterSetting";
        GameObject characterSettingGO = Instantiate(Resources.Load<GameObject>(characterSettingPath));        
        characterSetting = characterSettingGO.GetComponent<PlayerDataSetting>();

        // PhotonNetwork.Instantiate()
        string playerPrefabPath = "Pefabs/Player";
        InstantiatedPlayer = PhotonNetwork.Instantiate(playerPrefabPath, Vector3.zero, Quaternion.identity);
        characterSetting.ownerPlayer = InstantiatedPlayer;
        characterSetting.viewID = InstantiatedPlayer.GetPhotonView().ViewID;

        // ClassIdentifier 데이터 Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;
    }

    private void SyncPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            characterSetting.SetClassType((int)classNum, InstantiatedPlayer);
            int viewID = characterSetting.viewID;
            InstantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        }
    }
}
