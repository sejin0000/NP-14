using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class A1107 : MonoBehaviourPun//주변힐
{
    float time = 0;
    List<PlayerStatHandler> target= new List<PlayerStatHandler>();
    int healP=2;
    public GameObject Player;

 
    public void Init()
    {
        PlayerStatHandler a = transform.parent.gameObject.GetComponent<PlayerStatHandler>();
        target.Add(a);
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
