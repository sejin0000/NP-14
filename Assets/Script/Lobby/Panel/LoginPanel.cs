using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LoginPanel : MonoBehaviourPunCallbacks
{
    private TMP_InputField playerIdInput;
    private TMP_InputField playerPswdInput;
    private Button loginButton;

    private void Awake()
    {
        //loginButton.onClick.AddListener(OnLoginButtonClicked);
    }

    private void OnLoginButtonClicked()
    {
        // TODO : �ڳ� ���̽��� ����ϱ�� ����..
        // TODO : �ڳ� ���̽��� ID, PSWD Ȯ�� ����.


        // TODO : �г��� Ȯ�� �κ� �߰�
        if (PhotonNetwork.LocalPlayer.NickName == "") // TODO : ������ �ƴ� �ڳ� ���̽��� ���� Ȯ���ϴ� ������ �����ؾ���
        {
            // TODO : �г��� ���� �г� ����
        }
        else
        {
            // TODO : �ڳ� ���̽����� ����� �г��� ���� ��Ʈ��ũ�� ����
        }


        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // OnJoinedLobby() ����.
            LobbyManager.Instance.SetPanel(Enum.GetName(typeof(PanelType), PanelType.MainLobbyPanel));                                                  
        }
    }
}
