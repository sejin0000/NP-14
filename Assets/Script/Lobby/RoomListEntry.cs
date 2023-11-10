using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListEntry : MonoBehaviour
{
    public TextMeshProUGUI PublishedRoomNameText;
    public TextMeshProUGUI PublishedRoomMemberText;
    public Button SelectRoomButton;

    public string roomName;
    [SerializeField] public bool isEntryClicked;

    void Start()
    {
        isEntryClicked = false;
        SelectRoomButton.onClick.AddListener(OnSelectRoomButtonClicked);
    }

    // Update is called once per frame
    public void OnSelectRoomButtonClicked()
    {
        roomName = PublishedRoomNameText.text;
        isEntryClicked = true;
    }

    public void Initialize(string name, byte currentPlayers, byte maxPlayers)
    {
        roomName = name;
        PublishedRoomNameText.text = name;
        PublishedRoomMemberText.text = $"{currentPlayers} / {maxPlayers}";
    }
}
