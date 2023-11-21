using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManagerWooMin : MonoBehaviourPun
{
    public static TestGameManagerWooMin Instance;

    public enum MonsterType
    {
        ����1,
        ��������,
    }

    [Header("ClientPlayer")]
    public GameObject InstantiatedPlayer;
    [SerializeField] private bool isPlayerInstantiated;
    public Dictionary<int, Transform> playerInfoDictionary;
    public Dictionary<MonsterType, string> monsterNameDictionary;

    [Header("PlayerData")]
    public PlayerDataSetting characterSetting;
    public bool isDie;

    [Header("GameData")]
    public List<MonsterData> monsterDataList;
    public int currentMonsterCount;
    public int PartyDeathCount;

    [Header("Auguments")]
    public int tier;
    public int Ready;

    [HideInInspector]
    public event Action OnOverCheckEvent;

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
        playerInfoDictionary = new Dictionary<int, Transform>();
        monsterNameDictionary = new Dictionary<MonsterType, string>();
        AddMonsterDict();


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

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Prefabs/Player/Debuff", Vector3.zero, Quaternion.identity);
        }
    }

    private void Start()
    {
        // Augument ����
        TestResultController MakeSetting = InstantiatedPlayer.GetComponent<TestResultController>();
        MakeSetting.MakeManager();
    }

    private void AddMonsterDict()
    {
        monsterNameDictionary[MonsterType.����1] = "Test_Enemy";
        monsterNameDictionary[MonsterType.��������] = "Test_Boss";
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

        // �÷��̾� ������ �߰�
        int viewID = characterSetting.viewID;
        playerInfoDictionary.Add(viewID, InstantiatedPlayer.transform);
        GameObject sendingPlayer = InstantiatedPlayer;
        photonView.RPC("SendPlayerInfo", RpcTarget.Others, viewID);

        // ClassIdentifier ������ Init()
        InstantiatedPlayer.GetComponent<ClassIdentifier>().playerData = characterSetting;

        // isDie
        var playerStatHandler = InstantiatedPlayer.GetComponent<PlayerStatHandler>();
        isDie = playerStatHandler.isDie;
        PartyDeathCount = 0;
        //playerStatHandler.OnDieEvent += DiedAfter;
    }

    [PunRPC]
    public void SendPlayerInfo(int viewID)
    {
        GameObject clientPlayer = PhotonView.Find(viewID).gameObject;
        playerInfoDictionary.Add(viewID, clientPlayer.transform);
        Debug.Log($"{playerInfoDictionary.Count}���� ��ųʸ��� ��ϵ�");
        int cnt = 0;
        foreach (var key in playerInfoDictionary.Keys)
        {
            cnt += 1;
            Debug.Log($"{playerInfoDictionary.Count}���� Ű �� {cnt}��° == {key}");
        }
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
            //float destinationX = UnityEngine.Random.Range(-5f, 5f);
            //float destinationY = UnityEngine.Random.Range(-5f, 5f);
            go.transform.position = new Vector3(0, 0, 0);

            EnemySpawn enemySpawn = go.GetComponent<EnemySpawn>();
            enemySpawn.Spawn(targetMonster);
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
            string monsterPar = monsterNameDictionary[monsterType];

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

    public void DiedAfter()
    {
        photonView.RPC("AddPartyDeathCount", RpcTarget.All);
        Debug.Log("MainGameManager : DiedAfter() => PartyDeath : " + PartyDeathCount.ToString());
        OnOverCheckEvent?.Invoke();
    }

    [PunRPC]
    public void AddPartyDeathCount()
    {
        PartyDeathCount++;
    }
}
