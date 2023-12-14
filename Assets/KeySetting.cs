using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeySetting : MonoBehaviour
{
    public void KeySettingPanelClose()
    {
        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(LobbyManager.Instance.RoomP.PopRoomAnnouncePopup());
        }
    }
}
