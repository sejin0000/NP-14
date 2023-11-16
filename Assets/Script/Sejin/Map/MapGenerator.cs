using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize; //���ϴ� ���� ũ��
    [SerializeField] float minimumDevideRate; //������ �������� �ּ� ����
    [SerializeField] float maximumDivideRate; //������ �������� �ִ� ����
    [SerializeField] private GameObject line; //lineRenderer�� ����ؼ� ������ �������� ���������� �����ֱ� ����
    [SerializeField] private GameObject map; //lineRenderer�� ����ؼ� ù ���� ����� �����ֱ� ����
    [SerializeField] private GameObject roomLine; //lineRenderer�� ����ؼ� ���� ����� �����ֱ� ����
    [SerializeField] private int maximumDepth; //Ʈ���� ����, ���� ���� ���� �� �ڼ��� ������ ��

    private List<Node> parentsNodes = new List<Node>();
    private List<Node> L_childrenNode = new List<Node>();
    private List<Node> R_childrenNode = new List<Node>();

    public List<Node> AllRoomList = new List<Node>();

    int nodeDepth;

    private SetTile setTile;

    Node root;
    private void Awake()
    {
        setTile = GetComponent<SetTile>();
    }

    void Start()
    {
        root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ����
        DrawMap(0, 0);

        setTile.SetRectTile(root.nodeRect, setTile.groundTile, setTile.groundTileMap);
        setTile.SetRectTile(new RectInt(0, 0, mapSize.x + 20, mapSize.y + 10), setTile.wallTile, setTile.wallTileMap);

        Divide(root, 0);
        GenerateRoom(root, 0);

        MapMake();

        for(int i = 0; i < L_childrenNode.Count; i++)
        {
            AllRoomList.Add(L_childrenNode[i]);
        }
        for (int i = 0; i < R_childrenNode.Count; i++)
        {
            AllRoomList.Add(R_childrenNode[i]);
        }
    }


    public void MapMake()
    {
        for (int i = 0; i < maximumDepth; i++)
        {
            parentsNodes.Clear();

            nodeDepth = i;
            NodeSelection(root, 0);
            ConnectAdjacentNodes();
        }
    }


    private void DrawRectangle(RectInt rect)
    {
        LineRenderer lineRenderer = Instantiate(roomLine).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - mapSize / 2); //���� ���
    }
    private void DrawMap(int x, int y) //x y�� ȭ���� �߾���ġ�� ����
    {
        //�⺻������ mapSize/2��� ���� ����ؼ� ���� �ɰǵ�, ȭ���� �߾ӿ��� ȭ���� ũ���� ���� ����� ���� �ϴ���ǥ�� ���� �� �ֱ� �����̴�.
        LineRenderer lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //���� ���
    }
    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; //���� ���ϴ� ���̿� �����ϸ� �� �������� �ʴ´�.

        //�� ���� ��쿡��

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        //���ο� ������ �� ����� ������, ���ΰ� ��ٸ� �� ��, ��� ���ΰ� �� ��ٸ� ��, �Ʒ��� �����ְ� �� ���̴�.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));
        //���� �� �ִ� �ִ� ���̿� �ּ� �����߿��� �������� �� ���� ����

        if (tree.nodeRect.width >= tree.nodeRect.height) //���ΰ� �� ����� ��쿡�� �� ��� ������ �� ���̸�, �� ��쿡�� ���� ���̴� ������ �ʴ´�.
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴ� �����̹Ƿ� ������ ������, ���� ���̴� ������ ���� �������� �־��ش�.
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
            //���� ��忡 ���� ������ 
            //��ġ�� ���� �ϴܿ��� ���������� ���� ���̸�ŭ �̵��� ��ġ�̸�, ���� ���̴� ���� ���α��̿��� ���� ���� ���ΰ��� �� ������ �κ��� �ȴ�.
            // DrawLine(new Vector2(tree.nodeRect.x + split, tree.nodeRect.y), new Vector2(tree.nodeRect.x + split, tree.nodeRect.y + tree.nodeRect.height));
            //�� �� �� �ΰ��� ��带 ������ ���� �׸��� �Լ��̴�.        
        }
        else
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
            //  DrawLine(new Vector2(tree.nodeRect.x , tree.nodeRect.y+ split), new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y  + split));
            //���ΰ� �� ����� ����̴�. �ڼ��� ������ ���ο� ����.
        }
        tree.leftNode.parNode = tree; //�ڽĳ����� �θ��带 �������� ���� ����
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1); //����, ������ �ڽ� ���鵵 �����ش�.
        Divide(tree.rightNode, n + 1);//����, ������ �ڽ� ���鵵 �����ش�.
    }
    private void DrawLine(Vector2 from, Vector2 to) //from->to�� �̾����� ���� �׸��� �� ���̴�.
    {
        LineRenderer lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);
    }
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth) //�ش� ��尡 ��������� ���� ����� �� ���̴�.
        {
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            //���� ���� �ּ� ũ��� ����� ���α����� ����, �ִ� ũ��� ���α��̺��� 1 �۰� ������ �� �� ���� ���� ������ ���� �����ش�.
            int height = Random.Range(rect.height / 2, rect.height - 1);
            //���̵� ���� ����.
            int x = rect.x + Random.Range(1, rect.width - width);
            //���� x��ǥ�̴�. ���� 0�� �ȴٸ� �پ� �ִ� ��� �������� ������,�ּڰ��� 1�� ���ְ�, �ִ��� ���� ����� ���ο��� ���� ���α��̸� �� �� ���̴�.
            int y = rect.y + Random.Range(1, rect.height - height);
            //y��ǥ�� ���� ����.
            rect = new RectInt(x, y, width, height);
            DrawRectangle(rect);

            setTile.RemoveRectTile(rect, setTile.wallTileMap, new Vector2(rect.x, rect.y) - mapSize / 2);
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

    //ConnectAdjacentNodes

    private void GenerateLoad(List<Node> R_nodeList, List<Node> L_nodeList)
    {
        float minimumDistance = int.MaxValue;
        Node R_node = R_nodeList[0];
        Node L_node = L_nodeList[0];


        for (int i = 0; i < R_nodeList.Count; i++)
        {
            for (int j = 0; j < L_nodeList.Count; j++)
            {
                if (
                    Mathf.Abs(
                    R_nodeList[i].center.x - L_nodeList[j].center.x
                    )
                    +
                    Mathf.Abs(
                    R_nodeList[i].center.y - L_nodeList[j].center.y
                    )
                    < minimumDistance
                    )
                {
                    minimumDistance = (R_nodeList[i].center - L_nodeList[j].center).magnitude;

                    R_node = R_nodeList[i];
                    L_node = L_nodeList[j];
                }
            }
        }


        Vector2Int rightNodeCenter = R_node.center;
        Vector2Int leftNodeCenter = L_node.center;


        DrawLine(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));//�ڳʺκ�,���ۺκ�

        Vector2Int startPos = new Vector2Int(rightNodeCenter.x, rightNodeCenter.y);
        Vector2Int endPos = new Vector2Int(rightNodeCenter.x, leftNodeCenter.y);

        setTile.RemoveLineTile(setTile.wallTileMap, startPos,Mathf.Abs(startPos.y - endPos.y), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);



        DrawLine(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));//���ۺκ�,�ڳʺκ�

        startPos = new Vector2Int(leftNodeCenter.x, leftNodeCenter.y);
        endPos = new Vector2Int(rightNodeCenter.x, leftNodeCenter.y);

        setTile.RemoveLineTile(setTile.wallTileMap, startPos, Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);


        //if (Mathf.Abs(rightNodeCenter.x - leftNodeCenter.x) > Mathf.Abs(rightNodeCenter.y - leftNodeCenter.y))//y������ �̾���ϴ� ���
        //{
        //    int Center = (rightNodeCenter.y + leftNodeCenter.y) / 2;

        //    Vector2Int startPos = new Vector2Int(rightNodeCenter.x - (R_node.roomRect.width / 2), Center);
        //    Vector2Int endPos = new Vector2Int(leftNodeCenter.x + (L_node.roomRect.width / 2), Center);


        //    DrawLine(startPos, endPos);
        //    setTile.RemoveLineTile(setTile.wallTileMap, startPos, (startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);
        //}
        //else
        //{
        //    int Center = (rightNodeCenter.x + leftNodeCenter.x) / 2;

        //    Vector2Int startPos = new Vector2Int(Center, rightNodeCenter.y - (R_node.roomRect.height / 2));
        //    Vector2Int endPos = new Vector2Int(Center, leftNodeCenter.y + (L_node.roomRect.height / 2));

        //    DrawLine(startPos, endPos);
        //    setTile.RemoveLineTile(setTile.wallTileMap, startPos, (startPos.y - endPos.y), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);
        //}
    }
}