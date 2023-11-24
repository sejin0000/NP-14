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
        // TODO : 뒤끝 베이스와 사용하기로 결정..
        // TODO : 뒤끝 베이스로 ID, PSWD 확인 가능.


        // TODO : 닉네임 확인 부분 추가
        if (PhotonNetwork.LocalPlayer.NickName == "") // TODO : 포톤이 아닌 뒤끝 베이스를 통해 확인하는 것으로 변경해야함
        {
            // TODO : 닉네임 설정 패널 오픈
        }
        else
        {
            // TODO : 뒤끝 베이스에서 저장된 닉네임 포톤 네트워크에 적용
        }


        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // OnJoinedLobby() 실행.
            LobbyManager.Instance.SetPanel(Enum.GetName(typeof(PanelType), PanelType.MainLobbyPanel));                                                  
        }
    }
}
