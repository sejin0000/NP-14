using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class UIStageBlock : MonoBehaviour
{
    private Image blockImg;

    private void Awake()
    {
        blockImg = GetComponent<Image>();
    }

    public void SetColor(StageType type)
    {
        switch (type)
        {
            case StageType.normalStage:
                blockImg.color = Color.white;
                break;
            case StageType.bossStage:
                blockImg.color = Color.red;
                break;
            default:
                blockImg.color = Color.white;
                break;
        }
    }

    public void ResizeObject(float new_width, float new_height)
    {
        RectTransform temp = GetComponent<RectTransform>();
        temp.rect.Set(temp.rect.x, temp.rect.y, new_width, new_height);
    }
}
