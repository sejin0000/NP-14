﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManagerDohyun : MonoBehaviourPun
{
    public static TestGameManagerDohyun Instance;

    public enum MonsterType
    {
        몬스터1,
        몬스터2,
        몬스터3,
    }

    [Header("ClientPlayer")]
    public GameObject InstantiatedPlayer;
    [SerializeField] private bool isPlayerInstantiated;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;

    [Header("GameData")]
    public List<MonsterData> monsterDataList;
    public int currentMonsterCount;

    [Header("Auguments")]
    public int tier;
    public int Ready;

    [Serializable]
    public struct MonsterData
    {
        public int monsterNum;
        public MonsterType monsterType;
    }

    [Header("Button")]
    public Button MonsterSpawnButton;

    private void Awake()
    {
        isPlayerInstantiated = false;
        if (!isPlayerInstantiated)
        {
            isPlayerInstantiated = true;
            SpawnPlayer();
            SyncPlayer();

            // Augument
            PlayerResultController MakeSetting = InstantiatedPlayer.GetComponent<PlayerResultController>();
            MakeSetting.MakeManager();
        }

        if (Instance == null)
        {
            Instance = this;
        }

        MonsterSpawnButton.onClick.AddListener(OnMonsterSpawnButtonClicked);
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

        // AttachMiniHUD
        GameObject attachUI = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerHUD/HUD_Root"));
        attachUI.transform.SetParent(InstantiatedPlayer.gameObject.transform);
    }

    private void SyncPlayer()
    {
        int viewID = characterSetting.viewID;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object classNum))
        {
            characterSetting.SetClassType((int)classNum, InstantiatedPlayer);
            InstantiatedPlayer.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, (int)classNum, viewID);
        }

        InstantiatedPlayer.GetComponent<PhotonView>().RPC("AttachMiniUI", RpcTarget.Others, viewID);
    }

    private void SpawnMonster(int spawnNum, string targetMonster)
    {
        //monsterDataList.Add(new MonsterData { monsterNum = spawnNum, monsterType = Enum.GetName(typeof(MonsterType), targetMonster) });
        GameObject go = PhotonNetwork.Instantiate("Prefabs/Enemy/SpawnPoint", transform.position, Quaternion.identity);
        if (PhotonNetwork.IsMasterClient)
        {
            float destinationX = UnityEngine.Random.Range(-5f, 5f);
            float destinationY = UnityEngine.Random.Range(-5f, 5f);
            go.transform.position = new Vector3(destinationX, destinationY, 0);

            EnemySpawn enemySpawn = go.GetComponent<EnemySpawn>();
            enemySpawn.Spawn();
        }
    }

    private void SetSpawnData()
    {

    }

    public void OnMonsterSpawnButtonClicked()
    {
        foreach (var monsterInfo in monsterDataList)
        {
            int monsterCount = monsterInfo.monsterNum;
            var monsterType = monsterInfo.monsterType;
            string monsterPar = Enum.GetName(typeof(MonsterType), monsterType);

            for (int i = 0; i < monsterCount; i++)
            {
                SpawnMonster(monsterCount, monsterPar);
                currentMonsterCount += 1;
            }
        }
    }

    public void AllReady()
    {
        if (Ready == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC("uiscene", RpcTarget.All);
        }
    }

    [PunRPC]
    public void AttachMiniUI(int viewID)
    {
        PhotonView photonView = PhotonView.Find(viewID);
        GameObject attachUI = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerHUD/HUD_Root"));
        attachUI.transform.SetParent(photonView.gameObject.transform);
    }
}
