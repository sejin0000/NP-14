using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    public AugmentManager augmentManager;
    public ResultManager resultManager1;
    public MakeAugmentListManager makeAugmentListManager1;
    private void Start()
    {
        MakeManager();
        Invoke("testcall", 3f);
    }

    public void MakeManager() 
    {
        resultManager1 = new ResultManager(this.gameObject);
        ResultManager Prefab = Resources.Load<ResultManager>("Prefabs/ResultManager");
        resultManager1 = Instantiate(Prefab);
        augmentManager = new AugmentManager(this.gameObject);

    }
    void testcall() 
    {
        resultManager1.testCallProtoResult();
        resultManager1.testCallStatResult();
    }
}
