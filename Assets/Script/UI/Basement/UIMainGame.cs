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
            GameManager.Instance.OnStageStartEvent += Open;
    }

    public override void Open()
    {
        gameObject.SetActive(true);
    }

    public override void Close()
    {
        gameObject.SetActive(false);
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
