using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeInfo : MonoBehaviour
{
    Node StartRoom;
    Node EndRoom;

    public void ChooseRoom()
    {
        MapGenerator mapGenerator = GetComponent<MapGenerator>();

        StartRoom = mapGenerator.lastRoomList[0];
        EndRoom = mapGenerator.lastRoomList[mapGenerator.lastRoomList.Count - 1];
    }
}
