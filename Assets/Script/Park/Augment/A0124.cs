using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class A0124 : MonoBehaviourPun
{
    private GameObject A0124Prefabs;
    // Start is called before the first frame update
    private void Start()
    {
        if (photonView.IsMine) 
        {
            A0124Prefabs = Resources.Load<GameObject>("AugmentList/A1024");
            this.transform.SetSiblingIndex(0);
            Instantiate(A0124Prefabs);
            DarkEnd();
            MainGameManager.Instance.OnGameStartedEvent += DarkStart;
            MainGameManager.Instance.OnGameEndedEvent += DarkEnd;
        }

    }
    void DarkStart() 
    {
        A0124Prefabs.SetActive(true);
    }
    void DarkEnd()
    {
        A0124Prefabs.SetActive(false);
    }
}
