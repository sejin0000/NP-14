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
        // TODO : �ڳ� ���̽��� ����ϱ�� ����..
        // TODO : �ڳ� ���̽��� ID, PSWD Ȯ�� ����.       
        PhotonNetwork.LocalPlayer.NickName = playerIdInput.text;

        // TODO : �г��� Ȯ�� �κ� �߰�
        if (PhotonNetwork.LocalPlayer.NickName == "") // TODO : ������ �ƴ� �ڳ� ���̽��� ���� Ȯ���ϴ� ������ �����ؾ���
        {
            // TODO : �г��� ���� �г� ����
            PhotonNetwork.LocalPlayer.NickName = $"Player{Random.Range(1, 200)}";
        }
        else
        {
            // TODO : �ڳ� ���̽����� ����� �г��� ���� ��Ʈ��ũ�� ����
        }


        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();             
        }
    }
}
