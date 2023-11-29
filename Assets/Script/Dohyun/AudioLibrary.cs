using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomClip
{
    [SerializeField] string name;
    [SerializeField] AudioClip audio;
}

public class AudioLibrary : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] public AudioClip loby;
    [SerializeField] public AudioClip stage1;
    [SerializeField] public AudioClip stage2;
    [SerializeField] public AudioClip boss1;


    [Header("Player")]
    [SerializeField] public AudioClip attack;
    [SerializeField] public AudioClip rolling;
    [SerializeField] public AudioClip reload;

    [Header("Enemy")]
    [SerializeField] public AudioClip hit;

    [Header("Enviorment")]
    [SerializeField] public AudioClip DoorOpen;
    [SerializeField] public AudioClip DoorClose;

    [Header("UI")]
    [SerializeField] public AudioClip onMouse;
    [SerializeField] public AudioClip click;
    [SerializeField] public AudioClip buzzer;

    [Header("Effect")]
    [SerializeField] public AudioClip a;

    private void Awake()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
