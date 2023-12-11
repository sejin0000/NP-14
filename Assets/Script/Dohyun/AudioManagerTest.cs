using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerTest : MonoBehaviour
{
    List<GameObject> player;
    [SerializeField] GameManager manager;

    public void Initialize()
    {
        manager = GameManager.Instance;
        if (manager != null)
        {
            PhotonView pv = GameManager.Instance.GetComponent<PhotonView>();
            var viewID = new List<int>(GameManager.Instance.playerInfoDictionary.Keys).ToArray();

            for(int i=0; i < viewID.Length; i++)
            {
                var temp_pv = PhotonView.Find(viewID[i]);
                AudioManager.Instance.AudioLibrary.CallRoomSoundEvent(temp_pv.gameObject);
            }
            GameManager.Instance.OnStageStartEvent += PlayStageBGM;
        }
        PlayStageBGM();
    }

    void PlayStageBGM()
    {
        var temp = GameManager.Instance.stageListInfo.StagerList[GameManager.Instance.curStage];

        if (temp.stageType == StageType.bossStage)
        {
            AudioManager.PlayBGM(BGMList.Ace_Of_Bananas);
        }
        else if(temp.stageType==StageType.normalStage)
        {
            if (GameManager.Instance.curStage <= 2)
                AudioManager.PlayBGM(BGMList.Duty_Cycle_GB);
            else
                AudioManager.PlayBGM(BGMList.Strike_Witches_Get_Bitches);
        }
    }
}
