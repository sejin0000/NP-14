using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerTest : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameManager manager;
    

    // Start is called before the first frame update
    void Start()
    {
        //if (manager != null)
        //{
        //    Debug.Log("AudioManagerTest - PlayStageBGM �̺�Ʈ ����");
        //    GameManager.Instance.OnStageStartEvent += PlayStageBGM;
        //}
    }

    //private IEnumerator WaitGameManager()
    //{
    //    if (GameManager.Instance == null) 
    //    {
    //        Debug.Log("AudioManagerTest - Waiting");
    //        yield return new WaitForSeconds(1f);
    //        StartCoroutine(WaitGameManager());
    //    }
    //    Debug.Log("AudioManagerTest - PlayStageBGM �̺�Ʈ ����");
    //    GameManager.Instance.OnStageStartEvent += PlayStageBGM;
    //    yield return null;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStageBGM()
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
