using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainGame : UIBase
{
    public override void Initialize()
    {
        Debug.Log("[UIMainGame] Initialize");

        if (SceneManager.GetActiveScene().name == "Test_DoHyun")
            Open();
        else
            MainGameManager.Instance.OnPlayingStateChanged += Open;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
