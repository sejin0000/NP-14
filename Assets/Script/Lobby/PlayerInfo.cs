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
    솔져,
    샷견,
    저격수,
}

public class PlayerInfo : MonoBehaviour
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

    // 플레이어 번호 반환
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

    // 플레이어 정보 적용
    private void UpdateCharInfo()
    {
        // 캐릭터 샘플 애니메이션 적용
        // Load 하는 거 안되서 일단 보류
        //CharSampleAnim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(
        //    Resources.Load($"Animations\\Player_Idle_Sample_{curCharType}")
        //    );
        CharSampleAnim.SetInteger("curNum", curCharType);
        string playerClass = Enum.GetName(typeof(Char_Class_Kor), curCharType);

        // 직업명 적용
        playerClassText.text = $"<{playerClass}>";

        // 캐릭터 스탯 적용
        // 스탯이 있다고 가정
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
            // child 오브젝트가 둘다 같을 경우, 배열로 불러와서 처리함.
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = field.Name;
            texts[1].text = field.GetValue(player).ToString();

            sizeDelta.y += 60;
            // 프리팹은 Scene에다가 오브젝트로 생성하는 건데, transform을 가져오는 과정에서 world 좌표를 유지하는 옵션을 켰기 때문에 오류가 나타남.
            go.transform.SetParent(PlayerStatScrollContent.transform, false);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }

        statRect.sizeDelta = sizeDelta;
        StartCoroutine(DelayedLayoutRebuild(statRect));

        // 캐릭터 스킬 적용
        playerSkillText.text = $"{playerClass}의 스킬에 대한 설명입니다.";
    }

    #region button
    public void OnLeftButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType -= (curCharType != 0) ? 1 : -classNumber;
        Debug.Log($"왼쪽 클릭 후 : {curCharType}");
        UpdateCharInfo();
    }

    public void OnRightButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType += (curCharType != classNumber) ? 1 : -classNumber;
        Debug.Log($"오른쪽 클릭 후 : {curCharType}");
        UpdateCharInfo();
    }

    public void OnConfirmButtonClicked()
    {
        // 커스텀 프로퍼티 저장
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Char_Class", curCharType } });
        SetClassType(curCharType);

        // 팝업 닫기
        gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        // 커스텀 프로퍼티 저장
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "Char_Class", initCharType } });

        // 팝업 닫기
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
        yield return null; // 1프레임 대기
        LayoutRebuilder.ForceRebuildLayoutImmediate(statRect); // 레이아웃 강제 재구성
    }

    public void SetClassType(int charType)
    {
        PlayerStatHandler statSO = player.GetComponentInChildren<PlayerStatHandler>();
        SpriteLibrary playerSpriteLib = player.transform.GetChild(0).GetComponentInChildren<SpriteLibrary>();
        SpriteLibrary playerWeaponSpriteLib = player.transform.GetChild(0).GetChild(0).GetChild(1).GetComponentInChildren<SpriteLibrary>();
        switch (charType) 
        {
            case (int)LobbyPanel.CharClass.Soldier:
                statSO.CharacterChange(soldierSO);
                playerSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Player{(int)LobbyPanel.CharClass.Soldier + 1}");
                playerWeaponSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Weapon{(int)LobbyPanel.CharClass.Soldier + 1}");
                break;
            case (int)LobbyPanel.CharClass.Shotgun:
                statSO.CharacterChange(shotGunSO);
                playerSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Player{(int)LobbyPanel.CharClass.Shotgun + 1}");
                playerWeaponSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Weapon{(int)LobbyPanel.CharClass.Shotgun + 1}");
                break;
            case (int)LobbyPanel.CharClass.Sniper:
                statSO.CharacterChange(sniperSO);
                playerSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Player{(int)LobbyPanel.CharClass.Sniper + 1}");
                playerWeaponSpriteLib.spriteLibraryAsset = Resources.Load<SpriteLibraryAsset>($"SpriteLibrary/Weapon{(int)LobbyPanel.CharClass.Sniper + 1}");
                break;
        }
    }
    #endregion
}
