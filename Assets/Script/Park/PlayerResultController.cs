using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResultController : MonoBehaviour
{

    public void MakeManager() 
    {
        AugmentManager.Instance.startset(this.gameObject);
        MakeAugmentListManager.Instance.startset(this.gameObject);
        ResultManager.Instance.startset(this.gameObject);
        Debug.Log("µé¾î¿È¼ÂÆÃ");
    }
}
