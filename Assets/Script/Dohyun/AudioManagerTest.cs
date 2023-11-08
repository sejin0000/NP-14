using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.PlayBGM("TestBGM");
        //Fire2();
    }

    private void Fire()
    {
        Invoke("TestSFX1", 1f);
        Invoke("TestSFX2", 2f);
        Invoke("TestSFX3", 3f);
        Invoke("TestSFX1", 4f);
        Invoke("TestSFX2", 5f);
        Invoke("TestSFX3", 6f);
        Invoke("TestSFX1", 7f);
        Invoke("TestSFX2", 8f);
        Invoke("TestSFX3", 9f);
    }

    private void Fire2()
    {
        Invoke("TestSFX1", 2f);
        Invoke("TestSFX2", 3f);
        Invoke("TestSFX3", 8f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestSFX1()
    {
        AudioManager.PlaySE("gun_pistol_general_handling_04");
        Invoke("TestSFX1", 2f);
    }
    void TestSFX2()
    {
        AudioManager.PlaySE("gun_pistol_shot_05");
        Invoke("TestSFX2", 3f);
    }
    void TestSFX3()
    {
        AudioManager.PlaySE("gun_pistol_slide_fast_02");
        Invoke("TestSFX3", 8f);
    }
}
