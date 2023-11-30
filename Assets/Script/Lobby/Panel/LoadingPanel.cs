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
    private Vector3 InitPos;
    private float InitPosX;

    private float rollSpeed;
    private float elapsedRad;
    private float loadingTime;

    public void Initialize(float LoadingTime)
    {
        this.gameObject.SetActive(true);
        loadingTime = LoadingTime;
        InitPos = PlayerRolling.GetComponent<RectTransform>().localPosition;
        InitPosX = InitPos.x;
        rollSpeed = 1.5f;
        elapsedRad = 0f;
        StartCoroutine(LoadEnd());
    }
    private void Start()
    {
        InitPosX = PlayerRolling.GetComponent<RectTransform>().localPosition.x;
        rollSpeed = 1.5f;
    }
    private void Update()
    {
        elapsedRad += Time.deltaTime * rollSpeed;
        float calculatedRad = elapsedRad - Mathf.PI / 2;
        var RectPos = PlayerRolling.GetComponent<RectTransform>();
        RectPos.localPosition = new Vector3(calculatedRad * -InitPosX, InitPos.y, 0);

        if (calculatedRad % (Mathf.PI * 2) > Mathf.PI / 2
            && calculatedRad % (Mathf.PI * 2) < Mathf.PI * 1.5f)
        {
            PlayerRolling.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            PlayerRolling.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public IEnumerator LoadEnd()
    {
        yield return new WaitForSeconds(loadingTime);
        PlayerRolling.GetComponent<RectTransform>().localPosition = InitPos;
        Debug.Log($"InitPos : {PlayerRolling.GetComponent<RectTransform>().localPosition.x}, {PlayerRolling.GetComponent<RectTransform>().localPosition.y}");
        this.gameObject.SetActive(false);
    }
}