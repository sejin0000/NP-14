using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect; //분리된 공간의 rect정보
    public RectInt roomRect; //분리된 공간 속 방의 rect정보

    public bool thisRoomClear = false;
    public bool roomInPlayer = false;

    List<GameObject> roomInMoster = new List<GameObject>();

    public int roadCount = 0;

    public Vector2Int center
    {
        get
        {
            return new Vector2Int(roomRect.x + roomRect.width / 2, roomRect.y + roomRect.height / 2);
        }
        //방의 가운데 점. 방과 방을 이을 때 사용
    }

    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }

}