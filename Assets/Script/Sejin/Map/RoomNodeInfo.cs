using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNodeInfo : MonoBehaviour
{
    public GameObject porTal;

    public List<Node> allRoomList;
    Node startRoom;
    Node endRoom;

    PhotonView PV;
    public MapGenerator mapGenerator;

    private void Awake()
    {
        mapGenerator = GetComponent<MapGenerator>();
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        GameManager.Instance.OnRoomStartEvent += CloseDoor;
        GameManager.Instance.OnRoomEndEvent += OpenDoor;
    }

    public void ChooseRoom()
    {
        startRoom = mapGenerator.lastRoomList[0];
        mapGenerator.lastRoomList[0].thisRoomClear = true;
        endRoom = mapGenerator.lastRoomList[mapGenerator.lastRoomList.Count - 1];

        if(porTal == null)
        {
            porTal = PhotonNetwork.Instantiate("prefabs/portal", new Vector3(endRoom.roomRect.x + endRoom.roomRect.width / 2, endRoom.roomRect.y + endRoom.roomRect.height / 2),Quaternion.identity);
        }
        else
        {
            PV.RPC("PunPortalSetting",RpcTarget.AllBuffered);
            porTal.SetActive(true);
            porTal.transform.position = new Vector3(endRoom.roomRect.x + endRoom.roomRect.width / 2, endRoom.roomRect.y + endRoom.roomRect.height / 2);
        }
        mapGenerator.lastRoomList[mapGenerator.lastRoomList.Count - 1].thisRoomClear = true;

        allRoomList = mapGenerator.allRoomList;
        allRoomList.Remove(startRoom);
        allRoomList.Remove(endRoom);

    }



    public void PlayerPositionSetting()
    {
        Vector2 _startRoom = new Vector2(startRoom.roomRect.x, startRoom.roomRect.y);
        Vector2 _widthHeight = new Vector2(startRoom.roomRect.width, startRoom.roomRect.height);

        PV.RPC("PunPlayerPositionSetting", RpcTarget.AllBuffered, _startRoom, _widthHeight);
    }

    [PunRPC]
    private void PunPlayerPositionSetting(Vector2 startRoom, Vector2 widthHeight)
    {
        Vector2 vector;

        vector.x = Random.Range(startRoom.x + 1, startRoom.x + widthHeight.x - 1);
        vector.y = Random.Range(startRoom.y + 1, startRoom.y + widthHeight.y - 1);


        GameManager.Instance.clientPlayer.transform.position = vector;
    }

    public void CloseDoor()
    {
        PV.RPC("PunCloseDoor",RpcTarget.All);
    }

    [PunRPC]
    private void PunCloseDoor()
    {
        mapGenerator.setTile.doorTileMap.gameObject.SetActive(true);
    }

    public void OpenDoor()
    {
        PV.RPC("PunCloseDoor", RpcTarget.All);
    }

    [PunRPC]
    private void PunOpenDoor()
    {
        mapGenerator.setTile.doorTileMap.gameObject.SetActive(false);
    }

    [PunRPC]
    private void PunPortalSetting()
    {
        porTal.SetActive(true);
    }
}
