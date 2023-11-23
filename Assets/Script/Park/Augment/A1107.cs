using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class A1107 : MonoBehaviourPun //�ֺ���
{
    float time = 0;
    List<PlayerStatHandler> colleagueList = new List<PlayerStatHandler>();
    int healP=2;
    public GameObject Player;

 
    public void Init()
    {
        PlayerStatHandler playerStat = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        if (!colleagueList.Contains(playerStat)) 
        {
            colleagueList.Add(playerStat);
        }

    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
            && collision.gameObject.GetComponent<PlayerStatHandler>()) 
        {
            colleagueList.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log($"�÷��̾� ���� - ���� �÷��̾� �� : {colleagueList.Count()}");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")
            && collision.gameObject.GetComponent<PlayerStatHandler>())
        {
            colleagueList.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log($"�÷��̾� ���� - ���� �÷��̾� �� : {colleagueList.Count()}");
        }

    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 2f && (colleagueList.Count>=1)) 
        {
            if (colleagueList.Count >= 1) 
            {
                Heal();
                time = 0F;
            }
        }
    }
    private void Heal() 
    {
        for (int i = 0; i < colleagueList.Count; ++i) 
        {
            colleagueList[i].HPadd(healP);
            Debug.Log($"�� ��� : {colleagueList[i].photonView.ViewID}  /  ��� ���� ü�� : {colleagueList[i].CurHP}");
        }
    }
}
