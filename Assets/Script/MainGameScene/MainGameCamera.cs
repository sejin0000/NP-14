using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCamera : MonoBehaviour
{
    public GameObject Target;               // ī�޶� ����ٴ� Ÿ��

    public float offsetX = 0.0f;            // ī�޶��� x��ǥ
    public float offsetY = 0.0f;            // ī�޶��� y��ǥ
    public float offsetZ = -10.0f;          // ī�޶��� z��ǥ

    public float CameraSpeed = 10.0f;       // ī�޶��� �ӵ�
    Vector3 TargetPos;                      // Ÿ���� ��ġ
    Vector3 OtherTargetPos;
    int OtherTargetViewID;

    private void Start()
    {
        Debug.Log("MainGameCamera - Start");
        if(MainGameManager.Instance != null)
        {
            Target = MainGameManager.Instance.InstantiatedPlayer;             
        }
        else
        {
            Debug.Log("adaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Target = GameManager.Instance.clientPlayer;
        }
    }

    private void Update()
    {
        ChangeTarget();
    }

    void FixedUpdate()
    {        
        if (Target == null)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel("LobbyScene");
        }
        if (Target.GetComponent<PlayerStatHandler>().isDie)
        {
            //ChangeTarget();
            //DiedAfterTarget();
            TargetPos = OtherTargetPos;
        }
        else
        {
            TargetPos = new Vector3(
                Target.transform.position.x + offsetX,
                Target.transform.position.y + offsetY,
                Target.transform.position.z + offsetZ
                );
        }

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }

    public void DiedAfterTarget()
    {
        var playerInfoDictionary = GameManager.Instance.playerInfoDictionary;
        foreach (var viewID in playerInfoDictionary.Keys)
        {
            if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID)
            {
                OtherTargetPos = new Vector3(playerInfoDictionary[viewID].position.x, playerInfoDictionary[viewID].position.y, -10f);
                OtherTargetViewID = viewID;
            }
        }
    }

    public void UpdateDiedView()
    {

    }

    public void ChangeTarget()
    {
        //���� Ÿ�� ���� ������Ʈ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("��� ���� ��ȯ��");
            var playerInfoDictionary = GameManager.Instance.playerInfoDictionary;
            foreach (var viewID in playerInfoDictionary.Keys)
            {
                if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID
                    && viewID != OtherTargetViewID)
                {
                    OtherTargetPos = new Vector3(playerInfoDictionary[viewID].position.x, playerInfoDictionary[viewID].position.y, -10f);
                    OtherTargetViewID = viewID;
                }
            }
        }
    }
}
