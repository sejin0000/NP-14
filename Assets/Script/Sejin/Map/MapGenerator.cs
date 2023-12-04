using NavMeshPlus.Components;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public Vector2Int mapSize; //���ϴ� ���� ũ��

    [SerializeField] public Vector2Int BossMapSize; //���ϴ� ���� ũ��


    [SerializeField] float minimumDevideRate; //������ �������� �ּ� ����
    [SerializeField] float maximumDivideRate; //������ �������� �ִ� ����
    [SerializeField] private GameObject line; //lineRenderer�� ����ؼ� ������ �������� ���������� �����ֱ� ����
    [SerializeField] private GameObject map; //lineRenderer�� ����ؼ� ù ���� ����� �����ֱ� ����
    [SerializeField] private GameObject roomLine; //lineRenderer�� ����ؼ� ���� ����� �����ֱ� ����
    [SerializeField] private int maximumDepth; //Ʈ���� ����, ���� ���� ���� �� �ڼ��� ������ ��

    private List<Node> parentsNodes = new List<Node>();
    private List<Node> L_childrenNode = new List<Node>();
    private List<Node> R_childrenNode = new List<Node>();

    public List<Node> allRoomList = new List<Node>();
    public List<Node> loadRoomList = new List<Node>();
    public List<Node> lastRoomList = new List<Node>();

    int nodeDepth;

    public SetTile setTile;
    public RoomNodeInfo roomNodeInfo;

    public NavMeshSurface Surface2D;

    Node root;
    private void Awake()
    {
        setTile = GetComponent<SetTile>();
        roomNodeInfo = GetComponent<RoomNodeInfo>();
    }

    public void BossMapMake()
    {
        root = new Node(new RectInt(0, 0, BossMapSize.x, BossMapSize.y)); //��ü �� ũ���� ��Ʈ��带 ���� 

        setTile.OrderSetRectTile(new RectInt(0, 0, BossMapSize.x + 20, BossMapSize.y + 10), setTile.wallTileMap, setTile.wallTile, new Vector2(-((BossMapSize.x + 20) / 2), -((BossMapSize.y + 10) / 2)));
        setTile.OrderSetRectTile(new RectInt(0, 0, BossMapSize.x, BossMapSize.y), setTile.wallTileMap, null, new Vector2(-(BossMapSize.x / 2), -(BossMapSize.y / 2)));
        setTile.OrderSetRectTile(new RectInt(0, 0, BossMapSize.x, BossMapSize.y), setTile.groundTileMap, setTile.groundTile, new Vector2(-(BossMapSize.x / 2), -(BossMapSize.y / 2)));

        roomNodeInfo.porTal.GetComponent<Portal>().portalSetting(0, 20);
    }
    public void MapMake()
    {
        Debug.Log("MapMake");
        if (PhotonNetwork.IsMasterClient)
        {
            root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ���� 

            setTile.OrderSetRectTile(new RectInt(0, 0, mapSize.x + 20, mapSize.y + 10), setTile.wallTileMap, setTile.wallTile, new Vector2(-10, -5));

            Divide(root, 0);
            GenerateRoom(root, 0);

            RoomMake();
            allRoomList.Clear();
            for (int i = 0; i < L_childrenNode.Count; i++)
            {
                allRoomList.Add(L_childrenNode[i]);
            }
            for (int i = 0; i < R_childrenNode.Count; i++)
            {
                allRoomList.Add(R_childrenNode[i]);
            }
            lastRoomList.Clear();
            for (int i = 0; i < allRoomList.Count; i++)
            {
                if (allRoomList[i].roadCount == 1)
                {
                    lastRoomList.Add(allRoomList[i]);
                }
            }
            roomNodeInfo.ChooseRoom();
            roomNodeInfo.PlayerPositionSetting();
        }
    }

    public void RoomMake()
    {
        for (int i = 0; i < maximumDepth; i++)
        {
            parentsNodes.Clear();

            nodeDepth = i;
            NodeSelection(root, 0);
            ConnectAdjacentNodes();
        }
    }
    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; //���� ���ϴ� ���̿� �����ϸ� �� �������� �ʴ´�.

        //�� ���� ��쿡��

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        int split = Mathf.RoundToInt(UnityEngine.Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));

        if (tree.nodeRect.width >= tree.nodeRect.height)
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));       
        }
        else
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
        }
        tree.leftNode.parNode = tree;
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1);
        Divide(tree.rightNode, n + 1);
    }
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth)
        {
            rect = tree.nodeRect;
            int width = UnityEngine.Random.Range(rect.width / 2, rect.width - 1);
            int height = UnityEngine.Random.Range(rect.height / 2, rect.height - 1);
            int x = rect.x + UnityEngine.Random.Range(1, rect.width - width);
            int y = rect.y + UnityEngine.Random.Range(1, rect.height - height);
            rect = new RectInt(x, y, width, height);

            setTile.OrderSetRectTile(rect, setTile.wallTileMap,null, new Vector2(rect.x, rect.y));
            setTile.OrderSetRectTile(rect, setTile.groundTileMap,setTile.groundTile, new Vector2(rect.x, rect.y));
        }
        else 
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }

    private void NodeSelection(Node tree, int n)
    {
        if (n != (maximumDepth - (nodeDepth + 1)))
        {
            NodeSelection(tree.leftNode, n + 1);
            NodeSelection(tree.rightNode, n + 1);
        }
        else
        {
            parentsNodes.Add(tree);
        }
    }

    private void FindNodeChildren(Node tree, int n, List<Node> nodeList)
    {
        if (n != maximumDepth)
        {
            FindNodeChildren(tree.leftNode, n + 1, nodeList);
            FindNodeChildren(tree.rightNode, n + 1, nodeList);
        }
        else
        {
            nodeList.Add(tree);
        }
    }

    private void ConnectAdjacentNodes()
    {
        for (int i = 0; i < parentsNodes.Count; i++)
        {
            R_childrenNode.Clear();
            L_childrenNode.Clear();

            FindNodeChildren(parentsNodes[i].rightNode, maximumDepth - nodeDepth, R_childrenNode);
            FindNodeChildren(parentsNodes[i].leftNode, maximumDepth - nodeDepth, L_childrenNode);


            GenerateLoad(R_childrenNode, L_childrenNode);
        }

    }

    private void GenerateLoad(List<Node> R_nodeList, List<Node> L_nodeList)
    {
        float minimumDistance = int.MaxValue;
        Node R_node = R_nodeList[0];
        Node L_node = L_nodeList[0];


        for (int i = 0; i < R_nodeList.Count; i++)
        {
            for (int j = 0; j < L_nodeList.Count; j++)
            {   
                int x = Mathf.Abs(R_nodeList[i].center.x - L_nodeList[j].center.x);
                int y = Mathf.Abs(R_nodeList[i].center.y - L_nodeList[j].center.y);
                int curimumDistance = x + y;


                if ( curimumDistance <= minimumDistance )
                {
                    minimumDistance = curimumDistance;

                    R_node = R_nodeList[i];
                    L_node = L_nodeList[j];
                }
            }
        }

        R_node.roadCount += 1;
        L_node.roadCount += 1;

        RectInt R_roomRect = R_node.roomRect;
        RectInt L_roomRect = L_node.roomRect;

        int minX = Mathf.Max(R_roomRect.x, L_roomRect.x);//����
        int maxX = Mathf.Min(R_roomRect.x + R_roomRect.width, L_roomRect.x + L_roomRect.width);//����

        int minY = Mathf.Max(R_roomRect.y, L_roomRect.y);//����
        int maxY = Mathf.Min(R_roomRect.y + R_roomRect.height, L_roomRect.y + L_roomRect.height);



        if (minX < maxX)//y�� �׸���(y������ �׸����� x���� min ����max ���� �ȿ�����)
        {
            int X = minX + ((maxX - minX) / 2);

            Vector2Int startPos = new Vector2Int(X,(int)R_roomRect.center.y);
            Vector2Int endPos = new Vector2Int (X, (int)L_roomRect.center.y);

            if (startPos.y > endPos.y)//�������� �� �����
            {
                startPos.y -= R_roomRect.height / 2;
                endPos.y += L_roomRect.height / 2;
            }
            else
            {
                startPos.y += R_roomRect.height / 2;
                endPos.y -= L_roomRect.height / 2;
            }

            setTile.OrderSetLineTile(setTile.wallTileMap, null, startPos, Mathf.Abs(startPos.y - endPos.y), new Vector2(0, startPos.y - endPos.y).normalized);
            setTile.OrderSetLineTile(setTile.groundTileMap, setTile.groundTile, startPos, Mathf.Abs(startPos.y - endPos.y), new Vector2(0, startPos.y - endPos.y).normalized);

            setTile.OrderSetDoorTile(new Vector2Int(X, startPos.y - (Mathf.Abs(startPos.y - endPos.y) / 2) - 1),setTile.doorTileMap ,setTile.doorTile );
            setTile.OrderSetDoorTile(new Vector2Int(X - 1, startPos.y - (Mathf.Abs(startPos.y - endPos.y) / 2) - 1), setTile.doorTileMap, setTile.doorTile);

            startPos = new Vector2Int(X - 1, (int)R_roomRect.center.y);
            endPos = new Vector2Int(X - 1, (int)L_roomRect.center.y);


            setTile.OrderSetLineTile(setTile.wallTileMap, null, startPos, Mathf.Abs(startPos.y - endPos.y), new Vector2(0, startPos.y - endPos.y).normalized);
            setTile.OrderSetLineTile(setTile.groundTileMap, setTile.groundTile, startPos, Mathf.Abs(startPos.y - endPos.y), new Vector2(0, startPos.y - endPos.y).normalized);

        }
        else if(minY < maxY)//x�� �׸���
        {
            int Y = minY + ((maxY - minY) / 2);

            Vector2Int startPos = new Vector2Int((int)R_roomRect.center.x, Y);
            Vector2Int endPos = new Vector2Int((int)L_roomRect.center.x, Y);

            if (startPos.y > endPos.y)//�������� �� �����
            {
                startPos.x += R_roomRect.width / 2;
                endPos.x -= L_roomRect.width / 2;
            }
            else
            {
                startPos.x -= R_roomRect.width / 2;
                endPos.x += L_roomRect.width / 2;
            }


            setTile.OrderSetLineTile(setTile.wallTileMap, null, startPos, Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, 0).normalized);
            setTile.OrderSetLineTile(setTile.groundTileMap, setTile.groundTile, startPos, Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized);

            setTile.OrderSetDoorTile(new Vector2Int(startPos.x - (Mathf.Abs(startPos.x - endPos.x) / 2) - 1, Y), setTile.doorTileMap, setTile.doorTile);
            setTile.OrderSetDoorTile(new Vector2Int(startPos.x - (Mathf.Abs(startPos.x - endPos.x) / 2) - 1, Y - 1), setTile.doorTileMap, setTile.doorTile);


            startPos = new Vector2Int((int)R_roomRect.center.x, Y - 1);
            endPos = new Vector2Int((int)L_roomRect.center.x, Y - 1);

            setTile.OrderSetLineTile(setTile.wallTileMap, null, startPos, Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, 0).normalized);
            setTile.OrderSetLineTile(setTile.groundTileMap, setTile.groundTile, startPos , Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized);


        }
        else
        {
            Debug.LogError("�� ����� ����");
        }
    }

    public void NavMeshBakeRunTime()
    {
        Surface2D.BuildNavMeshAsync();
    }
}