using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebuffControl : MonoBehaviour
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
            _speedParticle.gameObject.SetActive(true);
            if (speedTime >= 0 && speedTime <= time)
            {
                speedTime = time;
                checkSpeedTime = 0;
                readySpeed = true;
            }
        }
        else 
        {
            _TwoMoonParticle.gameObject.SetActive(true);
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
            Debug.Log($"지속시간앞으로 { checkSpeedTime}");
            if (checkSpeedTime >= speedTime)
            {
                SpeedOff();
            }
        }

    }
    private void SpeedOff() 
    {
        _speedParticle.gameObject.SetActive(false);
        checkSpeedTime = -1f;
        speedTime = -1f;
        readySpeed = false;
    }
    private void TwoMoonOff()
    {
        _speedParticle.gameObject.SetActive(false);
        checkMoonTime = -1f;
        twoMoonTIME = -1f;
        readyMoon = false;
    }
}
