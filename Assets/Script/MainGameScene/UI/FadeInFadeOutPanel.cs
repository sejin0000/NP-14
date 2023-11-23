using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInFadeOutPanel : MonoBehaviour
{
    SpriteRenderer image;
    private void Awake()
    {
        image = GetComponent<SpriteRenderer>();
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn");
        StartCoroutine(FadeIn_());
    }

    IEnumerator FadeIn_()
    {
        while(image.color.a > 0)
        {
            Debug.Log("FadeIn_");
            image.color -= new Color(image.color.r, image.color.g, image.color.b, 0.05f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

    }

    public void FadeOut(int i)
    {

        Debug.Log("FadeOut");
        StartCoroutine(FadeOut_(i));
    }

    IEnumerator FadeOut_(int i)
    {
        while (image.color.a < 1)
        {
            Debug.Log("FadeOut_");

            image.color += new Color(image.color.r, image.color.g, image.color.b, 0.05f);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        if (i == 1)
        {
            GameManager.Instance.NextStageEndEvent();
        }
        else if(i == 2)
        {
            GameManager.Instance.NextGameClearEvent();
        }
        else
        {
            GameManager.Instance.NextGameOverEvent();
        }
    }
}
