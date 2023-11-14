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
                Debug.Log("솔져로 변환");
                statSO.CharacterChange(soldierSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player1Skill>();               
                break;
            case (int)LobbyPanel.CharClass.Shotgun:
                Debug.Log("샷건으로 변환");
                statSO.CharacterChange(shotGunSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player2Skill>();
                break;
            case (int)LobbyPanel.CharClass.Sniper:
                Debug.Log("스나이퍼로 변환");
                statSO.CharacterChange(sniperSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player3Skill>();
                break;
        }
    }

    public void DelComponent(GameObject GO)
    {
        
        if (GO.GetComponent<Player1Skill>())
        {
            GO.GetComponent<Player1Skill>().SkillEnd();            
            Destroy(GO.GetComponent<Player1Skill>());
        }
        if (GO.GetComponent<Player2Skill>())
        {
            GO.GetComponent<Player2Skill>().SkillEnd();
            Destroy(GO.GetComponent<Player2Skill>());
        }
        if (GO.GetComponent<Player3Skill>())
        {
            GO.GetComponent<Player3Skill>().SkillEnd();
            Destroy(GO.GetComponent<Player3Skill>());
        }
    }
}
