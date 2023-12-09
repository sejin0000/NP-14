using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class PlayerDebuffControl : MonoBehaviourPun
{
    public ParticleSystem _speedParticle;
    public ParticleSystem _TwoMoonParticle;
    public ParticleSystem _HealParticle;

    float speedTime = 0;
    float checkSpeedTime;

    float twoMoonTIME = 0;
    float checkMoonTime;

    float HealTime = 0;
    float checkHealTime;

    bool readyHeal;
    bool readyMoon;
    bool readySpeed;
    // Start is called before the first frame update
    public enum buffName
    {
        Speed =0,
        TwoMoon=1,
        Heal=2,
    }
    public void Init(buffName i,float time)
    {
        if (buffName.Speed == i)
        {
            photonView.RPC("SpeedBuffOn", RpcTarget.All);
            if (speedTime >= 0 && speedTime <= time)
            {
                speedTime = time;
                checkSpeedTime = 0;
                readySpeed = true;
            }
        }
        else if (buffName.TwoMoon == i)
        {
            photonView.RPC("TwoMoonBuffOn", RpcTarget.All);
            if (twoMoonTIME >= 0 && twoMoonTIME <= time)
            {
                twoMoonTIME = time;
                checkMoonTime = 0;
                readyMoon = true;
            }
        }
        else if (buffName.Heal == i) 
        {
            photonView.RPC("HealBuffOn", RpcTarget.All);
            if (HealTime >= 0 && HealTime <= time)
            {
                HealTime = time;
                checkHealTime = 0;
                readyHeal = true;
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
        if (readyHeal) 
        {
            checkHealTime += Time.deltaTime;
            if (checkHealTime >= HealTime) 
            
            {

            }
        }

    }
    private void SpeedOff() 
    {
        photonView.RPC("SpeedBuffOff", RpcTarget.All);
        checkSpeedTime = 0f;
        speedTime = 0f;
        readySpeed = false;
    }
    private void TwoMoonOff()
    {
        Debug.Log("²ô±â µé¾î¿È");
        photonView.RPC("TwoMoonBuffOff", RpcTarget.All);
        _TwoMoonParticle.gameObject.SetActive(false);
        checkMoonTime = 0f;
        twoMoonTIME = 0f;
        readyMoon = false;
    }
    private void HealOff()
    {
        photonView.RPC("HealBuffOff", RpcTarget.All);
        checkHealTime = 0f;
        HealTime = 0f;
        readyHeal = false;
    }

    [PunRPC]
    public void SpeedBuffOn()
    {
        _speedParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    public void SpeedBuffOff()
    {
        _speedParticle.gameObject.SetActive(false);
    }
    [PunRPC]
    public void TwoMoonBuffOn()
    {
        _TwoMoonParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    public void TwoMoonBuffOff()
    {
        _TwoMoonParticle.gameObject.SetActive(false);
    }
    [PunRPC]
    public void HealBuffOn()
    {
        _HealParticle.gameObject.SetActive(true);
    }
    [PunRPC]
    public void HealBuffOff()
    {
        _HealParticle.gameObject.SetActive(false);
    }
}
