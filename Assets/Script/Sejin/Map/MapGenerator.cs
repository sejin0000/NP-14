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

    public List<Node> parentsNodes = new List<Node>();
    public List<Node> L_childrenNode = new List<Node>();
    public List<Node> R_childrenNode = new List<Node>();
    int nodeDepth;
    void Start()
    {
        Node root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //��ü �� ũ���� ��Ʈ��带 ����
        //DrawMap(0, 0);

        Divide(root, 0);
        GenerateRoom(root, 0);


        for (int i = 0; i < maximumDepth; i++)
        {
            nodeDepth = i;
            NodeSelection(root, 0);
            ConnectAdjacentNodes();
            parentsNodes.Clear();
        }
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
        }
        else
        {
            tree.leftNode.roomRect = GenerateRoom(tree.leftNode, n + 1);
            tree.rightNode.roomRect = GenerateRoom(tree.rightNode, n + 1);
            rect = tree.leftNode.roomRect;
        }
        return rect;
    }

    private void DrawRectangle(RectInt rect)
    {
        LineRenderer lineRenderer = Instantiate(roomLine).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - mapSize / 2); //���� �ϴ�
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - mapSize / 2);//���� ���
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - mapSize / 2); //���� ���
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
            Debug.Log("�θ� �߰�");
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
            Debug.Log("�ڽ� �߰�");
        }
    }

    private void ConnectAdjacentNodes()
    {
        for (int i = 0; i < parentsNodes.Count; i++)
        {
            FindNodeChildren(parentsNodes[i].rightNode, maximumDepth - nodeDepth, R_childrenNode);
            FindNodeChildren(parentsNodes[i].leftNode, maximumDepth - nodeDepth, L_childrenNode);


            GenerateLoad(R_childrenNode, L_childrenNode);
            R_childrenNode.Clear();
            L_childrenNode.Clear();

            // �߰��� �ڽ� �����ϴ� �Լ�
        }

    }

    //ConnectAdjacentNodes

    private void GenerateLoad(List<Node> R_nodeList, List<Node> L_nodeList)
    {
        Debug.Log("�� �׸���");
        float minimumDistance = int.MaxValue;
        Node R_node = R_nodeList[0];
        Node L_node = L_nodeList[0];


        for (int i = 0; i < R_nodeList.Count; i++)
        {
            for (int j = 0; j < L_nodeList.Count; j++)
            {
                if ((R_nodeList[i].center - L_nodeList[j].center).magnitude < minimumDistance)
                {
                    minimumDistance = (R_nodeList[i].center - L_nodeList[j].center).magnitude;

                    R_node = R_nodeList[i];
                    L_node = L_nodeList[j];
                }
            }
        }
        Vector2Int leftNodeCenter = R_node.center;
        Vector2Int rightNodeCenter = L_node.center;

        DrawLine(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));
        //���� ������ leftnode�� ���缭 ���� ������ ��������.
        DrawLine(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));
    }

}