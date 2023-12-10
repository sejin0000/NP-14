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
        if (GameManager.Instance.curStage == 0)
            AudioManager.PlayBGM(BGMList.Duty_Cycle_GB);
        else if (GameManager.Instance.curStage >= 1)
            AudioManager.PlayBGM(BGMList.Strike_Witches_Get_Bitches);
    }

    //private void Fire()
    //{
    //    Invoke("TestSFX1", 1f);
    //    Invoke("TestSFX2", 2f);
    //    Invoke("TestSFX3", 3f);
    //    Invoke("TestSFX1", 4f);
    //    Invoke("TestSFX2", 5f);
    //    Invoke("TestSFX3", 6f);
    //    Invoke("TestSFX1", 7f);
    //    Invoke("TestSFX2", 8f);
    //    Invoke("TestSFX3", 9f);
    //}

    //private void Fire2()
    //{
    //    Invoke("TestSFX1", 2f);
    //    Invoke("TestSFX2", 3f);
    //    Invoke("TestSFX3", 8f);
    //}

    //void TestSFX1()
    //{
    //    AudioManager.PlaySE("gun_pistol_general_handling_04");
    //    Invoke("TestSFX1", 2f);
    //}
    //void TestSFX2()
    //{
    //    AudioManager.PlaySE("gun_pistol_shot_05");
    //    Invoke("TestSFX2", 3f);
    //}
    //void TestSFX3()
    //{
    //    AudioManager.PlaySE("gun_pistol_slide_fast_02");
    //    Invoke("TestSFX3", 8f);
    //}
}
