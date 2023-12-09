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
        Debug.Log("���� ���� �̺�Ʈ");
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StageListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// ���� ���������� ���� �д� �� 

        for (int i = 0; i < mapGenerator.roomNodeInfo.allRoomList.Count; i++) //�� �� ��ŭ ��ȸ
        {
            RectInt room = mapGenerator.roomNodeInfo.allRoomList[i].roomRect;
            int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  �̹� �濡 � �д븦 ��������

            for (int j = 0; j < stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterNum; j++)// ���д�� ����ŭ ��ȸ
            {
                //Debug.Log("���� ����");
                Vector2 spawnPos;
                spawnPos.x = Random.Range(room.x + 1, room.x + room.width);
                spawnPos.y = Random.Range(room.y + 1, room.y + room.height);

                int randamMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// ������ �д�� ������ ����

                MonsterName monster = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList[randamMonster];
                Spawn(monster.ToString(), i,spawnPos);
            }
        }
    }

    public void BossSpawn()
    {
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StageListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// ���� ���������� ���� �д� �� 
        int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  �̹� �濡 � �д븦 ��������
        int randamMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// ������ �д�� ������ ����

        MonsterName monster = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList[randamMonster];

        var dragonVector = new Vector2(0,10);
        var turtleVector = new Vector2(3,3);
        if (monster == MonsterName.Boss_Dragon)
        {
            BossSpawner(monster.ToString(), dragonVector);
        }
        if (monster == MonsterName.Boss_Turtle)
        {
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
        _enemyAI.appliedATK += exponent;
    }

    public void BossSpawner(string _name, Vector2 vector)
    {
        string testEnemy = $"Prefabs/Enemy/{_name}";
        GameObject GO = PhotonNetwork.Instantiate(testEnemy, vector, Quaternion.identity);
        GO.transform.parent = Case.transform;

        int viewID = GO.GetPhotonView().ViewID;
        photonView.RPC("ADDEnemyViewList", RpcTarget.All, viewID);

        // TODO : ��� �ź��� ���� ��ġ �ٸ���
    }

    public void StageMonsterClear()
    {
        Debug.Log("���� ���� ����");
        foreach (Transform child in Case.transform)
        {
            PhotonNetwork.Destroy(child.gameObject);
        }
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
