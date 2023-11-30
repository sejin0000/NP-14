using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    GameObject player;
    [SerializeField] GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        //player = MainGameManager.Instance.gameObject;
        player = manager.clientPlayer;
        SetupBGM();
        SetupSE();
        //AudioManager.PlayBGM("TestBGM");
        //Fire2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetupBGM()
    {
        AudioManager.PlayBGM("Duty Cycle GB");
    }

    void SetupSE()
    {
        player.GetComponent<PlayerInputController>().OnAttackEvent += PlayShotSE;
        player.GetComponent<PlayerInputController>().OnRollEvent += PlayRollingSE;
        player.GetComponent<PlayerInputController>().OnEndReloadEvent += PlayReloadSE;
    }

    void PlayShotSE()
    {
        AudioManager.PlaySE("player_atk_soldier");
    }

    void PlayRollingSE()
    {
        AudioManager.PlaySE("player_rolling");
    }

    void PlayReloadSE()
    {
        AudioManager.PlaySE("player_reload_soldier");
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
