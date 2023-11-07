using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    public AugmentManager augmentManager;
    public ResultManager resultManager;
    public MakeAugmentListManager makeAugmentListManager;

    public void MakeManager() 
    {
        AugmentManager.Instance.startset(this.gameObject);
        ResultManager.Instance.startset(this.gameObject);
        MakeAugmentListManager.Instance.startset(this.gameObject);

    }
}
