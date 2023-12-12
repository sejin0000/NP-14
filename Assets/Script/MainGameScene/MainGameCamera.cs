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
        SetInitialTarget();
    }

    private void SetInitialTarget()
    {
        if (MainGameManager.Instance != null)
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
            UpdateDiedView();
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


    public void UpdateDiedView()
    {
        var playerInfoDictionary = GameManager.Instance.playerInfoDictionary;
        foreach (var viewID in playerInfoDictionary.Keys)
        {
            if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID)
            {
                OtherTargetPos = new Vector3(playerInfoDictionary[viewID].position.x, playerInfoDictionary[viewID].position.y, offsetZ);
                OtherTargetViewID = viewID;
                break; // ù ��° �ٸ� �÷��̾ �����ϵ��� ����
            }
        }
    }

    public void ChangeTarget()
    {
            var playerInfoDictionary = GameManager.Instance.playerInfoDictionary;

        //���� Ÿ�� ���� ������Ʈ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool foundNewTarget = false;
            foreach (var viewID in playerInfoDictionary.Keys)
            {
                if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID
                    && viewID != OtherTargetViewID)
                {
                    OtherTargetPos = new Vector3(playerInfoDictionary[viewID].position.x, playerInfoDictionary[viewID].position.y, offsetZ);
                    OtherTargetViewID = viewID;
                    foundNewTarget = true;
                    break; // ù ��° �ٸ� �÷��̾ �����ϵ��� ����
                }
            }

            // Q �Է� �� �ٸ� �÷��̾ ã�� ���� ��� �ʱ� Ÿ��
            if (!foundNewTarget)
            {
                SetInitialTarget();
            }
        }
    }
}
