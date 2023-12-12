using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.UI;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviourPun
{
    public GameObject Case;
    public List<int> EnemyViewIDList;
    private void Start()
    {
        GameManager.Instance.OnStageEndEvent += StageMonsterClear;
        EnemyViewIDList = new List<int>();
    }

    public void MonsterSpawn()
    {
        Debug.Log("몬스터 생성 이벤트");
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StageListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// 현재 스테이지에 몬스터 분대 수 

        for (int i = 0; i < mapGenerator.roomNodeInfo.allRoomList.Count; i++) //방 수 만큼 순회
        {
            RectInt room = mapGenerator.roomNodeInfo.allRoomList[i].roomRect;
            int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  이번 방에 어떤 분대를 생성할지

            var playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            var spawnNum = playerCount * stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterNum;
            if (playerCount > 2)
            {
                spawnNum += 2;
            }
            if (playerCount == 1
                && GameManager.Instance.curStage > 3)
            {
                spawnNum += 2;
            }
            for (int j = 0; j < spawnNum; j++)// 스분대원 수만큼 순회
            {
                //Debug.Log("몬스터 생성");
                Vector2 spawnPos;
                spawnPos.x = Random.Range(room.x + 1, room.x + room.width);
                spawnPos.y = Random.Range(room.y + 1, room.y + room.height);

                int randamMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// 랜덤한 분대원 생성을 위한

                MonsterName monster = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList[randamMonster];
                Spawn(monster.ToString(), i,spawnPos);
            }
        }
    }

    public void BossSpawn()
    {
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StageListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// 현재 스테이지에 몬스터 분대 수 
        int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  이번 방에 어떤 분대를 생성할지
        int randomMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// 랜덤한 분대원 생성을 위한

        MonsterName monster = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList[randomMonster];
        RectInt room = mapGenerator.roomNodeInfo.allRoomList[0].roomRect;
        Vector2 spawnPos = new Vector2(room.x, room.y);        
        


        
        if (monster == MonsterName.Boss_Dragon)
        {
            var dragonVector = new Vector2(spawnPos.x + room.width * 0.5f, spawnPos.y + room.height);
            BossSpawner(monster.ToString(), dragonVector);
        }
        if (monster == MonsterName.Boss_Turtle)
        {
            var turtlePosX = Random.Range(room.x + 1, room.x + room.width);
            var turtlePosY = Random.Range(room.y + 1, room.y + room.height);
            var turtleVector = new Vector2(turtlePosX, turtlePosY);
            BossSpawner(monster.ToString(), turtleVector);
        }
    }


    public void Spawn(string _name,int _nodeNum, Vector2 vector)
    {
        string testEnemy = $"Prefabs/Enemy/{_name}";
        GameObject GO =  PhotonNetwork.Instantiate(testEnemy, vector, Quaternion.identity);
        MultiplyEnemyPower(GO);
        GO.GetComponent<EnemyAI>().roomNum = _nodeNum;
        GO.transform.parent = Case.transform;

        GameManager.Instance.MG.roomNodeInfo.allRoomList[_nodeNum].roomInMoster++;

        int viewID = GO.GetPhotonView().ViewID;
        photonView.RPC("ADDEnemyViewList", RpcTarget.All, viewID);
    }

    public void MultiplyEnemyPower(GameObject enemy)
    {
        var _enemyAI = enemy.GetComponent<EnemyAI>();
        float baseNumber = 1.3f;
        float exponent = GameManager.Instance.curStage;

        float result = (float)Math.Pow(baseNumber, exponent);
        _enemyAI.currentHP *= result;
        _enemyAI.maxHP *= result;
        _enemyAI.appliedATK += exponent;
    }

    public void BossSpawner(string _name, Vector2 vector)
    {
        StartCoroutine(WaitDoorClosed(_name, vector));
    }

    IEnumerator WaitDoorClosed(string name, Vector2 vector)
    {
        if (!GameManager.Instance.MG.roomNodeInfo.isDoorClosed)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(WaitDoorClosed(name, vector));
        }
        else
        {
            yield return new WaitForSeconds(1f);
            string testEnemy = $"Prefabs/Enemy/{name}";
            GameObject GO = PhotonNetwork.Instantiate(testEnemy, vector, Quaternion.identity);
            GO.transform.parent = Case.transform;

            int viewID = GO.GetPhotonView().ViewID;
            photonView.RPC("ADDEnemyViewList", RpcTarget.All, viewID);
        }
    }

    public void StageMonsterClear()
    {
        Debug.Log("몬스터 전부 삭제");
        GameManager.Instance.isStageEnd = true;
        foreach (Transform child in Case.transform)
        {
            PhotonNetwork.Destroy(child.gameObject);
        }
        GameManager.Instance.isStageEnd = false;
        photonView.RPC("CLEAREnemyViewList", RpcTarget.All);
    }

    [PunRPC]
    public void ADDEnemyViewList(int viewID)
    {
        EnemyViewIDList.Add(viewID);
    }

    [PunRPC]
    public void CLEAREnemyViewList()
    {
        EnemyViewIDList.Clear();
    }

    [PunRPC]
    public void RemoveEnemyViewList(int viewID)
    {
        EnemyViewIDList.Remove(viewID);
    }
}
