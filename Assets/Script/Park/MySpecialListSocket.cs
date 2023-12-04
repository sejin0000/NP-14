using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MySpecialListSocket : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Func;
    public void Init(string name, string func)
    {
        Name.text = name;
        Func.text = func;
    }
}
