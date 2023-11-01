using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatLog : MonoBehaviour
{
    public TextMeshProUGUI NickNameText;
    public TextMeshProUGUI ChatText;

    public void ConfirmTextSize(TMP_InputField textObject)
    {
        float maxWidth = ChatText.gameObject.GetComponent<RectTransform>().rect.width;        
        float textWidth = textObject.preferredWidth;

        if (textWidth > maxWidth)
        {
            Debug.Log("개행 발생");
            int sizeMultiplier = (int)(textWidth / maxWidth);
            LayoutElement layoutElement = ChatText.GetComponent<LayoutElement>();
            LayoutElement parentElement = this.gameObject.GetComponent<LayoutElement>();
            if (layoutElement != null)
            {
                layoutElement.preferredWidth *= sizeMultiplier;
                parentElement.preferredWidth *= sizeMultiplier;
            }
        }        
    } 
}
