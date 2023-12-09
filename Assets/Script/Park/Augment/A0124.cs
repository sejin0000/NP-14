using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A0124 : MonoBehaviourPun
{
    private GameObject blindeye;
    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine) 
        {
            GameObject A0124Prefabs = Resources.Load<GameObject>("AugmentList/A1024");
            A0124Prefabs.transform.SetSiblingIndex(0);
            blindeye = Instantiate(A0124Prefabs);
            //DarkEnd();
            GameManager.Instance.OnStageStartEvent += DarkStart;
            GameManager.Instance.OnBossStageStartEvent += DarkStart;
            GameManager.Instance.OnStageEndEvent += DarkEnd;
            GameManager.Instance.OnBossStageEndEvent += DarkEnd;
        }

    }
    void DarkStart() 
    {
        blindeye.SetActive(true);
    }
    void DarkEnd()
    {
        blindeye.SetActive(false);
    }
}
