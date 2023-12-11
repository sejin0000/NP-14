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
        memberButton.onClick.AddListener(OnSlotButtonClicked);
    }

    private void OnDisable()
    {
        IsClicked = false;
        memberButton.onClick.RemoveListener(OnSlotButtonClicked);        
    }

    [PunRPC]
    public void SlotClicked()
    {
        IsClicked = !IsClicked;
        if (IsClicked)
        {
            SlotText.text = "½½·Ô ¿­±â";
            SlotText.color = Color.black;
            GetComponent<Image>().color = new Color(140f / 255f, 140f / 255f, 140f / 255);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers -= 1;
            }
        }
        if (!IsClicked)
        {
            SlotText.text = "½½·Ô ´Ý±â";
            SlotText.color = Color.red;
            GetComponent<Image>().color = new Color(249f / 255f, 250f / 255f, 251f / 255);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers += 1;
            }
        }
    }


    public void OnSlotButtonClicked()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        photonView.RPC("SlotClicked", RpcTarget.AllBuffered);
    }

    public void ResetButton()
    {       
        IsClicked = false;
        SlotText.text = "½½·Ô ´Ý±â";
        SlotText.color = Color.red;
        GetComponent<Image>().color = new Color(249f / 255f, 250f / 255f, 251f / 255);
    }

    [PunRPC]
    public void GetCurrentMemberButtonState(bool curClicked)
    {
        IsClicked = curClicked;
        ApplyCurrentMemberButtonState(curClicked);
    }

    public void ApplyCurrentMemberButtonState(bool curClicked)
    {
        if (curClicked)
        {
            SlotText.text = "½½·Ô ¿­±â";
            SlotText.color = Color.black;
            GetComponent<Image>().color = new Color(140f / 255f, 140f / 255f, 140f / 255);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers -= 1;
            }
        }
        if (!curClicked)
        {
            SlotText.text = "½½·Ô ´Ý±â";
            SlotText.color = Color.red;
            GetComponent<Image>().color = new Color(249f / 255f, 250f / 255f, 251f / 255);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.MaxPlayers += 1;
            }
        }
    }
}
