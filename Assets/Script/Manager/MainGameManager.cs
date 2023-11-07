using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Start,
    Playing,
    End,
}
public class MainGameManager : MonoBehaviourPunCallbacks
{
    public static MainGameManager Instance;

    [Header("ClientPlayer")]
    public GameObject InstantiatedPlayer;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;

    [Header("GameData")]
    public MonsterData monsterData;
    [Space(10f)]
    public StageData stageData;
    [Space(10f)]
    public GameState gameState;

    [Serializable]
    public struct MonsterData
    {
        public int monsterNum;
        public string monsterType;
    }

    [Serializable]
    public struct StageData
    {
        public int currentStage;
        public bool isFarmingRoom;
    }


    [HideInInspector]
    public event Action OnGameStartedEvent;
    public event Action OnGameEndedEvent;



    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SpawnPlayer();
        SyncPlayer();
        CallStartEvent();
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

        // ClassIdentifier µ•¿Ã≈Õ Init()
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

    #region Events
    public void CallStartEvent()
    {
        OnGameStartedEvent?.Invoke();
    }

    public void CallEndEvent()
    {
        OnGameEndedEvent?.Invoke();
    }
    #endregion
}
