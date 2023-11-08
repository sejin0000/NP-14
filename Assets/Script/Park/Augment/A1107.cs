using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class A1107 : MonoBehaviour//주변힐
{
    float time = 0;
    List<PlayerStatHandler> target= new List<PlayerStatHandler>();
    int healP=2;
    public GameObject Player;
    private void Start()
    {
        Player = ResultManager.Instance.Player;
        transform.SetParent(Player.transform);
    }
    public void Init(PlayerStatHandler playerstatHandler)
    {
        target.Add(playerstatHandler);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            target.Add(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어입장");
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            target.Remove(collision.GetComponent<PlayerStatHandler>());
            Debug.Log("플레이어퇴장");
        }

    }

    private void FixedUpdate()
    {
        time += Time.deltaTime;
        if (time >= 2f && (target.Count>=1)) 
        {
            if (target.Count >= 1) 
            {
                heal();
                time = 0F;
            }

        }
    }
    private void heal() 
    {
        for (int i = 0; i < target.Count; ++i) 
        {
            target[i].CurHP += healP;
        }
    }
}
