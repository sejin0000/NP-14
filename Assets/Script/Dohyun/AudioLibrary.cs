using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CustomClip
{
    [SerializeField] string name;
    [SerializeField] AudioClip audio;
}

public class AudioLibrary : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private AudioClip loby;
    [SerializeField] private List<AudioClip> stage;
    [SerializeField] private List<AudioClip> boss;

    [Header("Player")]
    [SerializeField] private AudioClip player_rolling;
    [SerializeField] private AudioClip player_hit;

    private AudioClip player_attack;
    private AudioClip reloadStart;
    private AudioClip reloadFinish;

    [Header("Enemy")]
    [SerializeField] private AudioClip enemy_attack;
    [SerializeField] private AudioClip enemy_hit;

    [Header("Enviorment")]
    [SerializeField] private AudioClip DoorOpen;
    [SerializeField] private AudioClip DoorClose;

    [Header("UI")]
    [SerializeField] private AudioClip click;

    private GameObject player;

    private void Awake()
    {
        
    }

    void Start()
    {       
        player = GameManager.Instance.clientPlayer;
        SetupPlayerSE();
        AttachPlayerSE();
    }

    void SetupPlayerSE()
    {
        var stats = player.GetComponent<PlayerStatHandler>();
        player_attack = stats.atkClip;
        reloadStart = stats.reloadStartClip;
        reloadFinish = stats.reloadFinishClip;
    }

    void AttachPlayerSE()
    {
        var controller = player.GetComponent<PlayerInputController>();
        var stats = player.GetComponent<PlayerStatHandler>();

        // 조작 관련 효과음
        controller.OnAttackEvent += PlayShotSE;
        controller.OnRollEvent += PlayRollingSE;
        controller.OnReloadEvent += PlayReloadStartSE;
        controller.OnEndReloadEvent += PlayReloadFinishSE;

        // 피격 판정 효과음
        stats.HitEvent += PlayHitSE;
    }

    public void PlayMonsterDead()
    {
        AudioManager.PlayClip(enemy_hit);
    }

    void PlayShotSE()
    {
        AudioManager.PlayClip(player_attack);
    }

    void PlayRollingSE()
    {
        AudioManager.PlayClip(player_rolling);
    }

    void PlayHitSE()
    {
        AudioManager.PlayClip(player_hit);
    }

    void PlayReloadStartSE()
    {
       AudioManager.PlayClip(reloadStart);
    }

    void PlayReloadFinishSE()
    {
        AudioManager.PlayClip(reloadFinish);
    }
}
