using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainLobbyPanel : MonoBehaviourPunCallbacks
{
    [Header("Button")]
    [SerializeField] private Button characterSelectButton;
    [SerializeField] private Button testLobbyButton;
    [SerializeField] private Button gameRoomButton;

    [Header("CharacterSelectPopup")]
    private CharacterSelectPopup playerInfo;
    private GameObject characterSelectPopup;

    [Header("Player")]
    private GameObject Player;
    private string playerPrefabPath;

    public TextMeshProUGUI Gold; // TODO : 뒤끝베이스에 골드량 추가 예정

    private void Awake()
    {
        // DESC : check CharacterSelectPopup
        if (LobbyManager.Instance.IsCharacterSelectInitialized)
        {
            characterSelectPopup = LobbyManager.Instance._CharacterSelectPopup;
            playerInfo = LobbyManager.Instance.CharacterSelect;
        }
        else
        {
            Debug.Log("LobbyManager didn't Initialize characterSelectPopup");
        }

        // DESC : connect buttons
        characterSelectButton.onClick.AddListener(playerInfo.OnCharacterButtonClicked);
        testLobbyButton.onClick.AddListener(LobbyManager.Instance.OnTestLobbyButtonClicked);
        gameRoomButton.onClick.AddListener(LobbyManager.Instance.OnGameRoomButtonClicked);

        // DESC : instantiate Player
        InstantiatePlayer();

        // DESC : 커스텀 프로퍼티 - Char_Class 추가
        LobbyManager.Instance.ClassNum = playerInfo.GetCharClass();
    }

    private void InstantiatePlayer()
    {
        LobbyManager.Instance.instantiatedPlayer = Instantiate(Resources.Load<GameObject>(PrefabPathes.PLAYER_INLOBBY_PREFAB_PATH));
    }
}
