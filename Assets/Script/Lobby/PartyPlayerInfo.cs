using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyPlayerInfo : MonoBehaviourPun
{
    [Header("PlayerInfo")]
    public TextMeshProUGUI playerNickNameText;
    public Image playerImage; 

    private bool isPlayerReady;    

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }


    public void Start()
    {
        // �� ���������� �߰��� ������Ƽ �ִٸ� ���⼭ �߰�.
        //object isReady;
        //if (!PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isPlayerReady", out isReady))
        //{
        //    Hashtable initProps = new Hashtable() { { "IsPlayerReady", isPlayerReady } };
        //    PhotonNetwork.LocalPlayer.SetCustomProperties(initProps);
        //}
              

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object curClassType))
        {
            Debug.Log((int)curClassType);
            if (!(curClassType is int))
            {
                Hashtable charInitialProps = new Hashtable() { { "Char_Class", 0 } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(charInitialProps);
            }
        }
        else
        {
            Hashtable charInitialProps = new Hashtable() { { "Char_Class", 0 } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(charInitialProps);
        }
    }


    // ������ �ϳ� �ν��Ͻ�ȭ�� ��, ����
    public void Initialize(int partyNumber, Player player)
    {
        playerNickNameText.text = player.NickName;

        Sprite playerSprite;
        string spritePath = "Prefabs/CharacterInfoSprites/";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object curClassType);
        playerSprite = Resources.Load<GameObject>($"{spritePath}Player_{curClassType}").GetComponent<Sprite>();
        playerImage.sprite = playerSprite;
    }

    private void OnPlayerNumberingChanged()
    {

    }

    //OnJoinedRoom() �Ĺݺο� �پ�����
    //public void Save()
    //{
    //    int cnt = 0;
    //    // ���� �÷��̾��� custom property ����
    //    foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
    //    {
    //        GameObject playerInfo = Instantiate(PlayerInfo);
    //        playerInfo.transform.SetParent(PartyBox.transform.GetChild(cnt), false);

    //        // �÷��̾� �����տ� ����
    //        playerInfo.GetComponentInChildren<TextMeshProUGUI>().text = player.NickName;
    //        string spritePath = "Prefabs/CharacterInfoSprites/";

    //        Sprite playerSprite;
    //        if (player.CustomProperties.TryGetValue("Char_Class", out object curClassType))
    //        {
    //            Debug.Log((int)curClassType);
    //            if (curClassType is int)
    //            {
    //                playerSprite = Resources.Load<GameObject>($"{spritePath}Player_{curClassType}").GetComponent<Sprite>();
    //            }
    //            else
    //            {
    //                playerSprite = Resources.Load<GameObject>($"{spritePath}Player_0").GetComponent<Sprite>();
    //            }
    //        }
    //        else
    //        {
    //            ExitGames.Client.Photon.Hashtable charInitialProps = new ExitGames.Client.Photon.Hashtable() { { "Char_Class", 0 } };
    //            player.SetCustomProperties(charInitialProps);
    //            playerSprite = Resources.Load<GameObject>($"{spritePath}Player_0").GetComponent<Sprite>();
    //        }

    //        playerInfo.GetComponentInChildren<Image>().sprite = playerSprite;

    //        cnt += 1;
    //        // Ready �Ǻ��� ���� �κ� �߰��ؾ���
    //        // playerInfoPrefab�� ���� ��ũ��Ʈ ���� ��,,
    //    }
    //}
}
