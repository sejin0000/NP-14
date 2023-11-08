using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    public Button EnterTestRoomButton;
    public Button CreateTestRoomButton;    
    public Button BackButton;

    [Header("CurrentRoomBoard")]
    [SerializeField] private GameObject ScrollViewContent;

    [Header("CreateRoomBoard")]
    [SerializeField] private TMP_InputField RoomNameSetup;
    [SerializeField] private TMP_InputField RoomMemberSetup;



    private void Start()
    {
        EnterTestRoomButton.onClick.AddListener(OnEnterTestRoomButtonClicked);
        CreateTestRoomButton.onClick.AddListener(OnCreateTestRoomButtonClicked);            
        BackButton.onClick.AddListener(OnBackButtonClicked);
    }

    public void ShowTestRoomListEntries()
    {

    }

    #region Button
    private void OnEnterTestRoomButtonClicked()
    {

    }

    private void OnCreateTestRoomButtonClicked()
    {

    }

    private void OnBackButtonClicked()
    {

    }
    #endregion
}
