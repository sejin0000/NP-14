using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManager : MonoBehaviourPun
{
    public static TestGameManager Instance;

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
    public Button AugmentPanelOpenButton;

    [Header("Panel")]
    [SerializeField] private GameObject AugmentPanel;

    private void Awake()
    {
        isPlayerInstantiated = false;
        if (!isPlayerInstantiated)
        {
            isPlayerInstantiated = true;

            SpawnPlayer();
            SyncPlayer();
        }

        if (Instance == null)
        {
            Instance = this;
        }

        MonsterSpawnButton.onClick.AddListener(OnMonsterSpawnButtonClicked);
        AugmentPanelOpenButton.onClick.AddListener(OnAugmentPanelOpenButtonClicked);
    }

    private void Start()
    {
        // Augument 연결
        TestResultController MakeSetting = InstantiatedPlayer.GetComponent<TestResultController>();
        MakeSetting.MakeManager();
    }

    private void SpawnPlayer()
    {
        // PlayerCharacterSetting 
        string characterSettingPath = "Prefabs/CharacterData/PlayerCharacterSetting";
        GameObject characterSettingGO = Instantiate(Resources.Load<GameObject>(characterSettingPath));
        characterSetting = characterSettingGO.GetComponent<PlayerDataSetting>();

        // PhotonNetwork.Instantiate()
        string playerPrefabPath = "Pefabs/TestPlayer";
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
            Destroy(go);
        }
    }
    
    private void SetSpawnData()
    {
        AugmentPanel.SetActive(true);
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

    public void OnAugmentPanelOpenButtonClicked()
    {
        AugmentPanel.SetActive(true);
    }

    public void AllReady()
    {
        if (Ready == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            photonView.RPC("uiscene", RpcTarget.All);
        }
    }
}
