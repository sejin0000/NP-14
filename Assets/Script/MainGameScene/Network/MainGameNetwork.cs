using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameNetwork : MonoBehaviourPunCallbacks
{
    private bool IsSucceedOver;
    public LoadingPanel loadingPanel;

    
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        var GM = GameManager.Instance;
        if (GM.isGameOver)
        {
            return;
        }
        loadingPanel.Initialize(5f);

        GM._mansterSpawner.GetPhotonView().RPC("CLEAREnemyViewList", RpcTarget.All);

        GM.playerInfoDictionary.Clear();
        int viewID = GM.clientPlayer.GetPhotonView().ViewID;
        GM.gameObject.GetPhotonView().RPC("PlayerInfoDictionarySetting", RpcTarget.AllBuffered, viewID);

        if (PhotonNetwork.IsMasterClient)
        {
            if (GM != null) 
            {
                Debug.Log("MainGameNetwork - OnMasterClientSwitched : MasterClient");
                GM.OnStageSettingEvent += GM.MG.NavMeshBakeRunTime;
                GM.OnStageStartEvent += GM.MS.MonsterSpawn;
                GM.OnBossStageStartEvent += GM.MS.BossSpawn;
                photonView.RPC("SendSucceed", RpcTarget.All);
            }
        }
        StartCoroutine(WaitSucceed());
        GM.CallEmergencyProtocolEvent();
        GM.StageRestart();
        IsSucceedOver = false;
    }
    [PunRPC]
    public void SendSucceed()
    {
        IsSucceedOver = true;
    }

    public IEnumerator WaitSucceed()
    {
        if (!IsSucceedOver)
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(WaitSucceed());
        }
        else
        {
            Debug.Log("MainGameNetwork - Succeed Complete");
            yield return null;
        }
    }
}
