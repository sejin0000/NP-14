using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameCamera : MonoBehaviour
{
    public GameObject Target;               // 카메라가 따라다닐 타겟

    public float offsetX = 0.0f;            // 카메라의 x좌표
    public float offsetY = 0.0f;            // 카메라의 y좌표
    public float offsetZ = -10.0f;          // 카메라의 z좌표

    public float CameraSpeed = 10.0f;       // 카메라의 속도
    Vector3 TargetPos;                      // 타겟의 위치
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

    void FixedUpdate()
    {        
        if (Target == null)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel("LobbyScene");
        }
        if (Target.GetComponent<PlayerStatHandler>().isDie)
        {
            ChangeTarget();
            DiedAfterTarget();
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
                OtherTargetPos = playerInfoDictionary[viewID].position;
                OtherTargetViewID = viewID;
            }
        }
    }

    public void ChangeTarget()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            var playerInfoDictionary = GameManager.Instance.playerInfoDictionary;
            foreach (var viewID in playerInfoDictionary.Keys)
            {
                if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID
                    && viewID != OtherTargetViewID)
                {
                    OtherTargetPos = playerInfoDictionary[viewID].position;
                }
            }
        }
    }
}
