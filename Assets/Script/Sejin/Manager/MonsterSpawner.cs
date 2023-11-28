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
        Debug.Log("���� ����");
        MapGenerator mapGenerator = GameManager.Instance.MG;
        StagerListInfoSO stagerListInfoSO = GameManager.Instance.stageListInfo;

        int MonsterSquadTypeCount = stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList.Count;// ���� ���������� ���� �д� �� 






        for (int i = 0; i < mapGenerator.roomNodeInfo.allRoomList.Count; i++) //�� �� ��ŭ ��ȸ
        {
            RectInt room = mapGenerator.roomNodeInfo.allRoomList[i].roomRect;
            int randomSquad = Random.Range(0, MonsterSquadTypeCount);//  �̹� �濡 � �д븦 ��������






            for (int j = 0; j < stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterNum; j++)// ���д�� ����ŭ ��ȸ
            {
                Vector2 spawnPos;
                spawnPos.x = Random.Range(room.x + 1, room.x + room.width);
                spawnPos.y = Random.Range(room.y + 1, room.y + room.height);

                int randamMonster = Random.Range(0, stagerListInfoSO.StagerList[GameManager.Instance.curStage].MonsterSquadList[randomSquad].MonsterList.Count);// ������ �д�� ������ ����

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
        Debug.Log("���� ���� ����");
        foreach (Transform child in Case.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
