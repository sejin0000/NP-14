using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyMemberButton : MonoBehaviourPun
{
    public TextMeshProUGUI SlotText;
    public Button memberButton;
    public bool IsClicked;

    private void OnEnable()
    {
        IsClicked = false;
        memberButton = GetComponent<Button>();
        if (PhotonNetwork.IsMasterClient)
        {
            memberButton.onClick.AddListener(OnSlotButtonClicked);
        }
    }

    private void OnDisable()
    {
        IsClicked = false;
        if (PhotonNetwork.IsMasterClient)
        {
            memberButton.onClick.RemoveListener(OnSlotButtonClicked);
        }
    }

    [PunRPC]
    public void SlotClicked()
    {
        IsClicked = !IsClicked;
    }


    public void OnSlotButtonClicked()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC("SlotClicked", RpcTarget.All);

        if (IsClicked)
        {
            SlotText.text = "½½·Ô ¿­±â";
            SlotText.color = Color.black;
            GetComponent<Image>().color = new Color(140f / 255f, 140f / 255f, 140f / 255);
            PhotonNetwork.CurrentRoom.MaxPlayers -= 1;
        }
        if (!IsClicked)
        {
            SlotText.text = "½½·Ô ´Ý±â";
            SlotText.color = Color.red;
            GetComponent<Image>().color = new Color(249f / 255f, 250f / 255f, 251f / 255);
            PhotonNetwork.CurrentRoom.MaxPlayers += 1;
        }
    }
}
