using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestRoomOptionPopup : MonoBehaviour
{
    [Header("RoomInfo")]
    public TMP_InputField TestRoomNameInputField;
    public TMP_InputField TestRoomMemberInputField;
    public GameObject TestScrollViewContent;

    [Header("Buttons")]
    public Button OptionConfirmButton;
    public Button OptionCloseButton;
}
