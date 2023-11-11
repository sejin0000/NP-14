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
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object curClassType))
        {
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


    // 프리팹 하나 인스턴스화할 때, 정보
    public void Initialize(int partyNumber, Player player)
    {
        playerNickNameText.text = player.NickName;

        Image playerImage;
        string spritePath = "Prefabs/CharacterInfoSprites/";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Char_Class", out object curClassType);
        playerImage = Resources.Load<GameObject>($"{spritePath}Player_{curClassType}").GetComponent<Image>();
        this.playerImage = playerImage;
    }

    private void OnPlayerNumberingChanged()
    {

    }
}
