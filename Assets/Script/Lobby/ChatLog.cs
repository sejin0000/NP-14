using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatLog : MonoBehaviour
{
    public TextMeshProUGUI NickNameText;
    public TextMeshProUGUI ChatText;

    public float prefabHeight;
    public float prefabWidth;

    public void GetCurrentAmount()
    {
        prefabHeight = this.gameObject.GetComponent<RectTransform>().rect.height;
        prefabWidth = this.gameObject.GetComponent<RectTransform>().rect.width;

        Debug.Log($"º¯È¯ Àü : {prefabHeight}");
    }

    public void Initialize(string nickName, string inputText)
    {
        NickNameText.text = nickName;
        ChatText.text = inputText;
    }
    public float ConfirmTextSize(TMP_InputField textObject)
    {
        GetCurrentAmount();
        float prefabFontSize = ChatText.fontSize;
        float inputFontSize = textObject.textComponent.fontSize;
        float fontMultiplier = inputFontSize / prefabFontSize;
        float maxWidth = ChatText.gameObject.GetComponent<RectTransform>().rect.width;
        float textWidth = textObject.preferredWidth;

        if (textWidth > maxWidth)
        {            
            int sizeMultiplier = (int)(textWidth / (maxWidth * fontMultiplier)) + 1;
            this.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(prefabWidth, prefabHeight * sizeMultiplier);
            ChatText.GetComponent<RectTransform>().sizeDelta = new Vector2(maxWidth, prefabHeight * sizeMultiplier);
        }
        return gameObject.GetComponent<RectTransform>().rect.height;
    } 
}
