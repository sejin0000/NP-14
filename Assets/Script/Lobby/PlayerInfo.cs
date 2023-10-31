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
    ����,
    ����,
    ���ݼ�,
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
        // ĳ���� ���� �ִϸ��̼� ����
        // Load �ϴ� �� �ȵǼ� �ϴ� ����
        //CharSampleAnim.runtimeAnimatorController = (RuntimeAnimatorController)RuntimeAnimatorController.Instantiate(
        //    Resources.Load($"Animations\\Player_Idle_Sample_{curCharType}")
        //    );
        CharSampleAnim.SetInteger("curNum", curCharType);

        // ������ ����
        playerClassText.text = $"<{ Enum.GetName(typeof(Char_Class_Kor), curCharType)}>";

        // ĳ���� ���� ����
        // ������ �ִٰ� ����
        Sample_Player player = new Sample_Player(curCharType);
        Type type = player.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (FieldInfo field in fields)
        {
            GameObject go = Instantiate(PlayerPrefab);
            TextMeshProUGUI[] texts = go.GetComponentsInChildren<TextMeshProUGUI>();

            texts[0].text = field.Name;
            texts[1].text = field.GetValue(player).ToString();

            // �������� Scene���ٰ� ������Ʈ�� �����ϴ� �ǵ�, transform�� �������� �������� world ��ǥ�� �����ϴ� �ɼ��� �ױ� ������ ������ ��Ÿ��.
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
