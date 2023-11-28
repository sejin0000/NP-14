using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public GameObject Case;

    private void Start()
    {
        GameManager.Instance.OnStageEndEvent += StageMonsterClear;
    }

    public void MonsterSpawn()
    {
        Debug.Log("몬스터 생성");
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StagerListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// 현재 스테이지에 몬스터 분대 수 






        for (int i = 0; i < mapGenerator.roomNodeInfo.allRoomList.Count; i++) //방 수 만큼 순회
        {
            RectInt room = mapGenerator.roomNodeInfo.allRoomList[i].roomRect;
            int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  이번 방에 어떤 분대를 생성할지






            for (int j = 0; j < stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterNum; j++)// 스분대원 수만큼 순회
            {
                Vector2 spawnPos;
                spawnPos.x = Random.Range(room.x + 1, room.x + room.width);
                spawnPos.y = Random.Range(room.y + 1, room.y + room.height);

                int randamMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// 랜덤한 분대원 생성을 위한

                MonsterName monster = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList[randamMonster];
                Spawn(monster.ToString(), i,spawnPos);
            }
        }
    }

    public void Spawn(string _name,int _nodeNum, Vector2 vector)
    {
        string testEnemy = $"Prefabs/Enemy/{_name}";
        GameObject GO =  PhotonNetwork.Instantiate(testEnemy, vector, Quaternion.identity);
        GO.GetComponent<EnemyAI>().roomNum = _nodeNum;
        GO.transform.parent = Case.transform;
        GameManager.Instance.MG.roomNodeInfo.allRoomList[_nodeNum].roomInMoster++;
    }

    public void StageMonsterClear()
    {
        Debug.Log("몬스터 전부 삭제");
        foreach (Transform child in Case.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
