using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeInfo : MonoBehaviour
{
    Node startRoom;
    Node endRoom;

    MapGenerator mapGenerator;
    PhotonView PV;

    private void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        PV = GetComponent<PhotonView>();
    }

    public void ChooseRoom()
    {
        startRoom = mapGenerator.lastRoomList[0];
        endRoom = mapGenerator.lastRoomList[mapGenerator.lastRoomList.Count - 1];
    }

    public void PlayerPositionSetting()
    {
        Vector2 _startRoom = new Vector2(startRoom.roomRect.x, startRoom.roomRect.y);
        Vector2 _widthHeight = new Vector2(startRoom.roomRect.width, startRoom.roomRect.height);

        PV.RPC("PunPlayerPositionSetting",RpcTarget.AllBuffered, _startRoom, _widthHeight);
    }

    [PunRPC]
    public void PunPlayerPositionSetting( Vector2 startRoom, Vector2 widthHeight)
    {
        Vector2 vector;

        vector.x = Random.Range(startRoom.x + 1, startRoom.x + widthHeight.x - 1);
        vector.y = Random.Range(startRoom.y + 1, startRoom.y + widthHeight.y - 1);


        GameManager.Instance.clientPlayer.transform.position = vector;
    }
}
