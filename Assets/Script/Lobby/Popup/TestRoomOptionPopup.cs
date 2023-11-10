using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class TestRoomOptionPopup : MonoBehaviour
{
    [Header("RoomInfo")]
    public TMP_InputField TestRoomNameInputField;
    public TMP_InputField TestRoomMemberInputField;
    public GameObject SceneScrollViewContent;    

    [Header("Buttons")]
    public Button OptionConfirmButton;
    public Button OptionCloseButton;

    [Header("OptionInfo")]
    [SerializeField] public string selectedTestScene;
    [SerializeField] public string selectedRoomMemberText;
    [SerializeField] public string currentTestScene;
    [SerializeField] public string currentRoomMemberText;
    private string folderPath;
    private string sceneEntryPath;

    [Header("OtherPanels")]
    public GameObject _TestRoomPanel;
    public TestRoomPanel testRoomPanel;

    public List<SceneConnectButton> sceneConnectButtons;



    public void Initialize()
    {
        testRoomPanel = _TestRoomPanel.GetComponent<TestRoomPanel>();
        if (selectedTestScene == null && selectedRoomMemberText == null)
        {
            selectedTestScene = testRoomPanel.currentTestScene;
            selectedRoomMemberText = testRoomPanel.currentRoomMemberText;
        }

        currentTestScene = selectedTestScene;
        currentRoomMemberText = selectedRoomMemberText;


        if (folderPath == null)
        {
            folderPath = "Assets/Scenes/TestScenes";
        }
        if (sceneEntryPath == null) 
        {
            sceneEntryPath = "Prefabs/LobbyScene/SceneConnectButton";
        }

        if (sceneConnectButtons  == null)
        {
            sceneConnectButtons = new List<SceneConnectButton>();
        }

        OptionConfirmButton.onClick.AddListener(OnOptionConfirmButtonClicked);
        OptionCloseButton.onClick.AddListener(OnOptionCloseButtonClicked);

        GetSceneArray();
    }

    public void OnOptionConfirmButtonClicked()
    {
        GetCurrentInfo();
        this.gameObject.SetActive(false);
    }

    public void OnOptionCloseButtonClicked()
    {
        this.gameObject.SetActive(false);
    }

    public void OnSceneConnectButtonClicked(SceneConnectButton clickedButton)
    {
        foreach (SceneConnectButton button in sceneConnectButtons)
        {
            if (button != clickedButton)
            {
                button.IsSelected = false;                
            }
        }
    }

    public void GetCurrentInfo()
    {
        var contentTransform = SceneScrollViewContent.transform;
        int childCount = contentTransform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            var selectButtonTransform = contentTransform.GetChild(i).gameObject;
            var selectConnect = selectButtonTransform.GetComponent<SceneConnectButton>();
            if (selectConnect.IsSelected)
            {
                selectedTestScene = selectConnect.sceneNameText.text;
                testRoomPanel.ConnectedSceneText.text = $"Selected Scene : {selectedTestScene}";
                testRoomPanel.currentTestScene = selectedTestScene;
            }
        }
        if (TestRoomMemberInputField != null) 
        {
            selectedRoomMemberText = TestRoomMemberInputField.text;
        }
        else
        {
            selectedRoomMemberText = currentRoomMemberText;
        }

    }

    public void GetSceneArray()
    {
        string[] sceneFiles = Directory.GetFiles(folderPath, "*.unity").Select(Path.GetFileNameWithoutExtension).ToArray();
        GameObject SceneEntry;
        foreach (string sceneName in sceneFiles)
        {
            SceneScrollViewContent.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 90);
            SceneEntry = Instantiate(Resources.Load<GameObject>(sceneEntryPath));  
            SceneEntry.transform.SetParent(SceneScrollViewContent.transform, false);
            var sceneConnectButton = SceneEntry.GetComponent<SceneConnectButton>();
            sceneConnectButton.Initialize(sceneName, false, this, null);
            sceneConnectButtons.Add(sceneConnectButton);
        }
        foreach (var sceneButtons in sceneConnectButtons)
        {
            sceneButtons.InitializeButtonList(sceneConnectButtons);
        }
    }
}
