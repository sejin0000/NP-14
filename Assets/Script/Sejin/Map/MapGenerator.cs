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
    [SerializeField] Vector2Int mapSize; //원하는 맵의 크기
    [SerializeField] float minimumDevideRate; //공간이 나눠지는 최소 비율
    [SerializeField] float maximumDivideRate; //공간이 나눠지는 최대 비율
    [SerializeField] private GameObject line; //lineRenderer를 사용해서 공간이 나눠진걸 시작적으로 보여주기 위함
    [SerializeField] private GameObject map; //lineRenderer를 사용해서 첫 맵의 사이즈를 보여주기 위함
    [SerializeField] private GameObject roomLine; //lineRenderer를 사용해서 방의 사이즈를 보여주기 위함
    [SerializeField] private int maximumDepth; //트리의 높이, 높을 수록 방을 더 자세히 나누게 됨

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
        root = new Node(new RectInt(0, 0, mapSize.x, mapSize.y)); //전체 맵 크기의 루트노드를 만듬
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
        lineRenderer.SetPosition(0, new Vector2(rect.x, rect.y) - mapSize / 2); //좌측 하단
        lineRenderer.SetPosition(1, new Vector2(rect.x + rect.width, rect.y) - mapSize / 2); //우측 하단
        lineRenderer.SetPosition(2, new Vector2(rect.x + rect.width, rect.y + rect.height) - mapSize / 2);//우측 상단
        lineRenderer.SetPosition(3, new Vector2(rect.x, rect.y + rect.height) - mapSize / 2); //좌측 상단
    }
    private void DrawMap(int x, int y) //x y는 화면의 중앙위치를 뜻함
    {
        //기본적으로 mapSize/2라는 값을 계속해서 빼게 될건데, 화면의 중앙에서 화면의 크기의 반을 빼줘야 좌측 하단좌표를 구할 수 있기 때문이다.
        LineRenderer lineRenderer = Instantiate(map).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); //좌측 하단
        lineRenderer.SetPosition(1, new Vector2(x + mapSize.x, y) - mapSize / 2); //우측 하단
        lineRenderer.SetPosition(2, new Vector2(x + mapSize.x, y + mapSize.y) - mapSize / 2);//우측 상단
        lineRenderer.SetPosition(3, new Vector2(x, y + mapSize.y) - mapSize / 2); //좌측 상단
    }
    void Divide(Node tree, int n)
    {
        if (n == maximumDepth) return; //내가 원하는 높이에 도달하면 더 나눠주지 않는다.

        //그 외의 경우에는

        int maxLength = Mathf.Max(tree.nodeRect.width, tree.nodeRect.height);
        //가로와 세로중 더 긴것을 구한후, 가로가 길다면 위 좌, 우로 세로가 더 길다면 위, 아래로 나눠주게 될 것이다.
        int split = Mathf.RoundToInt(Random.Range(maxLength * minimumDevideRate, maxLength * maximumDivideRate));
        //나올 수 있는 최대 길이와 최소 길이중에서 랜덤으로 한 값을 선택

        if (tree.nodeRect.width >= tree.nodeRect.height) //가로가 더 길었던 경우에는 좌 우로 나누게 될 것이며, 이 경우에는 세로 길이는 변하지 않는다.
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, split, tree.nodeRect.height));
            //왼쪽 노드에 대한 정보다 
            //위치는 좌측 하단 기준이므로 변하지 않으며, 가로 길이는 위에서 구한 랜덤값을 넣어준다.
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x + split, tree.nodeRect.y, tree.nodeRect.width - split, tree.nodeRect.height));
            //우측 노드에 대한 정보다 
            //위치는 좌측 하단에서 오른쪽으로 가로 길이만큼 이동한 위치이며, 가로 길이는 기존 가로길이에서 새로 구한 가로값을 뺀 나머지 부분이 된다.
            // DrawLine(new Vector2(tree.nodeRect.x + split, tree.nodeRect.y), new Vector2(tree.nodeRect.x + split, tree.nodeRect.y + tree.nodeRect.height));
            //그 후 위 두개의 노드를 나눠준 선을 그리는 함수이다.        
        }
        else
        {
            tree.leftNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y, tree.nodeRect.width, split));
            tree.rightNode = new Node(new RectInt(tree.nodeRect.x, tree.nodeRect.y + split, tree.nodeRect.width, tree.nodeRect.height - split));
            //  DrawLine(new Vector2(tree.nodeRect.x , tree.nodeRect.y+ split), new Vector2(tree.nodeRect.x + tree.nodeRect.width, tree.nodeRect.y  + split));
            //세로가 더 길었던 경우이다. 자세한 사항은 가로와 같다.
        }
        tree.leftNode.parNode = tree; //자식노드들의 부모노드를 나누기전 노드로 설정
        tree.rightNode.parNode = tree;
        Divide(tree.leftNode, n + 1); //왼쪽, 오른쪽 자식 노드들도 나눠준다.
        Divide(tree.rightNode, n + 1);//왼쪽, 오른쪽 자식 노드들도 나눠준다.
    }
    private void DrawLine(Vector2 from, Vector2 to) //from->to로 이어지는 선을 그리게 될 것이다.
    {
        LineRenderer lineRenderer = Instantiate(line).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2);
        lineRenderer.SetPosition(1, to - mapSize / 2);
    }
    private RectInt GenerateRoom(Node tree, int n)
    {
        RectInt rect;
        if (n == maximumDepth) //해당 노드가 리프노드라면 방을 만들어 줄 것이다.
        {
            rect = tree.nodeRect;
            int width = Random.Range(rect.width / 2, rect.width - 1);
            //방의 가로 최소 크기는 노드의 가로길이의 절반, 최대 크기는 가로길이보다 1 작게 설정한 후 그 사이 값중 랜덤한 값을 구해준다.
            int height = Random.Range(rect.height / 2, rect.height - 1);
            //높이도 위와 같다.
            int x = rect.x + Random.Range(1, rect.width - width);
            //방의 x좌표이다. 만약 0이 된다면 붙어 있는 방과 합쳐지기 때문에,최솟값은 1로 해주고, 최댓값은 기존 노드의 가로에서 방의 가로길이를 빼 준 값이다.
            int y = rect.y + Random.Range(1, rect.height - height);
            //y좌표도 위와 같다.
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


        DrawLine(new Vector2(rightNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, rightNodeCenter.y));//코너부분,시작부분

        Vector2Int startPos = new Vector2Int(rightNodeCenter.x, rightNodeCenter.y);
        Vector2Int endPos = new Vector2Int(rightNodeCenter.x, leftNodeCenter.y);

        setTile.RemoveLineTile(setTile.wallTileMap, startPos,Mathf.Abs(startPos.y - endPos.y), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);



        DrawLine(new Vector2(leftNodeCenter.x, leftNodeCenter.y), new Vector2(rightNodeCenter.x, leftNodeCenter.y));//시작부분,코너부분

        startPos = new Vector2Int(leftNodeCenter.x, leftNodeCenter.y);
        endPos = new Vector2Int(rightNodeCenter.x, leftNodeCenter.y);

        setTile.RemoveLineTile(setTile.wallTileMap, startPos, Mathf.Abs(startPos.x - endPos.x), new Vector2(startPos.x - endPos.x, startPos.y - endPos.y).normalized, mapSize / 2);


        //if (Mathf.Abs(rightNodeCenter.x - leftNodeCenter.x) > Mathf.Abs(rightNodeCenter.y - leftNodeCenter.y))//y축으로 이어야하는 경우
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