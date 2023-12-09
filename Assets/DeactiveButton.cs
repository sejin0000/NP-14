using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeactiveButton : MonoBehaviour
{
    public Button SpawnButton;
    public Button AugmentButton;
    private bool isActive;

    private void Awake()
    {
        isActive = true;
    }

    public void OnButtonDeactive()
    {
        isActive = !isActive;
        SpawnButton.gameObject.SetActive(isActive);
        AugmentButton.gameObject.SetActive(isActive);
    }
}
