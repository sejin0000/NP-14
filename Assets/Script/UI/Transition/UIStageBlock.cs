using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

enum BlockType
{
    NONE,
    NORMAL,
    EVENT,
    BOSS
}

public class UIStageBlock : MonoBehaviour
{
    [SerializeField] BlockType blockType;

    private Image blockImg;

    private void Awake()
    {
        blockImg = GetComponent<Image>();
    }

    private void Start()
    {
        blockImg.color = InitColor();
    }

    public Color InitColor()
    {
        Color blockColor;
        switch (blockType)
        {
            default:
            case BlockType.NONE:
                blockColor = Color.grey;
                break;
            case BlockType.NORMAL:
                blockColor = Color.white;
                break;
            case BlockType.EVENT:
                blockColor = Color.magenta;
                break;
            case BlockType.BOSS:
                blockColor = Color.yellow;
                break;
        }

        return blockColor;
    }

    public void ResizeObject(float new_width, float new_height)
    {
        RectTransform temp = GetComponent<RectTransform>();
        temp.rect.Set(temp.rect.x, temp.rect.y, new_width, new_height);
    }
}
