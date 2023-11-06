using Photon.Pun;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.U2D.Animation;

public enum Char_Class_Kor
{
    ����,
    ����,
    ���ݼ�,
}

public class PlayerInfo : MonoBehaviourPun
{
    [Header("PlayerInfo")]
    public GameObject PlayerInfoPrefab;
    public GameObject PartyBox;

    [Header("Button")]
    public Button CharChangeLeftButton;
    public Button CharChangeRightButton;
    public Button ConfirmButton;
    public Button BackButton;

    [Header("CharacterStatInfo")]
    public ScrollRect playerStatScrollRect;
    public GameObject PlayerStatScrollContent;
    public TextMeshProUGUI playerClassText;
    public Animator CharSampleAnim;
    public GameObject StatInfoPrefab;
    public TextMeshProUGUI playerSkillText;

    [Header("PlayerSO")]    
    public PlayerSO soldierSO;
    public PlayerSO shotGunSO;
    public PlayerSO sniperSO;

    [Header("Player")]
    public GameObject player;

    private int initCharType;
    private int curCharType;
    public int viewID;

    void Start()
    {
        CharChangeLeftButton.onClick.AddListener(() => OnLeftButtonClicked());
        CharChangeRightButton.onClick.AddListener(() => OnRightButtonClicked());
        ConfirmButton.onClick.AddListener(() => OnConfirmButtonClicked());
        BackButton.onClick.AddListener(() => OnBackButtonClicked());

        playerStatScrollRect.normalizedPosition = new Vector2(1f, 1f);
        Vector2 size = playerStatScrollRect.content.sizeDelta;
        size.y = 1000f;
        playerStatScrollRect.content.sizeDelta = size;

    }
        
    void Update()
    {
        
    }

    // �÷��̾� ��ȣ ��ȯ
    public int GetCharClass()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object charNumberObj))
        {
            if (charNumberObj is int charNumber)
            {
                return charNumber;
            }
            return 0;
        }
        else 
        {
            Hashtable charInitialProps = new Hashtable() { { "Char_Class", 0 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(charInitialProps);
            return 0;
        }
    }

    // �÷��̾� ���� ����
    private void UpdateCharInfo()
    {
        CharSampleAnim.SetInteger("curNum", curCharType);
        string playerClass = Enum.GetName(typeof(Char_Class_Kor), curCharType);

        // ������ ����
        playerClassText.text = $"<{playerClass}>";

        // ĳ���� ���� ����
        // ������ �ִٰ� ����
        Sample_Player player = new Sample_Player(curCharType);
        Type type = player.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (Transform child in PlayerStatScrollContent.transform)
        {
            Destroy(child.gameObject); 
        }
        
        RectTransform statRect = PlayerStatScrollContent.GetComponent<RectTransform>();
        Vector2 sizeDelta = statRect.sizeDelta;
        sizeDelta.y = 70;

        foreach (FieldInfo field in fields)
        {
            GameObject go = Instantiate(StatInfoPrefab);
            // child ������Ʈ�� �Ѵ� ���� ���, �迭�� �ҷ��ͼ� ó����.
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = field.Name;
            texts[1].text = field.GetValue(player).ToString();

            sizeDelta.y += 60;
            // �������� Scene���ٰ� ������Ʈ�� �����ϴ� �ǵ�, transform�� �������� �������� world ��ǥ�� �����ϴ� �ɼ��� �ױ� ������ ������ ��Ÿ��.
            go.transform.SetParent(PlayerStatScrollContent.transform, false);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }

        statRect.sizeDelta = sizeDelta;
        StartCoroutine(DelayedLayoutRebuild(statRect));

        // ĳ���� ��ų ����
        playerSkillText.text = $"{playerClass}�� ��ų�� ���� �����Դϴ�.";
    }

    #region button
    public void OnLeftButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType -= (curCharType != 0) ? 1 : -classNumber;
        Debug.Log($"���� Ŭ�� �� : {curCharType}");
        UpdateCharInfo();
    }

    public void OnRightButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType += (curCharType != classNumber) ? 1 : -classNumber;
        Debug.Log($"������ Ŭ�� �� : {curCharType}");
        UpdateCharInfo();
    }

    public void OnConfirmButtonClicked()
    {
        // Ŀ���� ������Ƽ ����
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Char_Class", curCharType } });
        
        // ����
        if (PhotonNetwork.InLobby)
        {
            SetClassType(curCharType);
        }
        if (PhotonNetwork.InRoom)
        {
            var classIdentifier = player.GetComponent<ClassIdentifier>();
            classIdentifier.ClassChangeApply(curCharType);
            player.GetComponent<PhotonView>().RPC("ApplyClassChange", RpcTarget.Others, curCharType, viewID);
        }

        // �˾� �ݱ�
        gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        // Ŀ���� ������Ƽ ���� (���� ������)
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Char_Class", initCharType } });

        // �˾� �ݱ�
        gameObject.SetActive(false);
    }

    public void OnCharacterButtonClicked()
    {
        this.gameObject.SetActive(true);
        initCharType = GetCharClass();
        curCharType = initCharType;
        UpdateCharInfo();
    }
    #endregion

    #region Utilty
    private System.Collections.IEnumerator DelayedLayoutRebuild(RectTransform statRect)
    {
        yield return null; // 1������ ���
        LayoutRebuilder.ForceRebuildLayoutImmediate(statRect); // ���̾ƿ� ���� �籸��
    }

    public void SetClassType(int charType, GameObject playerGo = null)
    {
        PlayerStatHandler statSO;
        if (playerGo != null) 
        {
            Debug.Log($"���� ������Ʈ : {playerGo.name}");
            statSO = playerGo.GetComponent<PlayerStatHandler>();
        }
        else
        {
            Debug.Log($"���� ������Ʈ : PlayerContainer");
            statSO = player.GetComponentInChildren<PlayerStatHandler>();
        }

        switch (charType) 
        {
            case (int)LobbyPanel.CharClass.Soldier:
                statSO.CharacterChange(soldierSO);
                break;
            case (int)LobbyPanel.CharClass.Shotgun:
                statSO.CharacterChange(shotGunSO);
                break;
            case (int)LobbyPanel.CharClass.Sniper:
                statSO.CharacterChange(sniperSO);
                break;
        }
    }
    #endregion
}
