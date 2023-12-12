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
    int currentOtherTargetViewID;

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

        currentOtherTargetViewID = GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID;
    }

    private void Update()
    {
        if (Target == null)
        {
            PhotonNetwork.AutomaticallySyncScene = false;
            PhotonNetwork.LoadLevel("LobbyScene");
        }
        if (Target.GetComponent<PlayerStatHandler>().isDie)
        {
            ChangeTarget();
            //UpdateDiedView();
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

    void FixedUpdate()
    {        

    }

    //메인 카메라 지 , 늠 , 늠2 , 타겟만 바꾸자
    public void UpdateDiedView()
    {
        //업데이트 타겟은 항상 그냥 
    }

    public void ChangeTarget()
    {
            var playerInfoDictionary = GameManager.Instance.playerInfoDictionary; //게임매니저에서 플레이어 인포 딕셔너리 받아옴

        //여따 타겟 포스 업데이트
        if (Input.GetKeyDown(KeyCode.Q)) // 키 누르면 
        {
            bool foundNewTarget = false;
            foreach (var viewID in playerInfoDictionary.Keys)
            {
                if (viewID != GameManager.Instance.clientPlayer.gameObject.GetPhotonView().ViewID //내가 아니거나, 현재 보고 있는 타겟이 아닌 경우에만 작동
                    && viewID != currentOtherTargetViewID)
                {
                    OtherTargetPos = new Vector3(playerInfoDictionary[viewID].position.x, playerInfoDictionary[viewID].position.y, offsetZ);
                    currentOtherTargetViewID = viewID;
                    foundNewTarget = true;
                    break; // 첫 번째 다른 플레이어만 선택하도록 변경
                }
            }

            // Q 입력 시 다른 플레이어를 찾지 못한 경우 초기 타겟
            if (!foundNewTarget)
            {
                SetInitialTarget();
            }
        }

        //아무 입력도 없는경우 OtherTargetPos를 현재 타겟으로 계속해서 업데이트
        if (currentOtherTargetViewID != null)
            OtherTargetPos = new Vector3(playerInfoDictionary[currentOtherTargetViewID].position.x, playerInfoDictionary[currentOtherTargetViewID].position.y, offsetZ);
    }
}
