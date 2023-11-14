using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.HableCurve;

public class TestAugmentBtn : MonoBehaviour
{
    public TextMeshProUGUI AugmentNameText;
    public int Augmentcode;
    public Button AugmentSelectButton;

    public void Initialize(string augmentName, int code)
    {
        AugmentNameText.text = augmentName;
        Augmentcode = code;
        AugmentSelectButton.onClick.AddListener(OnTestAugmentButtonClicked);
    }

    public void OnTestAugmentButtonClicked()
    {
        var selectedBGColor = new Color(112f / 255f, 97f / 255f, 171f / 255f);
        var selectedTextColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        GetComponent<Image>().color = selectedBGColor;
        AugmentNameText.color = selectedTextColor;
        TestAugmentManager.Instance.AugmentCall(Augmentcode);
        AugmentSelectButton.onClick.RemoveAllListeners();

    }
}
