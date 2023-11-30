using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoginPanel : MonoBehaviourPunCallbacks
{
    [Header("Login")]
    public GameObject LoginBox;
    [SerializeField] private TMP_InputField playerIdInput;
    [SerializeField] private TMP_InputField playerPswdInput;
    [SerializeField] private Button loginButton;

    [Header("Background Image")]
    [SerializeField] private GameObject Trio;
    [SerializeField] private GameObject Tower;

    [Header("Start")]
    [SerializeField] private GameObject StartLogo;
    private bool isClicked;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        isClicked = false;
    }

    private void Update()
    {
        while (!isClicked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartLogo.SetActive(false);
                Trio.SetActive(true);
                LoginBox.SetActive(true);
                isClicked = true;
            }
        }
    }

    private void OnLoginButtonClicked()
    {
        // TODO : 뒤끝 베이스와 사용하기로 결정..
        // TODO : 뒤끝 베이스로 ID, PSWD 확인 가능.       
        PhotonNetwork.LocalPlayer.NickName = playerIdInput.text;

        // TODO : 닉네임 확인 부분 추가
        if (PhotonNetwork.LocalPlayer.NickName == "") // TODO : 포톤이 아닌 뒤끝 베이스를 통해 확인하는 것으로 변경해야함
        {
            // TODO : 닉네임 설정 패널 오픈
            PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(1, 200)}";
        }
        else
        {
            // TODO : 뒤끝 베이스에서 저장된 닉네임 포톤 네트워크에 적용
        }


        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();             
        }
    }
}
