using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffControl : MonoBehaviour
{
    ParticleSystem _ParticleSystem;
    float time;
    float targetTime;
    // Start is called before the first frame update
    void Start()
    {
        _ParticleSystem=GetComponent<ParticleSystem>();
        targetTime = _ParticleSystem.main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > targetTime) 
        {
            Destroy(gameObject);
        }
    }
}
