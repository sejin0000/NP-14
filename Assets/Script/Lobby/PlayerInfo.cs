using Photon.Pun;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;

public enum Char_Class_Kor
{
    솔져,
    샷견,
    저격수,
}
public class PlayerInfo : MonoBehaviour
{
    public GameObject PlayerInfoPrefab;
    public TextMeshProUGUI PlayerNickNameText;
    public Sprite PlayerSprite;

    public GameObject PlayerPrefab;
    public Animator CharSampleAnim;

    public Button CharChangeLeftButton;
    public Button CharChangeRightButton;

    public ScrollRect playerStatScrollRect;
    public GameObject PlayerStatScrollContent;
    public TextMeshProUGUI playerClassText;
    public GameObject playerStat;
    public TextMeshProUGUI playerSkillText;
    private int initCharType;
    private int curCharType;

    void Start()
    {
        initCharType = GetCharClass();
        curCharType = initCharType;
        UpdateCharInfo();
        CharChangeLeftButton.onClick.AddListener(() => OnLeftButtonClicked());
        CharChangeRightButton.onClick.AddListener(() => OnRightButtonClicked());

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

        // 직업명 적용
        playerClassText.text = $"<{ Enum.GetName(typeof(Char_Class_Kor), curCharType)}>";

        // 캐릭터 스탯 적용
        // 스탯이 있다고 가정
        Sample_Player player = new Sample_Player(curCharType);
        Type type = player.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (FieldInfo field in fields)
        {
            GameObject go = Instantiate(PlayerPrefab);
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = field.Name;
            texts[1].text = field.GetValue(player).ToString();

            // 프리팹은 Scene에다가 오브젝트로 생성하는 건데, transform을 가져오는 과정에서 world 좌표를 유지하는 옵션을 켰기 때문에 오류가 나타남.
            go.transform.SetParent(PlayerStatScrollContent.transform,false);
            go.transform.localScale = Vector3.one;
            go.SetActive(true);
        }
    }

    #region button
    public void OnLeftButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType -= (curCharType != 0) ? 1 : -classNumber;
        Debug.Log(curCharType);
        UpdateCharInfo();
    }

    public void OnRightButtonClicked()
    {
        int classNumber = Enum.GetNames(typeof(LobbyPanel.CharClass)).Length - 1;
        curCharType += (curCharType != classNumber) ? 1 : -classNumber;
        Debug.Log(curCharType);
        UpdateCharInfo();
    }

    public void OnConfirmButtonClicked()
    {

    }
    #endregion
}
