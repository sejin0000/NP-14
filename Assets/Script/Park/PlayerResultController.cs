using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    public AugmentManager augmentManager;
    public ResultManager resultManager;

    public void MakeManager() 
    {
        ResultManager Prefab = Resources.Load<ResultManager>("Prefabs/ResultManager");
        resultManager = Instantiate(Prefab);
        augmentManager = new AugmentManager(this.gameObject);
    }
}
