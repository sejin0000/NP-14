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
            Debug.Log($"���� ������Ʈ : {playerGo.name}");
            statSO = playerGo.GetComponent<PlayerStatHandler>();
        }
        else
        {
            Debug.Log($"���� ������Ʈ : PlayerContainer");
            statSO = playerContainer.GetComponentInChildren<PlayerStatHandler>();
        }

        switch (charType)
        {
            case (int)LobbyPanel.CharClass.Soldier:
                Debug.Log("������ ��ȯ");
                statSO.CharacterChange(soldierSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player1Skill>();               
                break;
            case (int)LobbyPanel.CharClass.Shotgun:
                Debug.Log("�������� ��ȯ");
                statSO.CharacterChange(shotGunSO);
                DelComponent(statSO.gameObject);
                statSO.gameObject.AddComponent<Player2Skill>();
                break;
            case (int)LobbyPanel.CharClass.Sniper:
                Debug.Log("�������۷� ��ȯ");
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
