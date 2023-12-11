using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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

    private GameObject player;

    [HideInInspector]
    public event Action OnRoomSoundEvent;
        

    void Start()
    {
        if (GameManager.Instance != null)
        {
            player = GameManager.Instance.clientPlayer;
            SetupPlayerSE();
            AttachPlayerSE();
        }
    }


    // ADDED 
    public void CallRoomSoundEvent(GameObject newPlayer)
    {
        player = newPlayer;
        OnRoomSoundEvent += SetupPlayerSE;
        OnRoomSoundEvent += AttachPlayerSE;
        OnRoomSoundEvent?.Invoke();
    }

    // ADDED
    public void CallLobbySoundEvent()
    {
        if (player != null)
        {
            OnRoomSoundEvent -= SetupPlayerSE;
            OnRoomSoundEvent -= AttachPlayerSE;
            player = null;
        }

        AudioManager.PlayBGM(BGMList.Dragao_Inkomodo);
    }
    
    public void SetupPlayerSE()
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

    public void PlayMonsterAttack()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, enemy_attack.name);
    }

    public void PlayMonsterDead()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, enemy_hit.name);
    }

    void PlayShotSE()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, player_attack.name); 
    }

    void PlayRollingSE()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, player_rolling.name); 
    }

    void PlayHitSE()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, player_hit.name); 
    }

    void PlayReloadStartSE()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, reloadStart.name);
    }

    void PlayReloadFinishSE()
    {
        var pv = gameObject.GetPhotonView();
        pv.RPC("SpreadClip", RpcTarget.All, reloadFinish.name);
    }

    [PunRPC]
    public void SpreadClip(string name)
    {
        AudioManager.PlaySE(name);
    }
}
