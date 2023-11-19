using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResultController : MonoBehaviour
{
    public void MakeManager()
    {
        TestAugmentManager.Instance.startset(this.gameObject);
        TestMakeAugmentListManager.Instance.startset(this.gameObject);
    }
}
