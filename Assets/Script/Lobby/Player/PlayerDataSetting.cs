using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSetting : MonoBehaviourPun
{
    [Header("PlayerSO")]
    public PlayerSO soldierSO;
    public PlayerSO shotGunSO;
    public PlayerSO sniperSO;

    [Header("Player")]
    public GameObject playerContainer;
    public GameObject ownerPlayer;
    public int viewID;

    public void SetClassType(int charType, GameObject playerGo = null)
    {
        PlayerStatHandler statSO;
        if (playerGo != null)
        {
            Debug.Log($"적용 오브젝트 : {playerGo.name}");
            statSO = playerGo.GetComponent<PlayerStatHandler>();
        }
        else
        {
            Debug.Log($"적용 오브젝트 : PlayerContainer");
            statSO = playerContainer.GetComponentInChildren<PlayerStatHandler>();
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
}
