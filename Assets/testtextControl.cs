using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class testtextControl : MonoBehaviour
{
    public TextMeshProUGUI target;


    // Update is called once per frame
    void Update()
    {
        target.fontSize += 20 * Time.deltaTime;
    }
}
