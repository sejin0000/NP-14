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
        
        GameManager.Instance.OnStageStartEvent += Open;
    }
}
