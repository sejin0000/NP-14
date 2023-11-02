using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatLog : MonoBehaviour
{
    public TextMeshProUGUI NickNameText;
    public TextMeshProUGUI ChatText;

    private float prefabHeight;
    private float prefabWidth;

    public void Initialize()
    {
        prefabHeight = this.gameObject.GetComponent<RectTransform>().rect.height;
        prefabWidth = this.gameObject.GetComponent<RectTransform>().rect.width;
    }
    public void ConfirmTextSize(TMP_InputField textObject)
    {
        Initialize();
        float prefabFontSize = ChatText.fontSize;
        float inputFontSize = textObject.textComponent.fontSize;
        float fontMultiplier = inputFontSize / prefabFontSize;
        float maxWidth = ChatText.gameObject.GetComponent<RectTransform>().rect.width;
        float textWidth = textObject.preferredWidth;

        Debug.Log($"chat : {ChatText.text}");
        Debug.Log($"textWidth : {textWidth}");
        Debug.Log($"maxWidth : {maxWidth}");

        if (textWidth > maxWidth)
        {            
            int sizeMultiplier = (int)(textWidth / (maxWidth * fontMultiplier)) + 1;
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(prefabWidth, prefabHeight * sizeMultiplier);
            ChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, prefabHeight * sizeMultiplier);
        }        
    } 
}
