using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainGame : UIBase
{

    public override void Initialize()
    {
        Debug.Log("[UIMainGame] Initialize");
        MainGameManager.Instance.OnPlayingStateChanged += OpenWindow;
    }

    private void OpenWindow()
    {
        this.Open();
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
