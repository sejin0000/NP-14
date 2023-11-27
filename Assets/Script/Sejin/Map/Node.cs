using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node leftNode;
    public Node rightNode;
    public Node parNode;
    public RectInt nodeRect; //�и��� ������ rect����
    public RectInt roomRect; //�и��� ���� �� ���� rect����

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
        //���� ��� ��. ��� ���� ���� �� ���
    }

    public Node(RectInt rect)
    {
        this.nodeRect = rect;
    }

}