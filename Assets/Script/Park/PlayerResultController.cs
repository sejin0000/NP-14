using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{
    ResultManager resultManager;
    PlayerInputController inputController;

    public void MakeManager() 
    {
        resultManager = ResultManager.Instance;
        inputController= GetComponent<PlayerInputController>();
        AugmentManager.Instance.startset(this.gameObject);
        MakeAugmentListManager.Instance.startset(this.gameObject);
        resultManager.startset(this.gameObject);
        inputController.OnAugmentcheck += AugMentOnOff;
    }
    public void AugMentOnOff() 
    {
        resultManager.OnOffGetList();
    }
}
