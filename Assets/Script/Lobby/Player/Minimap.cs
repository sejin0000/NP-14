using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minimap : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 0.0f;            // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치

    private void Start()
    {
        if (MainGameManager.Instance != null)
        {
            Target = MainGameManager.Instance.InstantiatedPlayer;
        }
        else
        {
            Target = GameManager.Instance.clientPlayer;
        }
    }

    void FixedUpdate()
    {
        if (Target == null)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel("LobbyScene");
        }
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
            );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }


    /*
    public Transform player;
    private void Awake()
    {
        MeshFilter filter = GetComponent<MeshFilter>();

        NavMeshTriangulation triangles = NavMesh.CalculateTriangulation();


        Mesh mesh = new Mesh();
        mesh.vertices = triangles.vertices;
        mesh.triangles = triangles.indices;

        filter.mesh = mesh;
    }
    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
    */



}
