using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerDebuffControl : MonoBehaviourPun
{
    public ParticleSystem _speedParticle;
    public ParticleSystem _TwoMoonParticle;
    float speedTime = 0;
    float checkSpeedTime;
    float twoMoonTIME = 0;
    float checkMoonTime;
    bool readyMoon;
    bool readySpeed;
    // Start is called before the first frame update
    public enum buffName
    {
        Speed =0,
        TwoMoon=1
    }
    public void Init(buffName i,float time)
    {
        if (0 == i)
        {
            photonView.RPC("TwoMoonBuffOn", RpcTarget.All);
            if (speedTime >= 0 && speedTime <= time)
            {
                speedTime = time;
                checkSpeedTime = 0;
                readySpeed = true;
            }
        }
        else 
        {
            photonView.RPC("SpeedBuffOn", RpcTarget.All);
            if (twoMoonTIME > 0 && twoMoonTIME <= time)
            {
                speedTime = time;
                checkMoonTime = 0;
                readyMoon = true;
            }
        }

    }
    private void Update()
    {
        if (readyMoon) 
        {
            checkMoonTime += Time.deltaTime;
            if (checkMoonTime >= twoMoonTIME)
            {
                TwoMoonOff();
            }
        }
        if (readySpeed)
        {
            checkSpeedTime += Time.deltaTime;
            if (checkSpeedTime >= speedTime)
            {
                SpeedOff();
            }
        }

    }
    private void SpeedOff() 
    {
        photonView.RPC("SpeedBuffOff", RpcTarget.All);
        _speedParticle.gameObject.SetActive(false);
        checkSpeedTime = 0f;
        speedTime = 0f;
        readySpeed = false;
    }
    private void TwoMoonOff()
    {
        photonView.RPC("TwoMoonBuffOff", RpcTarget.All);
        checkMoonTime = 0f;
        twoMoonTIME = 0f;
        readyMoon = false;
    }

    [PunRPC]
    private void SpeedBuffOn() 
    {
        _speedParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    private void SpeedBuffOff()
    {
        _speedParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    private void TwoMoonBuffOn()
    {
        _TwoMoonParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    private void TwoMoonBuffOff()
    {
        _TwoMoonParticle.gameObject.SetActive(true);
    }
}
