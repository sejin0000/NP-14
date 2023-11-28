using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoginPanel : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField playerIdInput;
    [SerializeField] private TMP_InputField playerPswdInput;
    [SerializeField] private Button loginButton;

    private void Awake()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
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
