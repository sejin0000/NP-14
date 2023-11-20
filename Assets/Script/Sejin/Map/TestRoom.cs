using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoom : MonoBehaviour
{
    public GameObject startPos;
    public GameObject endPos;

    

    public void ChooseRoom()
    {
        MapGenerator mapGenerator = GetComponent<MapGenerator>();
        startPos.transform.position = (Vector2)mapGenerator.lastRoomList[0].center - (mapGenerator.mapSize / 2);
        endPos.transform.position = (Vector2)mapGenerator.lastRoomList[mapGenerator.lastRoomList.Count - 1].center - (mapGenerator.mapSize / 2);
    }
}
