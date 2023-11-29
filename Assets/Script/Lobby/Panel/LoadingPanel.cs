using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviourPun
{
    public TextMeshProUGUI TipText;
    public GameObject PlayerRolling;
    private float InitPosX;
    private Vector3 direction;
    private float rollSpeed;

    private void Start()
    {
        InitPosX = PlayerRolling.GetComponent<RectTransform>().localPosition.x;
        Debug.Log(InitPosX);
        rollSpeed = 2.5f;
    }
    private void Update()
    {
        var RectPos = PlayerRolling.GetComponent<RectTransform>();
        RectPos.localPosition = new Vector3 (Mathf.Sin(Time.time * rollSpeed - 90) * -InitPosX, RectPos.localPosition.y, 0);

        if ((Time.deltaTime * rollSpeed - 90) % 360 > 90
            && (Time.deltaTime * rollSpeed - 90) % 360 < 270)
        {
            PlayerRolling.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            PlayerRolling.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
