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

    [Header("Skills")]
    [SerializeField] private List<Skill> skillList;

    public void SetClassType(int charType, GameObject playerGo = null)
    {
        PlayerStatHandler statSO;
        if (playerGo != null)
        {
            statSO = playerGo.GetComponent<PlayerStatHandler>();
        }
        else
        {
            statSO = playerContainer.GetComponentInChildren<PlayerStatHandler>();
        }

        switch (charType)
        {
            case (int)CharClass.Soldier:                
                statSO.CharacterChange(soldierSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player1Skill>();                
                break;
            case (int)CharClass.Shotgun:                
                statSO.CharacterChange(shotGunSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player2Skill>();
                break;
            case (int)CharClass.Sniper:                
                statSO.CharacterChange(sniperSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player3Skill>();
                break;
        }

        LobbyManager.Instance.audioLibrary.SetupPlayerSE();
    }

    public void DelComponent(GameObject GO)
    {
        if (GO.GetPhotonView().IsMine)
        {
            GO.GetComponent<PlayerInputController>().SkillReset();
        }
        if (GO.GetComponent<Player1Skill>())
        {            
            Destroy(GO.GetComponent<Player1Skill>());
        }
        if (GO.GetComponent<Player2Skill>())
        {            
            Destroy(GO.GetComponent<Player2Skill>());
        }
        if (GO.GetComponent<Player3Skill>())
        {            
            Destroy(GO.GetComponent<Player3Skill>());
        }
    }

    public PlayerStatHandler GetStatData(int classNum)
    {
        PlayerStatHandler statSO = LobbyManager.Instance.instantiatedPlayer.GetComponent<PlayerStatHandler>();
        switch (classNum)
        {
            case (int)CharClass.Soldier:
                statSO.CharacterChange(soldierSO);
                break;
            case (int)CharClass.Shotgun:
                statSO.CharacterChange(shotGunSO);
                break;
            case (int)CharClass.Sniper:
                statSO.CharacterChange(sniperSO);
                break;
        }
        return statSO;
    }
}
